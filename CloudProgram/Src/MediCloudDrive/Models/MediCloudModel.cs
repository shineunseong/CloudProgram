using System;
using System.IO;
using MediCloudDrive.Biz;

namespace MediCloudDrive.Models
{
    public class MediCloudModel
    {
        public CloudInfo S3Info { get; set; }

        public AppOption AppSetting { get; set; }

        public MediCloudModel()
        {
            S3Info = new CloudInfo();
            AppSetting = new AppOption();
        }

        public static void SetConfigModel(MediCloudModel optionModel)
        {
            string configFullPath = Path.Combine(MediConstants.ConfigPath, MediConstants.ConfigFile);

            if (Directory.Exists(MediConstants.ConfigPath) == false)
            {
                try
                {
                    Directory.CreateDirectory(MediConstants.ConfigPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            MediSerialize<MediCloudModel>.SerializeObject(optionModel, configFullPath);
        }
    }
}