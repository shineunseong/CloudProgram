using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

using log4net;

using MediCloudDrive.Models;
using MediCloudDrive.Models.Interfaces;
using MediCloudDrive.ViewModels.DetailModels;

namespace MediCloudDrive.ViewModels
{
    public class MainListViewModel : ViewModelBase
    {
        private MainViewModel ParentModel { get; set; }

        public ICommand CmdOpenFile { get; set; }

        public ICommand CmdOpenFolder { get; set; }

        public ICommand CmdClearItem { get; set; }

        public ICommand CmdClearAll { get; set; }

        public FileItemModel SelectedItem { get; set; }

        private StringBuilder sb = new StringBuilder(32);

        private ISendController FileSender;

        // 메인화면 갱신 주기 (옵션에 따라 1시간/1일/1주)-> 1일/1주는 다음날 00시 기준으로 리셋하는건지?
        private System.Timers.Timer time = new System.Timers.Timer();

        public ObservableCollection<FileItemModel> ListItems { get; set; }

        private ILog log = LogManager.GetLogger("RollingFile");

        public MainListViewModel(MainViewModel parentModel)
        {
            this.ParentModel = parentModel;

            ListItems = new ObservableCollection<FileItemModel>();

            FileSender = new S3SendController();
            if (false == string.IsNullOrEmpty(ParentModel.AppModel.S3Info.StorageName) &&
                false == string.IsNullOrEmpty(ParentModel.AppModel.S3Info.StoragePath) &&
                false == string.IsNullOrEmpty(ParentModel.AppModel.S3Info.SecretKey) &&
                false == string.IsNullOrEmpty(ParentModel.AppModel.S3Info.PrivateKey))
            {
                SetFileTransferAccount();
            }

            FileSender.ResultStatus += FileSender_ResultStatus;
            FileSender.ResultComplete += FileSender_ResultComplete;

            time.Interval = new TimeSpan(0, 1, 0).TotalMilliseconds; //1분 단위로 확인하면서
            time.Elapsed += Time_Elapsed;
            time.Start();

            CmdOpenFile = new RelayCommand(OpenFileCommand);
            CmdOpenFolder = new RelayCommand(OpenFolderCommand);
            CmdClearItem = new RelayCommand(ClearItemCommand);
            CmdClearAll = new RelayCommand(ClearAllCommand);
        }

        private void ClearAllCommand()
        {
            // 전송 완료된것만 삭제

            for (int i = ListItems.Count - 1; i >= 0; i--)
            {
                if (ListItems[i].SendStatusEnum == SendStatus.Complete)
                    ListItems.Remove(ListItems[i]);
            }
        }

        private void ClearItemCommand()
        {
            if (SelectedItem == null) return;

            // 선택한 아이템 삭제
            ListItems.Remove(SelectedItem);
            SelectedItem = null;
        }

        private void OpenFolderCommand()
        {
            // 선택한 아이템의 폴더 열기
            if (SelectedItem == null) return;

            System.Diagnostics.Process.Start("explorer.exe", "/select, " + SelectedItem.FileFullPath);
        }

        private void OpenFileCommand()
        {
            //선택한 아이템의 파일 바로 열기
            if (SelectedItem == null) return;

            System.Diagnostics.Process.Start(SelectedItem.FileFullPath);
        }

        public void SetFileTransferAccount()
        {
            FileSender.SetCloudAccountInfo(ParentModel.AppModel.S3Info);
        }

        private void FileSender_ResultComplete(SendStatus resultStatus, string strFileName)
        {
            // 전송동작 완료l
            // status bar에 상태내용을 나타내주는것이 좋을듯
        }

        private void FileSender_ResultStatus(SendStatus resultStatus, string strFileName)
        {
            var newlist = from item in ListItems
                          where (item.FileFullPath == strFileName)
                          select item;

            foreach (var item in newlist)
            {
                item.IsSending = false;
                item.SendStatusEnum = resultStatus;
            }
        }

        private void Time_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (ListItems.Count == 0) return;

            // 완료된 항목 시간비교 후 삭제하는 부분
            ClearCompleteList();
        }

        private void ClearCompleteList()
        {
            int nCompareTime = (int)ParentModel.AppModel.AppSetting.RemainDis;

            Console.WriteLine("///");
            Console.WriteLine("Delete Time : " + DateTime.Now.ToString("yyyy/MM/dd-HH:mm:ss"));
            for (int i = ListItems.Count - 1; i >= 0; i--)
            {
                if (ListItems[i].SendStatusEnum == SendStatus.Complete)
                {
                    TimeSpan ts = DateTime.Now - ListItems[i].ListAddTime;
                    // 해당 시간 간격 이상이 되는 항목은 삭제하는걸로
                    //if (ts.Hours >= nCompareTime)
                    if (ts.Minutes >= nCompareTime)
                    {
                        Application.Current.Dispatcher.Invoke(delegate ()
                        {
                            Console.WriteLine("Deleted Item : " + ListItems[i].FileName + " / " + ListItems[i].ListAddTime.ToString("yyyy/MM/dd-HH:mm:ss"));
                            ListItems.Remove(ListItems[i]);

                            log.Debug("File Item Deleted : " + ListItems[i].FileName);
                        });
                    }
                }
            }
            Console.WriteLine("---");
        }

        public void AddFile(string strFileFullPath)
        {
            FileInfo file = new FileInfo(strFileFullPath);

            if (file.Extension.Contains("pdf") == false &&
                file.Extension.Contains("jpg") == false &&
                file.Extension.Contains("bmp") == false &&
                file.Extension.Contains("gif") == false &&
                file.Extension.Contains("png") == false)
            {
                return;
            }

            StrFormatByteSizeW(file.Length, sb, sb.Capacity);

            ListItems.Add(new FileItemModel()
            {
                FileExt = (file.Extension.Contains("pdf") == true) ? FileExtend.PDF : FileExtend.IMG,
                ForderPath = file.DirectoryName,
                FileName = file.Name,
                FileSize = sb.ToString(),
                IsSending = false,
                SendStatusEnum = SendStatus.Ready,
                ListAddTime = DateTime.Now,
                FileFullPath = strFileFullPath
            });
        }

        public void SendProcess()
        {
            if (ListItems.Count == 0) return;

            Debug.Assert(FileSender != null);

            var newlist = from item in ListItems
                          where ((item.SendStatusEnum != SendStatus.Complete) && (item.SendStatusEnum != SendStatus.Sending))
                          select item;

            if (newlist.Count() == 0) return;

            var rtQuery = from item in newlist
                          select new FileSendInfo()
                          {
                              FileFullPath = item.FileFullPath,
                              FileSendStatus = SendStatus.Ready,
                          };

            if (newlist.Count() == 0)
            {
                return;
            }

            Debug.Assert(rtQuery.Count() != 0);

            FileSender.SendFiles(rtQuery.ToList());

            foreach (var item2 in newlist)
            {
                int nIndex = ListItems.IndexOf(item2);
                if (nIndex < 0) continue;
                ListItems[nIndex].IsSending = true;
                ListItems[nIndex].SendStatusEnum = SendStatus.Sending;
            }
        }

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern long StrFormatByteSizeW(long qdw, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszBuf, int cchBuf);
    }
}