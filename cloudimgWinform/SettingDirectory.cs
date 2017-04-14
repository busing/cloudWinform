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
            DataView.DataSource = UploadTask.tasks;
            Thread uploadThread=new Thread (UploadTask.MonitorUpload);
            //窗口关闭 线程退出
            uploadThread.IsBackground = true;
            uploadThread.Start();
            //控件跨线程更新
            Control.CheckForIllegalCrossThreadCalls = false;
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

        private static void pushUpload(string[] files)
        {
            foreach (String file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                String fileName = file.Substring(file.LastIndexOf("\\") + 1);
                UploadTask t = new UploadTask(fileName, file, 2, fileInfo.Length);
                UploadTask.tasks.Add(t);
            }
        }


        //数据字段格式化
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //状态
            if (this.tasksDataView.Columns["status"].Index == e.ColumnIndex && e.RowIndex >= 0)
            {
                switch (e.Value){
                    case 0:
                       e.Value = "等待中";
                       break;
                    case 1:
                        e.Value = "转化中";
                        break;
                    case 2:
                        e.Value = "转化成功";
                        break;
                    case 3:
                        e.Value = "转化失败";
                        break;
                    case 4:
                        e.Value = "上传中";
                        break;
                    case 5:
                        e.Value = "上传成功";
                        break;
                    case 6:
                        e.Value = "上传失败";
                        break;
                }
            }
            //文件大小
            else if (this.tasksDataView.Columns["fileSize"].Index == e.ColumnIndex && e.RowIndex >= 0)
            {
                long fileSize = (long)e.Value;
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
            String name = tasksDataView.CurrentRow.Cells[0].Value.ToString();
            DialogResult dr = MessageBox.Show("确定删除"+ name+"吗？", "删除任务", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                UploadTask.tasks.RemoveAt(row);
            }
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

        //窗口关闭事件
        private void SettingDirectory_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定退出程序吗？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Application.Exit();
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
            String[] files = e.Data.GetData(DataFormats.FileDrop, false) as String[];
            pushUpload(files);
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
    }
}
