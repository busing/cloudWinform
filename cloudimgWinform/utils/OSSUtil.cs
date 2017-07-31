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


        public static String PutObject(String bucketName, UploadTask uploadTask)
        {
            if (client == null)
            {
                initOSSClient();
            }
            string responseContent = null;
            try
            {
                Debug.WriteLine(Dictionary.API + "images?" + uploadTask.toCallBackString());
                Debug.WriteLine("bucket=" + Dictionary.OSSConfig.Buket);
                var metadata = BuildCallbackMetadata(Dictionary.API + "images?"+ uploadTask.toCallBackString(), "bucket="+ Dictionary.OSSConfig.Buket);
                using (var fs = File.Open(uploadTask.path, FileMode.Open))
                {
                    var putObjectRequest = new PutObjectRequest(bucketName, uploadTask.uploadPath, fs, metadata);
                    putObjectRequest.StreamTransferProgress += streamProgressCallback;
                    var result = client.PutObject(putObjectRequest);
                    responseContent = GetCallbackResponse(result);
                }
                Console.WriteLine("Put object:{0} succeeded, callback response content:{1}", uploadTask.uploadPath, responseContent);
            }
            catch (OssException ex)
            {
                Debug.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                if (retryTimes <= 3)
                {
                    Debug.WriteLine("retry times " + retryTimes);
                    initOSSClient();
                    return PutObject(bucketName, uploadTask);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed with error info: {0}", ex.Message);
            }
            return responseContent;
        }

        private static string GetCallbackResponse(PutObjectResult putObjectResult)
        {
            string callbackResponse = null;
            using (var stream = putObjectResult.ResponseStream)
            {
                var buffer = new byte[4 * 1024];
                var bytesRead = stream.Read(buffer, 0, buffer.Length);
                callbackResponse = Encoding.Default.GetString(buffer, 0, bytesRead);
            }
            return callbackResponse;
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
            double precent = (double)(args.TransferredBytes / args.TotalBytes);
            Progress.getProgress().progress = (int)Math.Floor(precent * 100);
        }
    }


}
