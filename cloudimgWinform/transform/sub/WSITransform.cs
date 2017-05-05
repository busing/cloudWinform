using cloudimgWinform.bean;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tdrConverterLibCppCLI;

namespace cloudimgWinform.transform.sub
{
    class WSITransform : ImageTransform
    {

        protected override void transform()
        {
            this.task.associatedImgPath = this.task.convertPath + this.task.name.Substring(0, this.task.name.LastIndexOf(".")) + "_associated.jpg";
            //转化文件
            UploadTask.TDR = new tdrConverter();
            UploadTask.TDR.setThreadNum(4);
            UploadTask.TDR.convertScannerFileToTdr(this.task.path, this.task.tdrPath, this.task.previewPath, this.task.associatedImgPath);
            this.task.associatedImgPath = File.Exists(this.task.associatedImgPath) ? this.task.associatedImgPath : "";
            this.task.height = UploadTask.TDR.getMetadataHeight();
            this.task.width = UploadTask.TDR.getMetadataWidth();
            this.task.resolution = Math.Round(UploadTask.TDR.getMetadataMicronPerPixel(), 4, MidpointRounding.AwayFromZero);
            this.task.scanRate = UploadTask.TDR.getMetadataMagnification();
            UploadTask.TDR = null;
        }
    }
}
