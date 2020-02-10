using System;
using System.IO;

namespace MediCloudDrive.Models
{
    public static class MediConstants
    {
        /// <summary>
        /// mediclouddrv
        /// </summary>
        public static string RegName = "mediclouddrv";

        /// <summary>
        /// CloudDrive
        /// </summary>
        public static string AppName = "CloudDrive";

        /// <summary>
        /// Config.xml
        /// </summary>
        public static string ConfigFile = "Config.xml";

        /// <summary>
        /// MediAge
        /// </summary>
        public static string AppCompanyName = "MediAge";

        public static string AppLog = "Logs";

        /// <summary>
        /// 내문서\Mediage\CloudDrive
        /// </summary>
        public static string ConfigPath { get; set; }

        static MediConstants()
        {
            ConfigPath =
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    AppCompanyName,
                    AppName);
        }
    }
}