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
            this.choose = new System.Windows.Forms.Button();
            this.tasksDataView = new System.Windows.Forms.DataGridView();
            this.delTask = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.delTasks = new System.Windows.Forms.ToolStripMenuItem();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.tasksDataView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
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
            // 
            // delTasks
            // 
            this.delTasks.Name = "delTasks";
            this.delTasks.Size = new System.Drawing.Size(124, 22);
            this.delTasks.Text = "删除任务";
            this.delTasks.Click += new System.EventHandler(this.delTasks_Click);
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
            // SettingDirectory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 344);
            this.Controls.Add(this.tasksDataView);
            this.Controls.Add(this.choose);
            this.Name = "SettingDirectory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择自动上传目录";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingDirectory_FormClosed);
            this.Load += new System.EventHandler(this.SettingDirectory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tasksDataView)).EndInit();
            this.delTask.ResumeLayout(false);
            this.ResumeLayout(false);

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
    }
}