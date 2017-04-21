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
using System.Threading;
using System.IO;
using cloudimgWinform.dao;
using cloudimgWinform.utils;
using System.Data.SQLite;

namespace cloudimgWinform
{
    public partial class SettingDirectory : Form
    {
        public static DataGridView DataView;
        public SettingDirectory()
        {
            InitializeComponent();
            DataView = this.tasksDataView;
            DataView.AutoGenerateColumns = false;
            Control.CheckForIllegalCrossThreadCalls = false;

            //UploadTask.tasks= UploadTaskDao.query();
            //tasksDataView.DataSource = UploadTask.tasks;
            bindDataSource();

            Thread uploadThread =new Thread (UploadTask.MonitorUpload);
            uploadThread.IsBackground = true;
            uploadThread.Start();

            Thread refreshThread = new Thread(refresh);
            refreshThread.IsBackground = true;
            refreshThread.Start();



        }

        public void bindDataSource()
        {
            int rowindex = tasksDataView.FirstDisplayedScrollingRowIndex;
            SQLiteDataAdapter da = new SQLiteDataAdapter("select * from t_uploadtask order by id", UploadTaskDao.connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            tasksDataView.DataSource = dt;
            if (rowindex > 0)
            {
                tasksDataView.FirstDisplayedScrollingRowIndex = rowindex;
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
            fileDialog.Filter = "所有文件(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] files = fileDialog.FileNames;
                pushUpload(files);
            }
        }

        private void pushUpload(string[] files)
        {
            foreach (String file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                String fileName = file.Substring(file.LastIndexOf("\\") + 1);
                UploadTask t = new UploadTask(fileName, file, 2, fileInfo.Length);
                UploadTaskDao.addTask(t);
            }
            refreshOnce();
        }


        //数据字段格式化
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //状态
            if (this.tasksDataView.Columns["status"].Index == e.ColumnIndex && e.RowIndex >= 0)
            {
                switch (e.Value){
                    case Dictionary.STATUS_WAIT:
                       e.Value = "等待中";
                       break;
                    case Dictionary.STATUS_TRANSFORM:
                        e.Value = "转化中";
                        break;
                    case Dictionary.STATUS_TRANSFORM_SUCCESS:
                        e.Value = "转化成功";
                        break;
                    case Dictionary.STATUS_TRANSFORM_FAIL:
                        e.Value = "转化失败";
                        break;
                    case Dictionary.STATUS_UPLOAD:
                        e.Value = "上传中";
                        break;
                    case Dictionary.STATUS_UPLOAD_SUCCESS:
                        e.Value = "上传成功";
                        break;
                    case Dictionary.STATUS_UPLOAD_FAIL:
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
                    e.Value = e.Value+ "b";
                }
                //kb
                else if (fileSize / 1024>0 && fileSize /(1024*1024)==0)
                {
                    e.Value = ((float)fileSize / 1024).ToString("F2") + "kb";
                }
                //M
                else if (fileSize / (1024*1024) > 0 && fileSize / (1024 * 1024* 1024) == 0)
                {
                    e.Value = ((float)fileSize / 1024/1024).ToString("F2") + "M";
                }
                //G
                else if (fileSize / (1024*1024*1024) > 0)
                {
                    e.Value = ((float)fileSize / 1024 / 1024/1024).ToString("F2") + "G";
                }
                
            }
        }

        //删除任务
        private void delTasks_Click(object sender, EventArgs e)
        {
            int row = tasksDataView.CurrentCell.RowIndex;
            int id = int.Parse(tasksDataView.CurrentRow.Cells[0].Value.ToString());
            String name = tasksDataView.CurrentRow.Cells[0].Value.ToString();
            DialogResult dr = MessageBox.Show("确定删除"+ name+"吗？", "删除任务", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
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
                    tasksDataView.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
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
            }
            refreshOnce();
        }
    }
}
