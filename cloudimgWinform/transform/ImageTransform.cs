using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cloudimgWinform.bean;
using cloudimgWinform.transform.sub;
using System.IO;
using cloudimgWinform.utils;

namespace cloudimgWinform.transform
{
    abstract class ImageTransform
    {

        public UploadTask task { get; set; }


        public static ImageTransform createTransform(UploadTask task)
        {
            ImageTransform it;
            if (FileUtils.isCommonImage(task.path))
            {
                it = new CommonTransform();
            }
            else
            {
                it = new WSITransform();
            }
            it.task = task;
            return it;
        }

        public void startTransform()
        {
            initDirectory();
            transform();
        }
        protected abstract void transform();

        public void initDirectory()
        {
            String date = DateTime.Now.ToString("yyyy-MM-dd");
            String uuid = Guid.NewGuid().ToString();
            StringBuilder key = new StringBuilder(Dictionary.USER_HOME).Append(Dictionary.APP_HOME).Append("convert\\")
                .Append(uuid).Append("_").Append(DateTime.Now.ToString("yyyyMMddfffffff"))
                .Append("\\");
            if (File.Exists(key.ToString()) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(key.ToString());
            }
            this.task.convertPath = key.ToString();
        }


    }
}
