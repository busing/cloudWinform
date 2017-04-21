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
            this.delTask = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.delTasks = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cleanTask = new System.Windows.Forms.LinkLabel();
            this.cleanAllTask = new System.Windows.Forms.LinkLabel();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scan_rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.width = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.height = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resolution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.id,
            this.name,
            this.path,
            this.fileSize,
            this.scan_rate,
            this.width,
            this.height,
            this.resolution,
            this.status});
            this.tasksDataView.Location = new System.Drawing.Point(12, 41);
            this.tasksDataView.Name = "tasksDataView";
            this.tasksDataView.ReadOnly = true;
            this.tasksDataView.RowTemplate.Height = 23;
            this.tasksDataView.Size = new System.Drawing.Size(941, 345);
            this.tasksDataView.TabIndex = 4;
            this.tasksDataView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tasksDataView_CellContentClick);
            this.tasksDataView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.tasksDataView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.tasksDataView_CellMouseDown);
            // 
            // delTask
            // 
            this.delTask.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.delTasks});
            this.delTask.Name = "delTask";
            this.delTask.Size = new System.Drawing.Size(125, 26);
            this.delTask.Text = "删除";
            this.delTask.Opening += new System.ComponentModel.CancelEventHandler(this.delTask_Opening);
            // 
            // delTasks
            // 
            this.delTasks.Name = "delTasks";
            this.delTasks.Size = new System.Drawing.Size(124, 22);
            this.delTasks.Text = "删除任务";
            this.delTasks.Click += new System.EventHandler(this.delTasks_Click);
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
            // cleanTask
            // 
            this.cleanTask.AutoSize = true;
            this.cleanTask.Location = new System.Drawing.Point(888, 23);
            this.cleanTask.Name = "cleanTask";
            this.cleanTask.Size = new System.Drawing.Size(65, 12);
            this.cleanTask.TabIndex = 6;
            this.cleanTask.TabStop = true;
            this.cleanTask.Text = "清除已完成";
            this.cleanTask.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.cleanTask_LinkClicked);
            // 
            // cleanAllTask
            // 
            this.cleanAllTask.AutoSize = true;
            this.cleanAllTask.Location = new System.Drawing.Point(807, 23);
            this.cleanAllTask.Name = "cleanAllTask";
            this.cleanAllTask.Size = new System.Drawing.Size(53, 12);
            this.cleanAllTask.TabIndex = 7;
            this.cleanAllTask.TabStop = true;
            this.cleanAllTask.Text = "清除所有";
            this.cleanAllTask.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.cleanAllTask_LinkClicked);
            // 
            // id
            // 
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "id";
            this.id.MaxInputLength = 20;
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            this.id.Width = 42;
            // 
            // name
            // 
            this.name.ContextMenuStrip = this.delTask;
            this.name.DataPropertyName = "name";
            this.name.HeaderText = "文件名";
            this.name.Name = "name";
            this.name.ReadOnly = true;
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
            this.fileSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.fileSize.ContextMenuStrip = this.delTask;
            this.fileSize.DataPropertyName = "size";
            this.fileSize.HeaderText = "文件大小";
            this.fileSize.Name = "fileSize";
            this.fileSize.ReadOnly = true;
            this.fileSize.Width = 78;
            // 
            // scan_rate
            // 
            this.scan_rate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.scan_rate.DataPropertyName = "scan_rate";
            this.scan_rate.HeaderText = "扫描倍率";
            this.scan_rate.Name = "scan_rate";
            this.scan_rate.ReadOnly = true;
            this.scan_rate.Width = 78;
            // 
            // width
            // 
            this.width.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.width.ContextMenuStrip = this.delTask;
            this.width.DataPropertyName = "width";
            this.width.HeaderText = "宽度(px)";
            this.width.Name = "width";
            this.width.ReadOnly = true;
            this.width.Width = 78;
            // 
            // height
            // 
            this.height.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.height.DataPropertyName = "height";
            this.height.HeaderText = "高度(px)";
            this.height.Name = "height";
            this.height.ReadOnly = true;
            this.height.Width = 78;
            // 
            // resolution
            // 
            this.resolution.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.resolution.DataPropertyName = "resolution";
            this.resolution.HeaderText = "比例尺";
            this.resolution.Name = "resolution";
            this.resolution.ReadOnly = true;
            this.resolution.Width = 66;
            // 
            // status
            // 
            this.status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.status.ContextMenuStrip = this.delTask;
            this.status.DataPropertyName = "status";
            this.status.HeaderText = "状态";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Width = 54;
            // 
            // SettingDirectory
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(965, 408);
            this.Controls.Add(this.cleanAllTask);
            this.Controls.Add(this.cleanTask);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tasksDataView);
            this.Controls.Add(this.choose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SettingDirectory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择自动上传目录";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingDirectory_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingDirectory_FormClosed);
            this.Load += new System.EventHandler(this.SettingDirectory_Load);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.LinkLabel cleanTask;
        private System.Windows.Forms.LinkLabel cleanAllTask;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn path;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn scan_rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn width;
        private System.Windows.Forms.DataGridViewTextBoxColumn height;
        private System.Windows.Forms.DataGridViewTextBoxColumn resolution;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
    }
}