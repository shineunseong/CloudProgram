namespace MediCloudDrive.Models
{
    public class AppOption
    {
        public bool IsStartUpApp { get; set; }

        public DistanceTime RemainDis { get; set; }

        public string PCName { get; set; }

        public string CompanyID { get; set; }

        public bool IsInstantSend { get; set; }

        public bool IsESCClose { get; set; }

        public AppOption()
        {
            IsStartUpApp = true;
            RemainDis = DistanceTime.OneDay;
            IsESCClose = false;
            IsInstantSend = true;
        }
    }
}