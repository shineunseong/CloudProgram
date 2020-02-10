using System;

using GalaSoft.MvvmLight;

using MediCloudDrive.Models;

namespace MediCloudDrive.ViewModels.DetailModels
{
    public class FileItemModel : ViewModelBase
    {
        private SendStatus eSendStatus;
        private bool bSending;

        public FileExtend FileExt { get; set; }

        public string FileName { get; set; }

        public string ForderPath { get; set; }

        public string FileSize { get; set; }

        public SendStatus SendStatusEnum
        {
            get { return eSendStatus; }
            set { eSendStatus = value; RaisePropertyChanged(); }
        }

        public bool IsSending
        {
            get { return bSending; }
            set { bSending = value; RaisePropertyChanged(); }
        }

        public DateTime ListAddTime { get; set; }

        public string FileFullPath { get; set; }
    }
}