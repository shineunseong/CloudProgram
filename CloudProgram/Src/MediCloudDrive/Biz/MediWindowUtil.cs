using System;
using System.Diagnostics;
using System.IO;

using MediCloudDrive.Models;

using Microsoft.Win32;

namespace MediCloudDrive.Biz
{
    public static class MediWindowUtil
    {
        public static void SetAutoStartUpWindowStart(bool bEnable)
        {
            try
            {
                RegistryKey runRegKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                // 레지스트리
                var RegValue = runRegKey.GetValue(MediConstants.RegName);
                if (bEnable == true)
                {
                    // 설정해주는곳
                    if (RegValue == null)
                    {
                        runRegKey.SetValue(
                            MediConstants.RegName,
                            Path.Combine(Environment.CurrentDirectory, AppDomain.CurrentDomain.FriendlyName));
                    }
                }
                else
                {
                    //삭제해주는곳
                    if (RegValue != null)
                    {
                        runRegKey.DeleteValue(MediConstants.RegName);
                    }
                }
            }
            catch (Exception ex)
            {
                // log에 저장
                Debug.Assert(false, ex.Message);
                Debug.WriteLine(ex.Message);
            }
        }
    }
}