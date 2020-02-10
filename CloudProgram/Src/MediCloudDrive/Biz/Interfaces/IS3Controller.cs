using System.Collections.Generic;

using MediCloudDrive.Models;

namespace MediCloudDrive.Biz.Interfaces
{
    public delegate void DlgStatus();

    public interface IS3Controller
    {
        event DlgStatus S3Status;

        void SendFiles(List<FileSendInfo> lstFileinfo);
    }
}