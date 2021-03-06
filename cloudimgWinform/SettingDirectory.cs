﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using cloudimgWinform.bean;
using System.Threading;
using System.IO;
using cloudimgWinform.dao;
using cloudimgWinform.utils;
using System.Data.SQLite;
using cloudimgWinform.form;

namespace cloudimgWinform
{
    public partial class SettingDirectory : Form
    {
        public static DataGridView DataView;
        private static Thread uploadThread;
        public SettingDirectory()
        {
            InitializeComponent();
            DataView = this.tasksDataView;
            DataView.AutoGenerateColumns = false;
            Control.CheckForIllegalCrossThreadCalls = false;

            bindDataSource();

            //扫描上传
            uploadThread = new Thread(UploadTask.MonitorUpload);
            uploadThread.IsBackground = true;
            uploadThread.Start();

            //刷新界面gridview数据
            Thread refreshThread = new Thread(refresh);
            refreshThread.IsBackground = true;
            refreshThread.Start();



        }

        public void bindDataSource()
        {
            try
            {
                int row = 1;
                int col = 1;
                if (tasksDataView.CurrentCell != null)
                {
                    row = tasksDataView.CurrentCell.RowIndex;
                    col = tasksDataView.CurrentCell.ColumnIndex;
                }

                int rowindex = tasksDataView.FirstDisplayedScrollingRowIndex;
                SQLiteParameter p1 = SQLiteHelper.CreateParameter("user_id", DbType.Int32, User.loginUser.userId);
                SQLiteDataAdapter da = new SQLiteDataAdapter("select * from t_uploadtask where user_id="+User.loginUser.userId+" order by id", UploadTaskDao.connection);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                tasksDataView.DataSource = dt;
               
                if (row < tasksDataView.RowCount)
                {
                    DataGridViewCell cell = tasksDataView[col, row];
                    tasksDataView.CurrentCell = cell;
                }
                if (rowindex > 0)
                {
                    tasksDataView.FirstDisplayedScrollingRowIndex = rowindex;
                }
                tasksDataView.Focus();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        public void refresh()
        {
            while (true)
            {
                Thread.Sleep(2000);
                refreshOnce();
            }

        }

        private void refreshOnce()
        {
          
            UploadTask.tasks = UploadTaskDao.query();
            if (tasksDataView != null)
            {
                tasksDataView.BeginInvoke(new MethodInvoker(() =>
                    bindDataSource()
                 ));
               
            }
        }


        //选择上传文件
        private void choose_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            StringBuilder filter = new StringBuilder();
            foreach (String s in Dictionary.SLIDE_FILE_SUFFIX)
            {
                filter.Append("*.").Append(s).Append(";");
            }
            filter.Remove(filter.Length-1,1);
            fileDialog.Filter = "虚拟切片|"+ filter.ToString();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] files = fileDialog.FileNames;
                pushUpload(files);
            }
        }

        private void pushUpload(string[] files)
        {
            List<String> spaceFile = new List<string>();
            foreach (String file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                String fileName = file.Substring(file.LastIndexOf("\\") + 1);
                if (fileName.Contains(" "))
                {
                    spaceFile.Add(fileName);
                }
                else
                {
                    long fileSize = fileInfo.Length / 1024;
                    fileSize = fileSize > 0 ? fileSize : 1;
                    UploadTask t = new UploadTask(fileName, file, fileSize);
                    UploadTaskDao.addTask(t);
                }
                
            }
            if (spaceFile.Count > 0)
            {
                MessageBox.Show(spaceFile.Count+"个文件被忽略,因为文件名包含空格");
            }
            refreshOnce();
        }



        //数据字段格式化
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //状态
            if (this.tasksDataView.Columns["status"].Index == e.ColumnIndex && e.RowIndex >= 0)
            {
                switch (int.Parse(e.Value.ToString())){
                    case Dictionary.STATUS_WAIT:
                       e.Value = "等待中";
                       break;
                    case Dictionary.STATUS_UPLOAD:
                        String progress = Progress.currentProgress.progress+"%";
                        e.Value = "上传"+ progress;
                        break;
                    case Dictionary.STATUS_UPLOAD_SUCCESS:
                        e.CellStyle.ForeColor = Color.Green;
                        e.Value = "上传成功";
                        break;
                    case Dictionary.STATUS_UPLOAD_FAIL:
                        e.CellStyle.ForeColor = Color.Red;
                        e.Value = "上传失败";
                        break;
                }

            }
            //文件大小
            else if (this.tasksDataView.Columns["fileSize"].Index == e.ColumnIndex && e.RowIndex >= 0)
            {
                long fileSize = long.Parse(e.Value.ToString());
                //b
                if (fileSize / 1024==0)
                {
                    e.Value = e.Value+ "kb";
                }
                //kb
                else if (fileSize / 1024>0 && fileSize /(1024*1024)==0)
                {
                    e.Value = ((float)fileSize / 1024).ToString("F2") + "M";
                }
                //M
                else if (fileSize / (1024*1024) > 0 && fileSize / (1024 * 1024* 1024) == 0)
                {
                    e.Value = ((float)fileSize / 1024/1024).ToString("F2") + "G";
                }
                //G
                else if (fileSize / (1024*1024*1024) > 0)
                {
                    e.Value = ((float)fileSize / 1024 / 1024/1024).ToString("F2") + "T";
                }
                
            }

        }

        //删除任务
        private void delTasks_Click(object sender, EventArgs e)
        {
            int row = tasksDataView.CurrentCell.RowIndex;
            int id = int.Parse(tasksDataView.CurrentRow.Cells[0].Value.ToString());
            int status = int.Parse(tasksDataView.CurrentRow.Cells[7].Value.ToString());
            String name = tasksDataView.CurrentRow.Cells[1].Value.ToString();
            DialogResult dr = MessageBox.Show("确定删除"+ name+"吗？", "删除任务", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
               
                //当前数据正在上传，停止上传
                if (status == Dictionary.STATUS_UPLOAD)
                {
                    uploadThread.Abort();
                    uploadThread = new Thread(UploadTask.MonitorUpload);
                    uploadThread.IsBackground = true;
                    uploadThread.Start();
                }
                UploadTaskDao.delTask(id);
            }
            refreshOnce();

        }

        //单元格右键  选中
        private void tasksDataView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex>0)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    tasksDataView.ClearSelection();
                    DataGridViewCell cell = tasksDataView[e.ColumnIndex, e.RowIndex];
                    tasksDataView.CurrentCell = cell;
                }
            }
        }


        //文件拖入
        private void SettingDirectory_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        //文件拖入
        private void SettingDirectory_DragDrop(object sender, DragEventArgs e)
        {
            e.Data.GetData(DataFormats.FileDrop).ToString();
            List<String> files = new List<String>();
            foreach (String file in (String[])e.Data.GetData(DataFormats.FileDrop))
            {
                foreach (String stuff in Dictionary.SLIDE_FILE_SUFFIX)
                {
                    if (file.ToLower().EndsWith("."+stuff))
                    {
                        files.Add(file);
                        break;
                    }
                }
            }
            String[] fileArr = files.ToArray();
            pushUpload(fileArr);

        }

        private void SettingDirectory_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.ShowInTaskbar = false;
                this.notifyIcon.Visible = true;
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                notifyIcon.Visible = false;
                this.ShowInTaskbar = true;
            }
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.notifyIcon_DoubleClick(sender,e);
        }

        private void tasksDataView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SettingDirectory_Load(object sender, EventArgs e)
        {

        }

        private void delTask_Opening(object sender, CancelEventArgs e)
        {
            int row = tasksDataView.CurrentCell.RowIndex;
            int id = int.Parse(tasksDataView.CurrentRow.Cells[0].Value.ToString());
            int status=int.Parse(tasksDataView.CurrentRow.Cells[7].Value.ToString());
            if (status != Dictionary.STATUS_UPLOAD_FAIL)
            {
                retry.Enabled = false;
            }
            else
            {
                retry.Enabled = true;
            }
        }

        private void SettingDirectory_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定退出程序吗？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void SettingDirectory_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void cleanTask_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UploadTaskDao.delTaskByStatus(Dictionary.STATUS_UPLOAD_SUCCESS);
            refreshOnce();
        }

        private void cleanAllTask_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定清除所有任务吗？", "清除任务", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                UploadTaskDao.delAllTask();
                uploadThread.Abort();
                uploadThread = new Thread(UploadTask.MonitorUpload);
                uploadThread.IsBackground = true;
                uploadThread.Start();
            }
            refreshOnce();
        }

        private void retry_Click(object sender, EventArgs e)
        {
            int row = tasksDataView.CurrentCell.RowIndex;
            int id = int.Parse(tasksDataView.CurrentRow.Cells[0].Value.ToString());
            UploadTaskDao.updateStatus(Dictionary.STATUS_WAIT, id);
            refreshOnce();
        }

        private void tasksDataView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            foreach (DataGridViewRow row in tasksDataView.Rows)
            {
                row.Cells[1].Value = row.Index + 1;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://cloud.terrydr.com");
        }
    }
}
