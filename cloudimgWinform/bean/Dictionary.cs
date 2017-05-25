using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudimgWinform.bean
{
    class Dictionary
    {
        public static String APP_VERSION = "1.0.0";
        public static String DATA_VERSION = "1.0.0";
        public static bool AUTO_DEL_CONVERTFILE = false;

        public static String API = "http://betacloudapi.terrydr.com/";

        public static String[] SLIDE_FILE_SUFFIX = new String[]{"svs", "tif" , "vms" , "vmu" , "ndpi" , "scn" , "mrxs" , "tiff" , "svslide" , "bif","jpg","jpeg","png","svg"};
        //public static String USER_HOME = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static String USER_HOME = "g:\\";
       
        public static String APP_HOME = "\\terrydr\\cloudimg\\";
        public static String VERSION_FILE = USER_HOME + APP_HOME + "version";



        //等待中
        public const int STATUS_WAIT = 0;
        //转化中
        public const int STATUS_TRANSFORM = 1;
        //转化成功
        public const int STATUS_TRANSFORM_SUCCESS = 2;
        //转换失败
        public const int STATUS_TRANSFORM_FAIL = 3;
        //上传中
        public const int STATUS_UPLOAD = 4;
        //上传成功
        public const int STATUS_UPLOAD_SUCCESS = 5;
        //上传失败
        public const int STATUS_UPLOAD_FAIL = 6;
        //提交成功
        public const int STATUS_SUBMIT_SUCCESS = 7;
        //提交失败
        public const int STATUS_SUBMIT_FAIL = 8;
    }
}
