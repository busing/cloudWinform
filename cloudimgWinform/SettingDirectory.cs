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
        public SettingDirectory()
        {
            InitializeComponent();
            this.tasksDataView.DataSource = UploadTask.tasks;
            new Thread (UploadTask.MonitorUpload).Start();
        }

        private void SettingDirectory_Load(object sender, EventArgs e)
        {

        }

        private void choose_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = fileDialog.FileName;
                FileInfo fileInfo = new FileInfo(file);
                String fileName = file.Substring(file.LastIndexOf("\\")+1);
                UploadTask t = new UploadTask(fileName, file,2, fileInfo.Length);
                UploadTask.tasks.Add(t);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
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
                //kb
                else if (fileSize / (1024*1024) > 0 && fileSize / (1024 * 1024* 1024) == 0)
                {
                    e.Value = ((float)fileSize / 1024/1024).ToString("F2") + "M";
                }
                //kb
                else if (fileSize / (1024*1024*1024) > 0)
                {
                    e.Value = ((float)fileSize / 1024 / 1024/1024).ToString("F2") + "G";
                }
                
            }
        }

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

        private void SettingDirectory_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定退出程序吗？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Application.Exit();
            }
        }

    }
}
