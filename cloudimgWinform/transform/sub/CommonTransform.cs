using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cloudimgWinform.utils;
using System.Drawing;
using System.IO;

namespace cloudimgWinform.transform.sub
{
    class CommonTransform:ImageTransform
    {
        protected override void transform()
        {
            File.Copy(this.task.path, this.task.previewPath);
            Image originalImage = Image.FromFile(this.task.path);
            this.task.width = originalImage.Width;
            this.task.height = originalImage.Height;
        }
    }
}
