using System.Threading.Tasks;

namespace MediCloudDrive.Models
{
    public class FileSendInfo
    {
        public string FileFullPath { get; set; }

        public SendStatus FileSendStatus { get; set; }
    }
}