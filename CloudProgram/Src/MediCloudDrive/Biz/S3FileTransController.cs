using System;
using System.Diagnostics;
using System.IO;

using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using log4net;
using MediCloudDrive.Models;

namespace MediCloudDrive.Biz
{
    internal class S3FileTransController
    {
        private string bucketName;

        private IAmazonS3 s3Client;

        private TransferUtility fileTransferUtility;

        private MediAES aes = new MediAES();

        private ILog log = LogManager.GetLogger("RollingFile");

        private void SetBucketName(string strName, string strPath)
        {
            Debug.Assert(false == string.IsNullOrEmpty(strName));
            Debug.Assert(false == string.IsNullOrEmpty(strPath));

            bucketName = strName + "/" + strPath;
            Debug.Assert(false == string.IsNullOrEmpty(bucketName));
        }

        public void SetAccountInfo(CloudInfo accountInfo)
        {
            s3Client = new AmazonS3Client(
                accountInfo.SecretKey,
                aes.GetDeCryptString(accountInfo.PrivateKey),
                RegionEndpoint.APNortheast2);

            fileTransferUtility = new TransferUtility(s3Client);

            SetBucketName(accountInfo.StorageName, accountInfo.StoragePath);
        }

        public SendStatus UploadFile(FileSendInfo lstFileinfo)
        {
            SendStatus result = SendStatus.Complete;

            try
            {
                FileInfo file = new FileInfo(lstFileinfo.FileFullPath);
                if (file.Exists == false)
                {
                    Debug.Assert(false, "파일이 존재하지 않습니다 : " + lstFileinfo.FileFullPath);
                    result = SendStatus.Fail;
                }

                string strFilePath = "IH" + DateTime.Now.ToString("yyyyMMddHHmmssfff00") + file.Extension;

                fileTransferUtility.Upload(lstFileinfo.FileFullPath, bucketName, strFilePath);
                Console.WriteLine("Upload Complete : " + lstFileinfo.FileFullPath);
                log.Debug("Upload Complete : " + lstFileinfo.FileFullPath + " / " + strFilePath);
            }
            catch (AmazonS3Exception e)
            {
                Debug.Assert(false, e.Message);
                log.Error(string.Format("Error encountered on server. Message:'{0}' when writing an object", e.Message));
                result = SendStatus.Fail;
            }
            catch (Exception e)
            {
                Debug.Assert(false, e.Message);
                log.Error(string.Format("Unknown encountered on server. Message:'{0}' when writing an object", e.Message));
                result = SendStatus.Fail;
            }

            return result;
        }
    }
}