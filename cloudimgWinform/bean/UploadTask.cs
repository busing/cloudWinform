using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cloudimgWinform.utils.oss;
using System.Threading;
using static cloudimgWinform.SettingDirectory;

namespace cloudimgWinform.bean
{
    class UploadTask 
    {
        //上传任务数据
        public static IList<UploadTask> tasks = new BindingList<UploadTask>();
        //文件名
        public String name { get; set; }
        //文件路径
        public String path { get; set; }
        //文件大小
        public long size { get; set; }
        //标签图
        public String associatedImgPath { get; set; }
        //缩略图
        public String previewPath { get; set; }
        //扫描倍率
        public int scanRate { get; set; }
        //宽度
        public int width { get; set; }
        //高度
        public int height { get; set; }
        //比例尺
        public float resolution { get; set; }
        //0 待转化  1已转化 2已上传
        public int status { get; set; }

        public UploadTask(String name,String path,int status,long size)
        {
            this.name = name;
            this.path = path;
            this.status = status;
            this.size = size;
        }

        //上传到oss
        public void upload()
        {
            this.status = 4;
            SettingDirectory.DataView.Refresh();
            String key = getKey();
            Console.WriteLine("upload file to {0}", key);
            OSSUpload.UploadMultipart(OSSConfig.Buket, this.path, key);
            this.status = 5;
            SettingDirectory.DataView.Refresh();
        }

        //获取文件key（路径：cloud/{date}/{uuid}_{yyyyMMddffffff}/{name}）
        public String getKey()
        {
            String date = DateTime.Now.ToString("yyyy-MM-dd");
            String uuid=Guid.NewGuid().ToString();
            StringBuilder key = new StringBuilder(OSSConfig.UploadPath).Append(date).Append("/")
                .Append(uuid).Append("_").Append(DateTime.Now.ToString("yyyyMMddfffffff"))
                .Append("/").Append(this.name);
            return key.ToString();
        }

        //获取可上传的文件
        public static UploadTask getNewToUpload()
        {
            foreach (UploadTask ut in tasks)
            {
                if (ut.status == 2)
                {
                    return ut;
                }
            }
            return null;
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
                    if (tasks.Count == 0)
                    {
                        continue;
                    }
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
                    throw;
                }
                
            }
        }

        
    }
}
