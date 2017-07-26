using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using cloudimgWinform.utils.oss;
using System.Threading;
using cloudimgWinform.dao;
using System.Windows.Forms;
using cloudimgWinform.bean;
using System.IO;
using System.Net;
using cloudimgWinform.utils;
using Newtonsoft.Json.Linq;
using Aliyun.OSS;
using Aliyun.OSS.Util;
using System.Diagnostics;
using System.Drawing;

namespace cloudimgWinform.bean
{
    public class UploadTask
    {
       
        //上传任务数据
        public static IList<UploadTask> tasks = new BindingList<UploadTask>();

        public int id { get; set; }
        //文件名
        public String name { get; set; }
        //文件路径
        public String path { get; set; }
        //上传路径
        public String uploadPath { get; set; }
        
        //文件大小
        public long size { get; set; }
        public String md5 { get; set; }
        //宽度
        public int width { get; set; }
        //高度
        public int height { get; set; }

        public int status { get; set; }


        public UploadTask()
        {

        }

        public UploadTask(String name,String path, long size)
        {
            this.name = name;
            this.path = path;
            this.size = size;
            this.status = Dictionary.STATUS_WAIT;
        }

        //上传到oss
        public void upload()
        {
            if (this.md5 != null)
            {
                this.md5 = FileUtils.GetMD5HashFromFile(this.path);
            }

            if (this.width==0 && height==0 && FileUtils.isCommonImage(this.path))
            {
                Image originalImage = Image.FromFile(this.path);
                this.width = originalImage.Width;
                this.height = originalImage.Height;
                originalImage.Dispose();
            }
            this.status = Dictionary.STATUS_UPLOAD;
            this.uploadPath = getKey() + (this.path.Substring(this.path.LastIndexOf(Path.DirectorySeparatorChar) + 1));
            UploadTaskDao.updateInfo(this);
            Debug.WriteLine(String.Format("upload file to {0}", this.uploadPath));
            try
            {
                //图片上传
                if (File.Exists(this.path))
                {
                    CompleteMultipartUploadResult result= OSSUpload.UploadMultipart(Dictionary.OSSConfig.Buket, this);
                    if (result == null || result.ResponseMetadata == null)
                    {
                        UploadTaskDao.updateStatus(Dictionary.STATUS_UPLOAD_FAIL, this.id);
                    }
                    else
                    {
                        Debug.WriteLine(result.ResponseMetadata);
                        UploadTaskDao.updateStatus(Dictionary.STATUS_UPLOAD_SUCCESS, this.id);
                    }
                }
                else
                {
                    UploadTaskDao.updateStatus(Dictionary.STATUS_UPLOAD_FAIL, this.id);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                UploadTaskDao.updateStatus(Dictionary.STATUS_UPLOAD_FAIL, this.id);
            }
        }


       
        
        //获取文件key（路径：cloud/{date}/{uuid}_{yyyyMMddffffff}/{name}）
        public String getKey()
        {
            String date = DateTime.Now.ToString("yyyy-MM-dd");
            String uuid=Guid.NewGuid().ToString();
            StringBuilder key = new StringBuilder(Dictionary.OSSConfig.UploadPath).Append("/").Append(date).Append("/")
                .Append(uuid).Append("_").Append(DateTime.Now.ToString("yyyyMMddfffffff"))
                .Append("/");
            return key.ToString();
        }

        /**
         * 监控任务数据，并执行分片上传
         **/
        public static void MonitorUpload()
        {
            while (true)
            {
                Debug.WriteLine("scan file to upload");
                try
                {
                    Thread.Sleep(3000);
                    List<UploadTask> ut = UploadTaskDao.getByStatus(Dictionary.STATUS_WAIT);
                    if (ut.Count==0)
                    {
                        continue;
                    }
                    foreach (UploadTask task in ut)
                    {
                        task.upload();
                        Debug.WriteLine("{0} uploaded ", task.name);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                
            }

        }

        public String toCallBackString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ogFilePath=");
            sb.Append(this.uploadPath);
            sb.Append("&md5=");
            sb.Append(this.md5);
            sb.Append("&width=");
            sb.Append(this.width);
            sb.Append("&height=");
            sb.Append(this.height);
            sb.Append("&imageSize=");
            sb.Append(this.size);
            sb.Append("&userId=");
            sb.Append(User.loginUser.userId);
            sb.Append("&access_token=");
            sb.Append(User.loginUser.accessToken);
            return sb.ToString();
        }
    }
}

