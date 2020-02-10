using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

using MediCloudDrive.Biz;
using MediCloudDrive.Models.Interfaces;

namespace MediCloudDrive.Models
{
    public class S3SendController : ISendController
    {
        private S3FileTransController AWSS3Controller = new S3FileTransController();

        private BackgroundWorker bw = new BackgroundWorker();

        public event SendResultDlg ResultStatus;

        public event SendResultDlg ResultComplete;

        private List<FileSendInfo> listFileInfo;

        public bool IsBusy { get; set; }

        public S3SendController()
        {
            bw.DoWork += Bw_DoWork;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.WorkerReportsProgress = false;
        }

        public void SetCloudAccountInfo(CloudInfo accountInfo)
        {
            AWSS3Controller.SetAccountInfo(accountInfo);
        }

        public void SendFiles(List<FileSendInfo> lstFileinfo)
        {
            if (lstFileinfo == null) return;
            listFileInfo = lstFileinfo;

            if (lstFileinfo.Count == 0) return;
            if (bw.IsBusy == true) return;

            bw.RunWorkerAsync();
        }

        public void SendComplete(FileSendInfo fileProcessinfo)
        {
            if (fileProcessinfo.FileSendStatus != SendStatus.Fail)
            {
                fileProcessinfo.FileSendStatus = SendStatus.Complete;
                Console.WriteLine("File Process Complete");
            }

            ResultStatus?.Invoke(fileProcessinfo.FileSendStatus, fileProcessinfo.FileFullPath);
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // 모든 작업이 완료되었을때 통지
            ResultComplete?.Invoke(SendStatus.Complete, null);
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            IsBusy = true;

            try
            {
                foreach (var item in listFileInfo)
                {
                    var result = AWSS3Controller.UploadFile(item);
                    item.FileSendStatus = result;
                    SendComplete(item);

                    // 1초뒤에 보냄
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                Console.WriteLine("Worker Error : " + ex.Message);
            }

            IsBusy = false;
        }
    }
}