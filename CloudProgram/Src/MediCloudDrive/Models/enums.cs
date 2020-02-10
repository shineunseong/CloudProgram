namespace MediCloudDrive.Models
{
    public enum SendStatus
    {
        Ready,             // 전송준비
        Fail,                // 전송완료
        Complete,        // 전송실패
        Sending             // 전송중
    }

    public enum DistanceTime : int
    {
        OneMinute = 1,
        OneHour = 60,
        OneDay = 1440,
        OneWeek = 10080
    }

    public enum FileExtend
    {
        PDF,    //pdf 파일
        IMG     //이미지파일 전체
    }
}