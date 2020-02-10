using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using log4net;
using Microsoft.Win32;

namespace MediCloudDrive.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private MainViewModel ParentModel { get; set; }

        public ICommand CmdAddFile { get; set; }

        public ICommand CmdSend { get; set; }

        public ICommand CmdMsg { get; set; }

        public bool IsMsg { get; set; }

        public string ErrMsg { get; set; }

        /// <summary>
        /// True면 전송버튼 비활성화됨
        /// </summary>
        public bool IsSending { get; set; }

        public ICommand CmdOption { get; set; }

        private ILog log = LogManager.GetLogger("RollingFile");

        public MenuViewModel(MainViewModel parentModel)
        {
            this.ParentModel = parentModel;

            CmdAddFile = new RelayCommand(AddFileList);
            CmdSend = new RelayCommand(DoSend);
            CmdMsg = new RelayCommand(ShowMsg);
            CmdOption = new RelayCommand(ShowOption);
        }

        private void AddFileList()
        {
            Debug.Assert(ParentModel != null);

            OpenFileDialog open = new OpenFileDialog
            {
                Multiselect = true                
            };
            open.DefaultExt = ".pdf";
            open.Filter = "Pdf documents (.pdf; .jpg; .bmp; .gif; .png)|*.pdf; *.jpg; *.bmp; *.gif; *.png";

            if (open.ShowDialog() == false) return;

            string[] strSelectedFiles = open.FileNames;

            Debug.Assert(strSelectedFiles.Length != 0);
            foreach (var item in strSelectedFiles)
            {
                ParentModel.ListViewModel.AddFile(item);
                log.Debug(item);
            }
            if (ParentModel.AppModel.AppSetting.IsInstantSend == true)
            {
                DoSend();
            }
        }

        private void DoSend()
        {
            // 파일을 전송하는 프로세스를 호출
            ParentModel.ListViewModel.SendProcess();
        }

        private void ShowMsg()
        {
            MessageBox.Show(ErrMsg, "MediAge Information");
        }

        private void ShowOption()
        {
            MainWindow mainWin = Application.Current.MainWindow as MainWindow;
            mainWin.mainview.ShowOptionView();
        }
    }
}