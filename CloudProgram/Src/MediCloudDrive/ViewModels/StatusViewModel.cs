using GalaSoft.MvvmLight;

using log4net;

namespace MediCloudDrive.ViewModels
{
    public class StatusViewModel : ViewModelBase
    {
        private string strHospitalName;
        private string strCompanyID;
        private MainViewModel ParentModel { get; set; }

        public string HospitalName
        {
            get { return strHospitalName; }
            set { strHospitalName = value; RaisePropertyChanged(); }
        }

        public string CompanyID
        {
            get { return strCompanyID; }
            set { strCompanyID = value; RaisePropertyChanged(); }
        }

        public bool IsConnectState { get; set; }

        private ILog log = LogManager.GetLogger("RollingFile");

        public StatusViewModel(MainViewModel parentModel)
        {
            this.ParentModel = parentModel;
            //여기서 병원이름을 파일에 있는 내용으로 설정

            SetHospitalInfo();

            IsConnectState = true;
        }

        public void SetHospitalInfo()
        {
            HospitalName = this.ParentModel.AppModel.AppSetting.PCName;
            CompanyID = this.ParentModel.AppModel.AppSetting.CompanyID;

            log.Info("Info : " + HospitalName + " : " + CompanyID);
        }
    }
}