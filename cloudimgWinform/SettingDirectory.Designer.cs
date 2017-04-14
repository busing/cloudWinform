namespace cloudimgWinform
{
    partial class SettingDirectory
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingDirectory));
            this.choose = new System.Windows.Forms.Button();
            this.tasksDataView = new System.Windows.Forms.DataGridView();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.delTask = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.delTasks = new System.Windows.Forms.ToolStripMenuItem();
            this.path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.tasksDataView)).BeginInit();
            this.delTask.SuspendLayout();
            this.SuspendLayout();
            // 
            // choose
            // 
            this.choose.Location = new System.Drawing.Point(12, 12);
            this.choose.Name = "choose";
            this.choose.Size = new System.Drawing.Size(75, 23);
            this.choose.TabIndex = 2;
            this.choose.Text = "上传文件";
            this.choose.UseVisualStyleBackColor = true;
            this.choose.Click += new System.EventHandler(this.choose_Click);
            // 
            // tasksDataView
            // 
            this.tasksDataView.AllowUserToAddRows = false;
            this.tasksDataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tasksDataView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.name,
            this.path,
            this.fileSize,
            this.status});
            this.tasksDataView.Location = new System.Drawing.Point(12, 41);
            this.tasksDataView.Name = "tasksDataView";
            this.tasksDataView.ReadOnly = true;
            this.tasksDataView.RowTemplate.Height = 23;
            this.tasksDataView.Size = new System.Drawing.Size(660, 280);
            this.tasksDataView.TabIndex = 4;
            this.tasksDataView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.tasksDataView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.tasksDataView_CellMouseDown);
            // 
            // name
            // 
            this.name.ContextMenuStrip = this.delTask;
            this.name.DataPropertyName = "name";
            this.name.Frozen = true;
            this.name.HeaderText = "文件名";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // delTask
            // 
            this.delTask.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.delTasks});
            this.delTask.Name = "delTask";
            this.delTask.Size = new System.Drawing.Size(125, 26);
            this.delTask.Text = "删除";
            // 
            // delTasks
            // 
            this.delTasks.Name = "delTasks";
            this.delTasks.Size = new System.Drawing.Size(124, 22);
            this.delTasks.Text = "删除任务";
            this.delTasks.Click += new System.EventHandler(this.delTasks_Click);
            // 
            // path
            // 
            this.path.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.path.ContextMenuStrip = this.delTask;
            this.path.DataPropertyName = "path";
            this.path.HeaderText = "路径";
            this.path.Name = "path";
            this.path.ReadOnly = true;
            // 
            // fileSize
            // 
            this.fileSize.ContextMenuStrip = this.delTask;
            this.fileSize.DataPropertyName = "size";
            this.fileSize.HeaderText = "文件大小";
            this.fileSize.Name = "fileSize";
            this.fileSize.ReadOnly = true;
            // 
            // status
            // 
            this.status.ContextMenuStrip = this.delTask;
            this.status.DataPropertyName = "status";
            this.status.HeaderText = "状态";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(91, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "选择或拖动文件上传";
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "泰瑞云图";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            // 
            // SettingDirectory
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 344);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tasksDataView);
            this.Controls.Add(this.choose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SettingDirectory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择自动上传目录";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingDirectory_FormClosed);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.SettingDirectory_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.SettingDirectory_DragEnter);
            this.Resize += new System.EventHandler(this.SettingDirectory_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.tasksDataView)).EndInit();
            this.delTask.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button choose;
        private System.Windows.Forms.DataGridView tasksDataView;
        private System.Windows.Forms.ContextMenuStrip delTask;
        private System.Windows.Forms.ToolStripMenuItem delTasks;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn path;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NotifyIcon notifyIcon;
    }
}