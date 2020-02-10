namespace MediCloudDrive.Biz.Interfaces
{
    public interface IMediLogger
    {
        void Debug(string strMsg);

        void Info(string strMsg);

        void Error(string strMsg);
    }
}