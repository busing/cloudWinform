using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudimgWinform.bean
{
    class Dictionary
    {
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
    }
}
