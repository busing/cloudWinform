using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using cloudimgWinform.utils;

namespace cloudimgWinform.bean
{
    class Dictionary
    {
        public static String CurrentPath = Directory.GetCurrentDirectory();
        public static ConfigLoad configLoad = new ConfigLoad(CurrentPath+"\\conf.ini");
        public static String APP_VERSION = null;
        public static String DATA_VERSION = null;
        public static String ENV = null;
        public static bool AUTO_DEL_CONVERTFILE = false;
        public static int CLIENT_TYPE = 2;

        public static String API = null;

        public static String[] SLIDE_FILE_SUFFIX = new String[] { "svs", "tif", "vms", "vmu", "ndpi", "scn", "mrxs", "tiff", "svslide", "bif", "jpg", "jpeg", "png", "svg" };
        public static String USER_HOME = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public static String APP_HOME = "\\terrydr\\cloudimg\\";
        public static String VERSION_FILE = USER_HOME + APP_HOME + "version";

        internal class OSSConfig
        {
            public static string Endpoint = null;

            public static string UploadPath = null;

            public static string Buket = null;
        }

        public static void init()
        {
            APP_VERSION = configLoad.GetStringValue("app.version");
            DATA_VERSION = configLoad.GetStringValue("data.version");
            API = configLoad.GetStringValue("api");
            ENV = configLoad.GetStringValue("env");
            AUTO_DEL_CONVERTFILE = configLoad.GetStringValue("autoDelConvertFile").Equals("1") ? true : false;
            OSSConfig.Endpoint = configLoad.GetStringValue("oss.endpoint");
            OSSConfig.UploadPath = configLoad.GetStringValue("oss.uploadPath");
            OSSConfig.Buket= configLoad.GetStringValue("oss.bucket");
        }


        //等待中
        public const int STATUS_WAIT = 0;
        //上传中
        public const int STATUS_UPLOAD = 1;
        //上传成功
        public const int STATUS_UPLOAD_SUCCESS = 2;
        //上传失败
        public const int STATUS_UPLOAD_FAIL = 3;

       

    }
}
