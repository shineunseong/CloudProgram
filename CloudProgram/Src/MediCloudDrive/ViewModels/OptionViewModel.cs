using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using log4net;
using MediCloudDrive.Biz;
using MediCloudDrive.Models;

namespace MediCloudDrive.ViewModels
{
    public class OptionViewModel : ViewModelBase
    {
        private MainViewModel ParentModel { get; set; }

        private string strHospitalName;
        private string strCompanyID;

        public string AppVer { get; set; }

        public bool IsStartUp { get; set; }

        public bool IsRightNow { get; set; }

        public bool IsEnableESC { get; set; }

        public DistanceTime DisplayTime { get; set; }

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

        public CloudInfo CloudInfoModel { get; set; }

        public ICommand CmdSave { get; set; }

        public ICommand CmdConfirm { get; set; }

        public ICommand CmdCancel { get; set; }

        public Action GetCloudKey { get; internal set; }

        private MediAES aes = new MediAES();

        private ILog log = LogManager.GetLogger("RollingFile");

        public OptionViewModel(MainViewModel parentModel)
        {
            this.ParentModel = parentModel;

            var AppOpt = ParentModel.AppModel.AppSetting;

            AppVer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            IsStartUp = AppOpt.IsStartUpApp;
            IsRightNow = AppOpt.IsInstantSend;
            IsEnableESC = AppOpt.IsESCClose;
            DisplayTime = AppOpt.RemainDis;
            CompanyID = AppOpt.CompanyID;
            HospitalName = AppOpt.PCName;

            var info = ParentModel.AppModel.S3Info;
            CloudInfoModel = new CloudInfo()
            {
                StorageName = info.StorageName,
                StoragePath = info.StoragePath,
                SecretKey = info.SecretKey,
                PrivateKey = info.PrivateKey
            };

            CmdSave = new RelayCommand(SaveCommand);
            CmdConfirm = new RelayCommand(ConfirmCommand);
            CmdCancel = new RelayCommand(CancelCommand);
        }

        private bool CheckValidationInfo()
        {
            Debug.Assert(GetCloudKey != null);
            GetCloudKey();

            bool bCheck = true;

            // s3 정보 4개 확인

            if (true == string.IsNullOrEmpty(CompanyID) ||
               true == string.IsNullOrEmpty(HospitalName))
            {
                bCheck = false;
            }

            if (true == string.IsNullOrEmpty(CloudInfoModel.StorageName) ||
                true == string.IsNullOrEmpty(CloudInfoModel.StoragePath) ||
                true == string.IsNullOrEmpty(CloudInfoModel.SecretKey) ||
                true == string.IsNullOrEmpty(CloudInfoModel.PrivateKey))
            {
                bCheck = false;
            }

            return bCheck;
        }

        private void ConfirmCommand()
        {
            // 입력값 확인
            if (CheckValidationInfo() == false)
            {
                MessageBox.Show("입력한 데이터가 맞지 않습니다");
                return;
            }

            // Model 저장
            SaveAppModel();

            // 클라우드 정보 설정
            ParentModel.ListViewModel.SetFileTransferAccount();

            // 상태바에 병원정보 출력
            ParentModel.StatusModel.SetHospitalInfo();

            // 화면 닫기
            MainWindow mainWin = Application.Current.MainWindow as MainWindow;
            mainWin.mainview.CloseOptionView();

            log.Debug("Confirm Command Option");
        }

        private void SaveCommand()
        {
            // 입력값 확인
            if (CheckValidationInfo() == false)
            {
                MessageBox.Show("입력한 데이터가 맞지 않습니다");
                return;
            }

            // Model 저장
            SaveAppModel();

            // 클라우드 정보 설정
            ParentModel.ListViewModel.SetFileTransferAccount();

            // 상태바에 병원정보 출력
            ParentModel.StatusModel.SetHospitalInfo();

            // 화면닫기 없음
            log.Debug("Save Command Option");
        }

        private void CancelCommand()
        {
            /// pjh2104-191218
            /// 프로그램 첫 설정시에는 해당 루트로 인해 옵션창에서 벗어날 수 없도록 해야한다
            /// 기본 데이터 저장 확인(기본 데이터 자체가 없는 상황이면 경고를 내고 필수입력하도록 한다)
            if (CheckValidationInfo() == false)
            {
                MessageBox.Show("기본 데이터를 입력해야합니다. " + Environment.NewLine + "병원정보와 클라우드 정보를 필히 입력해주세요. 해당 정보는 메디에이지(070-4947-6308)에서 입력합니다.", "MediAge - Cloud Drive");
                return;
            }

            // 화면 닫기
            MainWindow mainWin = Application.Current.MainWindow as MainWindow;
            mainWin.mainview.CloseOptionView();

            log.Debug("Cancel Option");
        }

        private bool CheckStoragePath()
        {
            string strPath = CloudInfoModel.StoragePath;
            // 삭제
            if (strPath.Length > 3)
            {
                // 맨 앞자리 '/' 삭제
                if (strPath[0] == '/') strPath = strPath.Remove(0, 1);
                // 맨 뒷자리 '/' 삭제
                if (strPath[strPath.Length - 1] == '/') strPath = strPath.Remove(strPath.Length - 1, 1);
            }
            CloudInfoModel.StoragePath = strPath;

            return true;
        }

        private void SetPrivateKey(string strPrivateDeSKey)
        {
            string strEncryptSecretKey = aes.GetEncryptString(strPrivateDeSKey);

            CloudInfoModel.PrivateKey = strEncryptSecretKey;
        }

        private void SaveAppModel()
        {
            log.Debug("Start Cloud Setting");
            var AppOpt = ParentModel.AppModel.AppSetting;
            AppOpt.CompanyID = this.CompanyID;
            AppOpt.IsESCClose = this.IsEnableESC;
            AppOpt.IsInstantSend = this.IsRightNow;
            AppOpt.PCName = this.HospitalName;
            AppOpt.RemainDis = this.DisplayTime;
            AppOpt.IsStartUpApp = this.IsStartUp;

            var cloudInfo = ParentModel.AppModel.S3Info;
            cloudInfo.StorageName = CloudInfoModel.StorageName;
            cloudInfo.StoragePath = CloudInfoModel.StoragePath;
            cloudInfo.SecretKey = CloudInfoModel.SecretKey;
            cloudInfo.PrivateKey = CloudInfoModel.PrivateKey;

            MediCloudModel.SetConfigModel(ParentModel.AppModel);
            log.Debug("Complete Cloud Setting");
        }

        private void SetStartUpInfo()
        {
            if (true == this.IsStartUp)
            {
                // 윈도우 시작시 동시 실행 설정
            }
            else
            {
                // 윈도우 시작시 실행 해제
            }
        }
    }
}