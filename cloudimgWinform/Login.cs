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

using System.IO;

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
            try
            {
                String userName = this.userName.Text;
                String password = this.password.Text;
                if (Utils.isNotEmpty(userName) && Utils.isNotEmpty(password))
                {
                    User user = User.httpLogin(userName, password);
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

        private void linkcloud_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://cloud.terrydr.com");
        }
    }
}
