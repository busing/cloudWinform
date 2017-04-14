using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cloudimgWinform.utils.oss;
using System.Threading;

namespace cloudimgWinform.bean
{
    class UploadTask 
    {
        public static IList<UploadTask> tasks = new BindingList<UploadTask>();
        public String name { get; set; }
        public String path { get; set; }
        public long size { get; set; }
        
        //0 待转化  1已转化 2已上传
        public int status { get; set; }



        public UploadTask(String name,String path,int status,long size)
        {
            this.name = name;
            this.path = path;
            this.status = status;
            this.size = size;
        }

        public void upload()
        {
            this.status = 4;
            String key = getKey();
            Console.WriteLine("upload file to {0}", key);
            OSSUpload.UploadMultipart(OSSConfig.Buket, this.path, key);
            this.status = 5;
        }

        public String getKey()
        {
            String date = DateTime.Now.ToString("yyyy-MM-dd");
            String uuid=Guid.NewGuid().ToString();
            StringBuilder key = new StringBuilder(OSSConfig.UploadPath).Append(date).Append("/")
                .Append(uuid).Append("_").Append(DateTime.Now.ToString("yyyyMMddfffffff"))
                .Append("/").Append(this.name);
            return key.ToString();
        }

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
