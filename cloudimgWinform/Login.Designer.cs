namespace cloudimgWinform
{
    partial class login
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(login));
            this.userName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.TextBox();
            this.loginbtn = new System.Windows.Forms.Button();
            this.linkcloud = new System.Windows.Forms.LinkLabel();
            this.logintip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // userName
            // 
            this.userName.Location = new System.Drawing.Point(85, 42);
            this.userName.Name = "userName";
            this.userName.Size = new System.Drawing.Size(200, 21);
            this.userName.TabIndex = 0;
            this.userName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.login_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "账号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "密码：";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(85, 87);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(200, 21);
            this.password.TabIndex = 2;
            this.password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.login_KeyUp);
            // 
            // loginbtn
            // 
            this.loginbtn.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.loginbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginbtn.ForeColor = System.Drawing.Color.White;
            this.loginbtn.Location = new System.Drawing.Point(133, 139);
            this.loginbtn.Name = "loginbtn";
            this.loginbtn.Size = new System.Drawing.Size(85, 30);
            this.loginbtn.TabIndex = 4;
            this.loginbtn.Text = "登录";
            this.logintip.SetToolTip(this.loginbtn, "使用泰瑞云图账号登录");
            this.loginbtn.UseVisualStyleBackColor = false;
            this.loginbtn.Click += new System.EventHandler(this.loginbtn_Click);
            // 
            // linkcloud
            // 
            this.linkcloud.AutoSize = true;
            this.linkcloud.Location = new System.Drawing.Point(97, 181);
            this.linkcloud.Name = "linkcloud";
            this.linkcloud.Size = new System.Drawing.Size(149, 12);
            this.linkcloud.TabIndex = 9;
            this.linkcloud.TabStop = true;
            this.linkcloud.Text = "http://cloud.terrydr.com";
            this.linkcloud.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkcloud_LinkClicked);
            // 
            // login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(345, 202);
            this.Controls.Add(this.linkcloud);
            this.Controls.Add(this.loginbtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.password);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.userName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "login";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "泰瑞云图登录";
            this.Load += new System.EventHandler(this.login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox userName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Button loginbtn;
        private System.Windows.Forms.LinkLabel linkcloud;
        private System.Windows.Forms.ToolTip logintip;
    }
}

