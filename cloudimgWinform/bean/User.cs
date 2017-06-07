using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using System.Xml.Linq;
using System.Runtime.Remoting.Contexts;
using Newtonsoft.Json.Linq;
using System.IO;
using cloudimgWinform.utils;

namespace cloudimgWinform.bean
{
    class User
    {

        public static User loginUser = null;

        public int userId { get; set; }
        public String userName { get; set; }
        public String email { get; set; }
        public String password { get; set; }
        public String passwordMD5 { get; set; }
        public String accessToken { get; set; }


        public static User httpLogin(String userName, String password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = Encoding.Default.GetBytes(password.Trim());
            byte[] output = md5.ComputeHash(result);
            String passwordMd5 = BitConverter.ToString(output).Replace("-", "").ToLower();
            String loginUrl = Dictionary.API + "users/sessions";
            IDictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("userName", userName);
            postParams.Add("password", passwordMd5);
            JObject jsonObj = HttpHelper.CreatePostHttpResponse(loginUrl, postParams, 5000, "", new UTF8Encoding(), null);
            Object userInfo = jsonObj["returnObject"];
            if (int.Parse(jsonObj["responseCode"].ToString()) == 0 && userInfo != null)
            {
                User user = new User();
                user.userId = int.Parse((string)jsonObj["returnObject"]["id"]);
                user.password = password;
                user.passwordMD5 = passwordMd5;
                user.userName = (string)jsonObj["returnObject"]["userName"];
                user.accessToken = (string)jsonObj["returnObject"]["accessToken"];
                return user;
            }
            return null;
        }



    }
}
