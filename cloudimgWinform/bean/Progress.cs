using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace cloudimgWinform.bean
{
    class Progress
    {
        public int progress { get; set; }

        public long totalSize { get; set; }

        public long transferredSize { get; set; }

        public static Progress currentProgress = new Progress();
        public static Progress getProgress()
        {
            return currentProgress;
        }
    }
}
