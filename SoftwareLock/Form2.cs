using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareLock
{
    public partial class Form2 : Form
    {
        public static string FileName { get; set; }
        public static string FolderName { get; set; }
        private bool folderEnaled;
        public bool FolderEnabled
        {
            set
            {
                this.folderEnaled = value;
                if(this.folderEnaled == true)
                {
                    this.btnFolder.Enabled = true;
                }
                else
                {
                    this.btnFolder.Enabled = false;
                }
            }
        }
        private bool fileEnabled;
        public bool FileEnabled
        {
            set
            {
                this.fileEnabled = value;
                if(this.fileEnabled == true)
                {
                    this.btnFile.Enabled = true;
                }
                else
                {
                    this.btnFile.Enabled = false;
                }
            }
        }
        public Form2()
        {
            InitializeComponent();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "所有文件|*.*";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //string strFileName = ofd.FileName;
                //其他代码
                FileName = ofd.FileName;
                this.Close();
                ofd.Dispose();
                /*Thread.Sleep(1000);
                FileName = null;*/
                //MessageBox.Show(strFileName);
            }
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "请选择所要锁定的文件夹";
            if (folder.ShowDialog() == DialogResult.OK)
            {
                FolderName = folder.SelectedPath;
                this.Close();
                //string sPath = folder.SelectedPath;
                //MessageBox.Show(sPath);
            }
        }
    }
}
