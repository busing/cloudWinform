using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudimgWinform.bean
{
    class User
    {

        public static User loginUser = null;

        public int userId { get; set; }
        public String userName { get; set; }
        public String email { get; set; }

      

    }
}
