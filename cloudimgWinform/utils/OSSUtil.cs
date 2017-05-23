using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aliyun.OSS;
using System.IO;
using System.Threading;
using Aliyun.OSS.Common;
using Aliyun.OSS.Util;
using cloudimgWinform.bean;
using System.Windows.Forms;

namespace cloudimgWinform.utils.oss
{
    internal class OSSConfig
    {
        public static string AccessKeyId = "LTAImUeM8LsKP2lg";

        public static string AccessKeySecret = "5nFEgCX6PpeMkNTgfgeyX5wbMe4Aat";

        public static string Endpoint = "http://oss-cn-hangzhou.aliyuncs.com";

        public static string UploadPath = "cloud/images/";

        public static string Buket = "terrydr-hd";
    }

    public class OSSUpload
    {
        static string accessKeyId = OSSConfig.AccessKeyId;
        static string accessKeySecret = OSSConfig.AccessKeySecret;
        static string endpoint = OSSConfig.Endpoint;
        static OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret);
        static int partSize = 50 * 1024 * 1024;
        static AutoResetEvent _event = new AutoResetEvent(false);

        public class UploadPartContext
        {
            public string BucketName { get; set; }
            public string ObjectName { get; set; }

            public List<PartETag> PartETags { get; set; }

            public string UploadId { get; set; }
            public long TotalParts { get; set; }
            public long CompletedParts { get; set; }
            public object SyncLock { get; set; }
            public ManualResetEvent WaitEvent { get; set; }
        }

        public class UploadPartContextWrapper
        {
            public UploadPartContext Context { get; set; }
            public int PartNumber { get; set; }
            public Stream PartStream { get; set; }

            public UploadPartContextWrapper(UploadPartContext context, Stream partStream, int partNumber)
            {
                Context = context;
                PartStream = partStream;
                PartNumber = partNumber;
            }
        }

        public class UploadPartCopyContext
        {
            public string TargetBucket { get; set; }
            public string TargetObject { get; set; }

            public List<PartETag> PartETags { get; set; }

            public string UploadId { get; set; }
            public long TotalParts { get; set; }
            public long CompletedParts { get; set; }
            public object SyncLock { get; set; }
            public ManualResetEvent WaitEvent { get; set; }
        }

        public class UploadPartCopyContextWrapper
        {
            public UploadPartCopyContext Context { get; set; }
            public int PartNumber { get; set; }

            public UploadPartCopyContextWrapper(UploadPartCopyContext context, int partNumber)
            {
                Context = context;
                PartNumber = partNumber;
            }
        }

        /// <summary>
        /// 分片上传。
        /// </summary>
        public static void UploadMultipart(String bucketName,String fileToUpload,String key)
        {
            var uploadId = InitiateMultipartUpload(bucketName, key);
            var partETags = UploadParts(bucketName, key, fileToUpload, uploadId, partSize);
            CompleteMultipartUploadResult request=CompleteUploadPart(bucketName, key, uploadId, partETags);
            Console.WriteLine("Multipart put object:{0} succeeded", key);
        }

        /// <summary>
        /// 异步分片上传。
        /// </summary>
        public static void AsyncUploadMultipart(String bucketName, String fileToUpload, String key)
        {
            var uploadId = InitiateMultipartUpload(bucketName, key);
            AsyncUploadParts(bucketName, key, fileToUpload, uploadId, partSize);
        }

        /// <summary>
        /// 分片拷贝。
        /// </summary>
        public static void UploadMultipartCopy(String targetBucket, String targetObject, String sourceBucket, String sourceObject)
        {
            var uploadId = InitiateMultipartUpload(targetBucket, targetObject);
            var partETags = UploadPartCopys(targetBucket, targetObject, sourceBucket, sourceObject, uploadId, partSize);
            var completeResult = CompleteUploadPart(targetBucket, targetObject, uploadId, partETags);

            Console.WriteLine(@"Upload multipart copy result : ");
            Console.WriteLine(completeResult.Location);
        }

        /// <summary>
        /// 异步分片拷贝。
        /// </summary>
        public static void AsyncUploadMultipartCopy(String targetBucket, String targetObject, String sourceBucket, String sourceObject)
        {
            var uploadId = InitiateMultipartUpload(targetBucket, targetObject);
            AsyncUploadPartCopys(targetBucket, targetObject, sourceBucket, sourceObject, uploadId, partSize);
        }
        /// <summary>
        /// 列出所有执行中的Multipart Upload事件
        /// </summary>
        /// <param name="bucketName">目标bucket名称</param>
        public static void ListMultipartUploads(String bucketName)
        {
            var listMultipartUploadsRequest = new ListMultipartUploadsRequest(bucketName);
            var result = client.ListMultipartUploads(listMultipartUploadsRequest);
            Console.WriteLine("Bucket name:" + result.BucketName);
            Console.WriteLine("Key marker:" + result.KeyMarker);
            Console.WriteLine("Delimiter:" + result.Delimiter);
            Console.WriteLine("Prefix:" + result.Prefix);
            Console.WriteLine("UploadIdMarker:" + result.UploadIdMarker);

            foreach (var part in result.MultipartUploads)
            {
                Console.WriteLine(part.ToString());
            }
        }

        private static string InitiateMultipartUpload(String bucketName, String objectName)
        {
            var request = new InitiateMultipartUploadRequest(bucketName, objectName);
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
                    Console.WriteLine("finish {0}/{1}", partETags.Count, partCount);
                }
            }
            return partETags;
        }

        private static void AsyncUploadParts(String bucketName, String objectName, String fileToUpload,
            String uploadId, int partSize)
        {
            var fi = new FileInfo(fileToUpload);
            var fileSize = fi.Length;
            var partCount = fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            var ctx = new UploadPartContext()
            {
                BucketName = bucketName,
                ObjectName = objectName,
                UploadId = uploadId,
                TotalParts = partCount,
                CompletedParts = 0,
                SyncLock = new object(),
                PartETags = new List<PartETag>(),
                WaitEvent = new ManualResetEvent(false)
            };

            for (var i = 0; i < partCount; i++)
            {
                var fs = new FileStream(fileToUpload, FileMode.Open, FileAccess.Read, FileShare.Read);
                var skipBytes = (long)partSize * i;
                fs.Seek(skipBytes, 0);
                var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                var request = new UploadPartRequest(bucketName, objectName, uploadId)
                {
                    InputStream = fs,
                    PartSize = size,
                    PartNumber = i + 1
                };
                client.BeginUploadPart(request, UploadPartCallback, new UploadPartContextWrapper(ctx, fs, i + 1));
            }

            ctx.WaitEvent.WaitOne();
        }

        private static void UploadPartCallback(IAsyncResult ar)
        {
            var result = client.EndUploadPart(ar);
            var wrappedContext = (UploadPartContextWrapper)ar.AsyncState;
            wrappedContext.PartStream.Close();

            var ctx = wrappedContext.Context;
            lock (ctx.SyncLock)
            {
                var partETags = ctx.PartETags;
                partETags.Add(new PartETag(wrappedContext.PartNumber, result.ETag));
                ctx.CompletedParts++;

                Console.WriteLine("finish {0}/{1}", ctx.CompletedParts, ctx.TotalParts);
                if (ctx.CompletedParts == ctx.TotalParts)
                {
                    partETags.Sort((e1, e2) => (e1.PartNumber - e2.PartNumber));
                    var completeMultipartUploadRequest =
                        new CompleteMultipartUploadRequest(ctx.BucketName, ctx.ObjectName, ctx.UploadId);
                    foreach (var partETag in partETags)
                    {
                        completeMultipartUploadRequest.PartETags.Add(partETag);
                    }

                    var completeMultipartUploadResult = client.CompleteMultipartUpload(completeMultipartUploadRequest);
                    Console.WriteLine(@"Async upload multipart result : " + completeMultipartUploadResult.Location);

                    ctx.WaitEvent.Set();
                }
            }
        }

        private static List<PartETag> UploadPartCopys(String targetBucket, String targetObject, String sourceBucket, String sourceObject,
            String uploadId, int partSize)
        {
            var metadata = client.GetObjectMetadata(sourceBucket, sourceObject);
            var fileSize = metadata.ContentLength;

            var partCount = (int)fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            var partETags = new List<PartETag>();
            for (var i = 0; i < partCount; i++)
            {
                var skipBytes = (long)partSize * i;
                var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                var request =
                    new UploadPartCopyRequest(targetBucket, targetObject, sourceBucket, sourceObject, uploadId)
                    {
                        PartSize = size,
                        PartNumber = i + 1,
                        BeginIndex = skipBytes
                    };
                var result = client.UploadPartCopy(request);
                partETags.Add(result.PartETag);
            }

            return partETags;
        }

        private static void AsyncUploadPartCopys(String targetBucket, String targetObject, String sourceBucket, String sourceObject,
            String uploadId, int partSize)
        {
            var metadata = client.GetObjectMetadata(sourceBucket, sourceObject);
            var fileSize = metadata.ContentLength;

            var partCount = (int)fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            var ctx = new UploadPartCopyContext()
            {
                TargetBucket = targetBucket,
                TargetObject = targetObject,
                UploadId = uploadId,
                TotalParts = partCount,
                CompletedParts = 0,
                SyncLock = new object(),
                PartETags = new List<PartETag>(),
                WaitEvent = new ManualResetEvent(false)
            };

            for (var i = 0; i < partCount; i++)
            {
                var skipBytes = (long)partSize * i;
                var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                var request =
                    new UploadPartCopyRequest(targetBucket, targetObject, sourceBucket, sourceObject, uploadId)
                    {
                        PartSize = size,
                        PartNumber = i + 1,
                        BeginIndex = skipBytes
                    };
                client.BeginUploadPartCopy(request, UploadPartCopyCallback, new UploadPartCopyContextWrapper(ctx, i + 1));
            }

            ctx.WaitEvent.WaitOne();
        }

        private static void UploadPartCopyCallback(IAsyncResult ar)
        {
            var result = client.EndUploadPartCopy(ar);
            var wrappedContext = (UploadPartCopyContextWrapper)ar.AsyncState;

            var ctx = wrappedContext.Context;
            lock (ctx.SyncLock)
            {
                var partETags = ctx.PartETags;
                partETags.Add(new PartETag(wrappedContext.PartNumber, result.ETag));
                ctx.CompletedParts++;

                if (ctx.CompletedParts == ctx.TotalParts)
                {
                    partETags.Sort((e1, e2) => (e1.PartNumber - e2.PartNumber));
                    var completeMultipartUploadRequest =
                        new CompleteMultipartUploadRequest(ctx.TargetBucket, ctx.TargetObject, ctx.UploadId);
                    foreach (var partETag in partETags)
                    {
                        completeMultipartUploadRequest.PartETags.Add(partETag);
                    }

                    var completeMultipartUploadResult = client.CompleteMultipartUpload(completeMultipartUploadRequest);
                    Console.WriteLine(@"Async upload multipart copy result : " + completeMultipartUploadResult.Location);

                    ctx.WaitEvent.Set();
                }
            }
        }

        private static CompleteMultipartUploadResult CompleteUploadPart(String bucketName, String objectName,
            String uploadId, List<PartETag> partETags)
        {
            var metadata = BuildCallbackMetadata("","");
            var completeMultipartUploadRequest =
                new CompleteMultipartUploadRequest(bucketName, objectName, uploadId)
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
            string callbackHeaderBuilder = new CallbackHeaderBuilder("","").Build();
            string CallbackVariableHeaderBuilder = new CallbackVariableHeaderBuilder().Build();

            var metadata = new ObjectMetadata();
            metadata.AddHeader(HttpHeaders.Callback, callbackHeaderBuilder);
            metadata.AddHeader(HttpHeaders.CallbackVar, CallbackVariableHeaderBuilder);
            return metadata;
        }

        public static void streamProgressCallback(object sender, StreamTransferProgressArgs args)
        {
            MessageBox.Show("invoke");
            System.Console.WriteLine("ProgressCallback - TotalBytes:{0}, TransferredBytes:{1}, IncrementTransferred:{2}",
                args.TotalBytes, args.TransferredBytes, args.IncrementTransferred);
            double precent = (double)args.TransferredBytes / args.TotalBytes;
            Progress.currentProgress.progress = (int)Math.Floor(precent * 100);
        }



        public static void MultipartUploadProgress(string bucketName)
        {
            const string key = "MultipartUploadProgress";

            try
            {
                // 初始化分片上传任务
                var initRequest = new InitiateMultipartUploadRequest(bucketName, key);
                var initResult = client.InitiateMultipartUpload(initRequest);

                // 设置每块为 1M
                const int partSize = 1024 * 1024 * 1;
                var partFile = new FileInfo("E:\\wsi\\Leica-1.scn");
                // 计算分块数目
                var partCount = CalculatePartCount(partFile.Length, partSize);

                // 新建一个List保存每个分块上传后的ETag和PartNumber
                var partETags = new List<PartETag>();
                //upload the file
                using (var fs = new FileStream(partFile.FullName, FileMode.Open))
                {
                    for (var i = 0; i < partCount; i++)
                    {
                        // 跳到每个分块的开头
                        long skipBytes = partSize * i;
                        fs.Position = skipBytes;

                        // 计算每个分块的大小
                        var size = partSize < partFile.Length - skipBytes ? partSize : partFile.Length - skipBytes;

                        // 创建UploadPartRequest，上传分块
                        var uploadPartRequest = new UploadPartRequest(bucketName, key, initResult.UploadId)
                        {
                            InputStream = fs,
                            PartSize = size,
                            PartNumber = (i + 1)
                        };
                        uploadPartRequest.StreamTransferProgress += streamProgressCallback;
                        var uploadPartResult = client.UploadPart(uploadPartRequest);


                        // 将返回的PartETag保存到List中。
                        partETags.Add(uploadPartResult.PartETag);
                    }
                }

                // 提交上传任务
                var completeRequest = new CompleteMultipartUploadRequest(bucketName, key, initResult.UploadId);
                foreach (var partETag in partETags)
                {
                    completeRequest.PartETags.Add(partETag);
                }
                client.CompleteMultipartUpload(completeRequest);

                Console.WriteLine("Multipart upload object:{0} succeeded", key);
            }
            catch (OssException ex)
            {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
            }
        }


        private static int CalculatePartCount(long totalSize, int partSize)
        {
            var partCount = (int)(totalSize / partSize);
            if (totalSize % partSize != 0)
            {
                partCount++;
            }
            return partCount;
        }


    }


}
