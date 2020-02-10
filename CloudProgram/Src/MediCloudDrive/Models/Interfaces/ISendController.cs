using System.Collections.Generic;

namespace MediCloudDrive.Models.Interfaces
{
    public delegate void SendResultDlg(SendStatus resultStatus, string strFileName);

    public interface ISendController
    {
        bool IsBusy { get; set; }

        event SendResultDlg ResultStatus;

        event SendResultDlg ResultComplete;

        void SendFiles(List<FileSendInfo> lstFileFullPath);

        void SetCloudAccountInfo(CloudInfo accountInfo);

        void SendComplete(FileSendInfo fileProcessinfo);
    }
}