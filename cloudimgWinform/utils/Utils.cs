using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudimgWinform
{
    class Utils
    {
        public static bool isNotEmpty(String s)
        {
            if (s == null)
            {
                return false;
            }
            if (s.Trim().Equals(""))
            {
                return false;

            }
            return true;
        }

        public static bool isEmpty(String s)
        {
            return !isNotEmpty(s);
        }

       
    }
}
