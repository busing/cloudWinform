using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aliyun.OSS;
using System.IO;
using System.Threading;
using Aliyun.OSS.Common;
using Aliyun.OSS.Util;
using cloudimgWinform.bean;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace cloudimgWinform.utils.oss
{
    
    public class OSSUpload
    {
        static OssClient client;
        static String accessKeyId;
        static String accessKeySecret;
        static String securityToken;
        static int partSize = 50 * 1024 * 1024;
        static AutoResetEvent _event = new AutoResetEvent(false);
        static int retryTimes = 0;

        public static OssClient initOSSClient()
        {
            getSts();
            client = new OssClient(Dictionary.OSSConfig.Endpoint, accessKeyId, accessKeySecret, securityToken);
            return client;
        }


        public static void getSts()
        {
            String loginUrl = Dictionary.API + "securitytokens";
            JObject jsonObj = HttpHelper.CreatePostHttpResponse(loginUrl, null, 5000, "", new UTF8Encoding(), null);
            Debug.WriteLine(jsonObj.ToString());
            Object userInfo = jsonObj["returnObject"];
            if (int.Parse(jsonObj["responseCode"].ToString()) == 0)
            {
                accessKeyId = (string)jsonObj["returnObject"]["accessKeyId"];
                accessKeySecret = (string)jsonObj["returnObject"]["accessKeySecret"];
                securityToken = (string)jsonObj["returnObject"]["securityToken"];
            }
        }


        /// <summary>
        /// 分片上传。
        /// </summary>
        public static CompleteMultipartUploadResult UploadMultipart(String bucketName, UploadTask uploadTask)
        {
            if (client == null)
            {
                initOSSClient();
            }
            try
            {
                var uploadId = InitiateMultipartUpload(bucketName, uploadTask);
                var partETags = UploadParts(bucketName, uploadTask.uploadPath, uploadTask.path, uploadId, partSize);
                CompleteMultipartUploadResult request = CompleteUploadPart(bucketName,  uploadId, partETags, uploadTask);
                Debug.WriteLine(String.Format("Multipart put object:{0} succeeded", uploadTask.uploadPath));
                retryTimes = 0;
                return request;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                if (retryTimes < 3)
                {
                    retryTimes++;
                    Debug.WriteLine("retry times " + retryTimes);
                    initOSSClient();
                    return UploadMultipart(bucketName, uploadTask);
                }
                else
                {
                    return null;
                }
            }
        }

        private static string InitiateMultipartUpload(String bucketName,UploadTask uploadTask)
        {
            var request = new InitiateMultipartUploadRequest(bucketName, uploadTask.uploadPath);
            var result = client.InitiateMultipartUpload(request);
            return result.UploadId;
        }

        private static List<PartETag> UploadParts(String bucketName, String objectName, String fileToUpload,
                                                  String uploadId, int partSize)
        {
            var fi = new FileInfo(fileToUpload);
            var fileSize = fi.Length;
            var partCount = fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            var partETags = new List<PartETag>();
            using (var fs = File.Open(fileToUpload, FileMode.Open))
            {
                for (var i = 0; i < partCount; i++)
                {
                    var skipBytes = (long)partSize * i;
                    fs.Seek(skipBytes, 0);
                    var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                    var request = new UploadPartRequest(bucketName, objectName, uploadId)
                    {
                        InputStream = fs,
                        PartSize = size,
                        PartNumber = i + 1
                    };
                    request.StreamTransferProgress += streamProgressCallback;
                    var result = client.UploadPart(request);

                    partETags.Add(result.PartETag);
                    Debug.WriteLine(String.Format("finish {0}/{1}", partETags.Count, partCount));
                }
            }
            return partETags;
        }

        

        private static CompleteMultipartUploadResult CompleteUploadPart(String bucketName,
            String uploadId, List<PartETag> partETags,UploadTask uploadTask)
        {
            var metadata = BuildCallbackMetadata(Dictionary.API+ "images", uploadTask.toCallBackString());
            var completeMultipartUploadRequest =
                new CompleteMultipartUploadRequest(bucketName, uploadTask.uploadPath, uploadId)
                {
                    Metadata = metadata
                };
            foreach (var partETag in partETags)
            {
                completeMultipartUploadRequest.PartETags.Add(partETag);
            }
            return client.CompleteMultipartUpload(completeMultipartUploadRequest);
        }


        private static ObjectMetadata BuildCallbackMetadata(string callbackUrl, string callbackBody)
        {
            string callbackHeaderBuilder = new CallbackHeaderBuilder(callbackUrl, callbackBody).Build();
            string CallbackVariableHeaderBuilder = new CallbackVariableHeaderBuilder().Build();
            var metadata = new ObjectMetadata();
            metadata.AddHeader(HttpHeaders.Callback, callbackHeaderBuilder);
            metadata.AddHeader(HttpHeaders.CallbackVar, CallbackVariableHeaderBuilder);
            return metadata;
        }

        public static void streamProgressCallback(object sender, StreamTransferProgressArgs args)
        {
            double precent = (double)args.TransferredBytes / args.TotalBytes;
            Progress.currentProgress.progress = (int)Math.Floor(precent * 100);
        }
    }


}
