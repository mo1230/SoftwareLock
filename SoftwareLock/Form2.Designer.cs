namespace SoftwareLock
{
    partial class Form2
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
            this.btnFile = new System.Windows.Forms.Button();
            this.btnFolder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(25, 21);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(289, 39);
            this.btnFile.TabIndex = 0;
            this.btnFile.Text = "选择文件";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // btnFolder
            // 
            this.btnFolder.Location = new System.Drawing.Point(25, 83);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(289, 39);
            this.btnFolder.TabIndex = 0;
            this.btnFolder.Text = "选择文件夹";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 158);
            this.Controls.Add(this.btnFolder);
            this.Controls.Add(this.btnFile);
            this.MaximizeBox = false;
            this.Name = "Form2";
            this.Text = "选择";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Button btnFolder;
    }
}