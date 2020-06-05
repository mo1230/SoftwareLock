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
        private int flag = 0;   // 0为解密 1为加密
        public Form1()
        {
            InitializeComponent();
            // 定时器 每隔一秒执行
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(listLockAddAndDel);
            timer.Enabled = true;
            timer.Start();

        }

   

        private void btnLock_Click(object sender, EventArgs e)
        {

            this.flag = 1;
            form2.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listLockAddAndDel(object sender, EventArgs e)
        {
            //try {
                if (Form2.FileName != null)
                {
                    if (flag == 1)
                    {
                        this.listLock.Items.Add(Form2.FileName);
                        this.EncryptFile(Form2.FileName, Form2.FileName + ".lock");
                        File.Delete(Form2.FileName);
                        Form2.FileName = null;
                    }
                    else
                    {
                        this.listLock.Items.Remove(Form2.FileName.Substring(0, Form2.FileName.Length - 5));
                        this.DecryptFile(Form2.FileName, Form2.FileName.Substring(0, Form2.FileName.Length - 5));
                        File.Delete(Form2.FileName);
                        Form2.FileName = null;    
                    }


                }
                if (Form2.FolderName != null)
                {
                    if (flag == 1)
                    {
                        this.listLock.Items.Add(Form2.FolderName);

                        Form2.FolderName = null;
                    }
                    else
                    {
                        this.listLock.Items.Remove(Form2.FolderName);
                        this.DecryptFile(Form2.FolderName, Form2.FolderName.Substring(0, Form2.FolderName.Length - 5));
                        Form2.FolderName = null;
                    }

                }
            /*}catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/
            
            
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


        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="sInputFilename">待解密的文件的完整路径</param>
        /// <param name="sOutputFilename">解密后的文件的完整路径</param>
        private bool DecryptFile(string sInputFilename, string sOutputFilename)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sSecretKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sSecretKey);

            FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);
            StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
            fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
            fsDecrypted.Flush();
            fsDecrypted.Close();
            fsread.Close();
            return true;
        }


        private void btnUnlock_Click(object sender, EventArgs e)
        {
            this.flag = 0;
            form2.ShowDialog();
        }
    }
}
