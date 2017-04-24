using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cloudimgWinform.utils.oss;
using System.Threading;
using static cloudimgWinform.SettingDirectory;
using cloudimgWinform.dao;
using System.Windows.Forms;
using tdrConverterLibCppCLI;
using System.IO;
using System.Net;
using cloudimgWinform.utils;
using Newtonsoft.Json.Linq;

namespace cloudimgWinform.bean
{
    class UploadTask
    {
        //上传任务数据
        public static IList<UploadTask> tasks = new BindingList<UploadTask>();
        public int id { get; set; }
        //文件名
        public String name { get; set; }
        //文件路径
        public String path { get; set; }
        public String convertPath { get; set; }
        //上传路径
        public String uploadPath { get; set; }
        public String tdrName
        {
            get
            {
                return name.Substring(0, name.LastIndexOf(".")) + ".tdr";
            }
            set { }
        }
        //tdr路径
        public String tdrPath{
            get {
                return this.convertPath + name.Substring(0, name.LastIndexOf(".")) + ".tdr";
            }
            set { }
        }

        //文件大小
        public long size { get; set; }
        public String associatedName
        {
            get
            {
                return name.Substring(0, name.LastIndexOf(".")) + "_associated.jpg";
            }
            set { }
        }
        //标签图
        public String associatedImgPath
        {
            get
            {
                return this.convertPath + name.Substring(0, name.LastIndexOf(".")) + "_associated.jpg";
            }
            set { }
        }
        //缩略图
        public String previewName
        {
            get
            {
                return name.Substring(0, name.LastIndexOf(".")) + "_thumbnail.jpg";
            }
            set { }
        }
        public String previewPath
        {
            get
            {
                return this.convertPath + name.Substring(0,name.LastIndexOf(".")) + "_thumbnail.jpg";
            }
            set { }
        }
        //扫描倍率
        public int scanRate { get; set; }
        //宽度
        public int width { get; set; }
        //高度
        public int height { get; set; }
        //比例尺
        public double resolution { get; set; }
        //0 待转化  1已转化 2已上传
        public int status { get; set; }

        public UploadTask(String name,String path,int status,long size)
        {
            this.name = name;
            this.path = path;
            this.status = status;
            this.size = size;
        }

        public UploadTask()
        {

        }

        //上传到oss
        public void upload()
        {
            UploadTaskDao.updateStatus(Dictionary.STATUS_UPLOAD, this.id);
            String key = Utils.isNotEmpty(this.uploadPath)? this.uploadPath:getKey();
            Console.WriteLine("upload file to {0}", key);
            try
            {
                if (File.Exists(this.associatedImgPath))
                {
                    OSSUpload.UploadMultipart(OSSConfig.Buket, this.associatedImgPath, key + this.associatedName);
                    File.Delete(this.associatedImgPath);
                }
                if (File.Exists(this.previewPath))
                {
                    OSSUpload.UploadMultipart(OSSConfig.Buket, this.previewPath, key + this.previewName);
                    File.Delete(this.previewPath);
                }
                if (File.Exists(this.tdrPath))
                {
                    OSSUpload.UploadMultipart(OSSConfig.Buket, this.tdrPath, key + this.tdrName);
                    File.Delete(this.tdrPath);
                }
                UploadTaskDao.updateStatus(Dictionary.STATUS_UPLOAD_SUCCESS, this.id);
            }
            catch (Exception e)
            {
                UploadTaskDao.updateStatus(Dictionary.STATUS_UPLOAD_FAIL, this.id);
            }

            DirectoryInfo di = new DirectoryInfo(this.convertPath);
            di.Attributes = di.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
            di.Delete();
        }


        public void transform()
        {
            try
            {
                tdrConverter tdr = new tdrConverter();
                String date = DateTime.Now.ToString("yyyy-MM-dd");
                String uuid = Guid.NewGuid().ToString();
                StringBuilder key = new StringBuilder(Dictionary.UserHome).Append(Dictionary.AppHome)
                    .Append(uuid).Append("_").Append(DateTime.Now.ToString("yyyyMMddfffffff"))
                    .Append("\\");
                if (File.Exists(key.ToString()) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(key.ToString());
                }
                String name = this.name.Substring(0, this.name.LastIndexOf("."));
                this.convertPath = key.ToString();
                UploadTaskDao.updateStatus(Dictionary.STATUS_TRANSFORM, this.id);
                tdr.convertScannerFileToTdr(this.path, this.tdrPath, this.previewPath, this.associatedImgPath);
                this.height = tdr.getMetadataHeight();
                this.width = tdr.getMetadataWidth();
                this.resolution = Math.Round(tdr.getMetadataMicronPerPixel(), 2, MidpointRounding.AwayFromZero) ;
                this.scanRate = tdr.getMetadataMagnification();
                this.status = Dictionary.STATUS_TRANSFORM_SUCCESS;
                this.uploadPath = getKey();
                UploadTaskDao.updateInfo(this);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                UploadTaskDao.updateStatus(Dictionary.STATUS_TRANSFORM_FAIL, this.id);

            }
        }

        //提交图片信息到云图
        private bool submitImage()
        {
            try
            {
                String url = Dictionary.API + "image";
                IDictionary<String, String> imageParams = new Dictionary<String, String>();
                imageParams.Add("associatedImgPath", this.uploadPath + this.associatedName);
                imageParams.Add("previewPath", this.uploadPath + this.previewName);
                imageParams.Add("filePath", this.uploadPath + this.name);
                imageParams.Add("associatedImgPath", this.associatedImgPath);
                imageParams.Add("associatedImgPath", this.associatedImgPath);
                imageParams.Add("scanRate", this.scanRate+"");
                imageParams.Add("width", this.width + "");
                imageParams.Add("height", this.height + "");
                imageParams.Add("resolution", this.resolution + "");
                imageParams.Add("createUserId", User.loginUser.userId + "");
                HttpWebResponse response = HttpHelper.CreatePostHttpResponse(url, imageParams, 5000, "", new UTF8Encoding(), null);
                StreamReader responseReader = new StreamReader(response.GetResponseStream());
                String responseData = responseReader.ReadToEnd();
                JObject jsonObj = JObject.Parse(responseData);
                if (jsonObj["responseCode"].Equals("0"))
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


        //获取文件key（路径：cloud/{date}/{uuid}_{yyyyMMddffffff}/{name}）
        public String getKey()
        {
            String date = DateTime.Now.ToString("yyyy-MM-dd");
            String uuid=Guid.NewGuid().ToString();
            StringBuilder key = new StringBuilder(OSSConfig.UploadPath).Append(date).Append("/")
                .Append(uuid).Append("_").Append(DateTime.Now.ToString("yyyyMMddfffffff"))
                .Append("/");
            return key.ToString();
        }

        //获取可上传的文件
        public static UploadTask getNewToUpload()
        {
            UploadTask ut= UploadTaskDao.getByStatus(Dictionary.STATUS_TRANSFORM_SUCCESS);
            return ut;
        }


        /**
         * 监控任务数据，并执行分片上传
         **/
        public static void MonitorUpload()
        {
            while (true)
            {
                Console.WriteLine("scan file to upload");
                try
                {
                    Thread.Sleep(3000);
                    UploadTask ut = getNewToUpload();
                    if (ut == null)
                    {
                        continue;
                    }
                    ut.upload();
                    Console.WriteLine("{0} uploaded ",ut.name);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }

        }

        public static void MonitorTransform()
        {
            while (true)
            {
                Console.WriteLine("scan file to transform");
                try
                {
                    Thread.Sleep(3000);
                    UploadTask ut = UploadTaskDao.getByStatus(Dictionary.STATUS_WAIT);
                    if (ut == null)
                    {
                        continue;
                    }
                    ut.transform();
                    Console.WriteLine("{0} transformed ", ut.name);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }





    }
}
