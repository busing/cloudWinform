using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudimgWinform.bean
{
    class Progress
    {
        public String taskName { get; set; }
        public int progress { get; set; }

        public static Progress currentProgress = new Progress();
        public static Progress getProgress()
        {
            return currentProgress;
        }
    }
}
