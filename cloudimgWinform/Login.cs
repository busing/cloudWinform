using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cloudimgWinform.bean;
using cloudimgWinform.utils;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Runtime.Remoting.Contexts;
using Newtonsoft.Json.Linq;
using cloudimgWinform.dao;

namespace cloudimgWinform
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void loginbtn_Click(object sender, EventArgs e)
        {
            //openSettingDirectory();
            try
            {
                String userName = this.userName.Text;
                String password = this.password.Text;
                if (Utils.isNotEmpty(userName) && Utils.isNotEmpty(password))
                {
                    User user = httpLogin(userName, password);
                    if (user == null)
                    {
                        MessageBox.Show("登录失败");
                        return;
                    }
                    User.loginUser = user;
                    openSettingDirectory();
                }
                else
                {
                    MessageBox.Show("请输入账号和密码");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("登录失败");
                Console.WriteLine(ex.Message);
            }
        }

        private User httpLogin(String userName, String password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = Encoding.Default.GetBytes(password.Trim());
            byte[] output = md5.ComputeHash(result);
            String passwordMd5 = BitConverter.ToString(output).Replace("-", "").ToLower();
            String loginUrl = Dictionary.API + "account/login?userName=" + userName + "&password=" + passwordMd5;
            HttpWebResponse response=HttpHelper.CreateGetHttpResponse(loginUrl, 5000, "", null);
            StreamReader  responseReader = new StreamReader(response.GetResponseStream());
            String responseData = responseReader.ReadToEnd();
           
            Console.WriteLine(responseData);
            //JObject jo = XObject.Parse(Context);
            JObject jsonObj = JObject.Parse(responseData);
            Object userInfo = jsonObj["returnObject"];
            if (userInfo!=null)
            {
                User user = new User();
                user.userId = int.Parse((string)jsonObj["returnObject"]["id"]);
                user.userName = (string)jsonObj["returnObject"]["userName"];
                return user;
            }
            return null;
        }

        private void openSettingDirectory()
        {
            SettingDirectory sd = new SettingDirectory();
            this.Hide();
            sd.Show();
        }

        private void login_Load(object sender, EventArgs e)
        {
            UploadTaskDao.initDataBase();
            //init version file
            File.Delete(Dictionary.VERSION_FILE);
            FileUtils.AppendFile(Dictionary.VERSION_FILE, "app:" + Dictionary.APP_VERSION+"\n");
            FileUtils.AppendFile(Dictionary.VERSION_FILE, "data:" + Dictionary.DATA_VERSION);
        }
    }
}
