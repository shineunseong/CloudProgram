using System;
using System.IO;
using System.Windows;

using log4net;
using log4net.Appender;
using log4net.Config;

using MediCloudDrive.Biz;
using MediCloudDrive.Models;

namespace MediCloudDrive
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private ILog log = LogManager.GetLogger("RollingFile");

        protected override void OnStartup(StartupEventArgs e)
        {
            // Setting log4net
            var logCollect = XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"Log4Config.xml"));
            var logger = LogManager.GetRepository();
            foreach (var item in logger.GetAppenders())
            {
                RollingFileAppender rolling = item as RollingFileAppender;
                if (null != rolling)
                {
                    rolling.File = Path.Combine(MediConstants.ConfigPath, MediConstants.AppLog, "DriveLog.log");
                    rolling.ActivateOptions();
                }
            }
            // end setting log4net

            // Log4Net 설정 호출
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            log.Debug("Start Logger");

            // App 초기 설정 호출
            MediCloudModel appModel = GetConfigSetting();

            // TODO : appModel.AppSetting.IsStartUpApp을 확인해서 false가 아닌 경우 레지스트리를 확인해서 run에서 삭제되었는지 확인할 필요

            // 메인 윈도 출력
            MainWindow mainWin = new MainWindow();
            mainWin.SetInitView(appModel);
            mainWin.ShowDialog();

            base.OnStartup(e);
        }

        private void App_DispatcherUnhandledException(
            object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // log에 기록
            log.Error(e.Exception.Message);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private MediCloudModel GetConfigSetting()
        {
            // 설정파일 load해서 가져오기
            // 파일이 없는 초기 상태면 기본값을 만들어서 넣고,
            // 파일이 있으면 해당 파일을 XML Deserialize 해서 가져온다

            var logPath = Path.Combine(MediConstants.ConfigPath, MediConstants.AppLog);
            log.Debug("Log path : " + logPath);

            if (Directory.Exists(logPath) == false)
            {
                try
                {
                    Directory.CreateDirectory(logPath);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }

            string configFullPath = Path.Combine(MediConstants.ConfigPath, MediConstants.ConfigFile);

            MediCloudModel optionModel = null;
            if (File.Exists(configFullPath) == true)
            {
                optionModel = MediSerialize<MediCloudModel>.DeserializeModel(configFullPath);
            }
            else
            {
                optionModel = new MediCloudModel();
#if DEBUG
                optionModel.AppSetting = new AppOption()
                {
                    IsStartUpApp = true,
                    RemainDis = DistanceTime.OneDay,
                };
#endif
            }

            return optionModel;
        }
    }
}