namespace SoftwareLock
{
    partial class Form1
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
            this.btnLock = new System.Windows.Forms.Button();
            this.btnUnlock = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listLock = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnLock
            // 
            this.btnLock.Location = new System.Drawing.Point(35, 12);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(298, 42);
            this.btnLock.TabIndex = 0;
            this.btnLock.Text = "上锁";
            this.btnLock.UseVisualStyleBackColor = true;
            this.btnLock.Click += new System.EventHandler(this.btnLock_Click);
            // 
            // btnUnlock
            // 
            this.btnUnlock.Location = new System.Drawing.Point(35, 92);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(298, 42);
            this.btnUnlock.TabIndex = 0;
            this.btnUnlock.Text = "解锁";
            this.btnUnlock.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("楷体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(33, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "已上锁文件或文件夹：";
            // 
            // listLock
            // 
            this.listLock.FormattingEnabled = true;
            this.listLock.ItemHeight = 12;
            this.listLock.Location = new System.Drawing.Point(37, 233);
            this.listLock.Name = "listLock";
            this.listLock.Size = new System.Drawing.Size(296, 88);
            this.listLock.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 450);
            this.Controls.Add(this.listLock);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnUnlock);
            this.Controls.Add(this.btnLock);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLock;
        private System.Windows.Forms.Button btnUnlock;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ListBox listLock;
    }
}

