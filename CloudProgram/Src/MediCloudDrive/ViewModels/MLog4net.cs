using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.IO;

namespace MediCloudDrive.ViewModels
{
    public class MLog4net
    {        
        ILog log = LogManager.GetLogger("RollingFile");        
        public void Debuglog()
        {
            XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"Log4Config.xml"));
            InfoDir();
            log.Debug("Debug");
        }
        public void Infolog()
        {
            XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"Log4Config.xml"));
            InfoDir();
            log.Info("Info");
        }
        public void Warnlog()
        {
            XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"Log4Config.xml"));
            InfoDir();
            log.Warn("Warning");        
        }
        public void Errorlog()
        {
            XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"Log4Config.xml"));
            InfoDir();            
            log.Error("My Error");            
        }
        public void Fatallog()
        {
            XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"Logconfig.xml"));
            InfoDir();            
            log.Fatal("My Fatal Error");
        }
        public void InfoDir()
        {
            string strDirPath = @"Z:\부서함\HC 개발실\2.인수인계\1.생체나이 관련\20. S3CloudDriveApp/log";
            DirectoryInfo dirInfo = new DirectoryInfo(strDirPath);

            if (dirInfo.Exists == false)
            {
                dirInfo.Create();
            }
        }
    }
}
