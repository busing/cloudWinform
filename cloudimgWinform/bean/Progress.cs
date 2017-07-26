using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cloudimgWinform.bean
{
    class Progress
    {
        public int progress { get; set; }

        public static Progress currentProgress = new Progress();
        public static Progress getProgress()
        {
            return currentProgress;
        }
    }
}
