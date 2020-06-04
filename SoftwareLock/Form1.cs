using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareLock
{
    public partial class Form1 : Form
    {

        private readonly string sSecretKey = "?\a??64(?";
        private Form2 form2 = new Form2();
        public Form1()
        {
            InitializeComponent();
            // 定时器 每隔一秒执行
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(listLockAdd);
            timer.Enabled = true;
            timer.Start();

        }

   

        private void btnLock_Click(object sender, EventArgs e)
        {
            
            
            form2.ShowDialog();
        }

        private void listLockAdd(object sender, EventArgs e)
        {
            if(Form2.FileName != null)
            {
                this.listLock.Items.Add(Form2.FileName);
                this.EncryptFile(Form2.FileName, Form2.FileName + ".txt");
                Form2.FileName = null;
                
            }
            if(Form2.FolderName != null)
            {
                this.listLock.Items.Add(Form2.FolderName);
                
                Form2.FolderName = null;
            }
            
        }

        // 加密
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="sInputFilename">待加密的文件的完整路径</param>
        /// <param name="sOutputFilename">加密后的文件的完整路径</param>
        private void EncryptFile(string sInputFilename, string sOutputFilename)
        {
            FileStream fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);

            FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sSecretKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sSecretKey);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);

            byte[] bytearrayinput = new byte[fsInput.Length];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);

            cryptostream.Flush();
            fsInput.Flush();
            fsEncrypted.Flush();
            cryptostream.Close();
            fsInput.Close();
            fsEncrypted.Close();
        }
    }
}
