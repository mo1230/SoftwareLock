using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
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
        private string zipFile = "";
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
            form2.FolderEnabled = true;
            form2.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void listLockAddAndDel(object sender, EventArgs e)
        {
            
                if (Form2.FileName != null)
                {
                    
                    if (flag == 1)
                    {
                        this.listLock.Items.Add(Path.GetDirectoryName(Form2.FileName) + "\\" + Path.GetFileNameWithoutExtension(Form2.FileName) + ".zip");
                        await Task.Run(() =>
                        {
                            
                            this.CreateZip(Form2.FileName, "123456");
                          
                        });
                        
                        File.Delete(Form2.FileName);
                        
                        Form2.FileName = null;
                    }
                    else
                    {
                        this.listLock.Items.Remove(Form2.FileName);
                        await Task.Run(()=>
                        {
                            
                            this.UnZipFile(Form2.FileName, "123456");
                        });
                        
                        File.Delete(Form2.FileName);
                        Form2.FileName = null;    
                    }


                }
                if (Form2.FolderName != null)
                {
                    this.listLock.Items.Add(Form2.FolderName + ".zip");
                    await Task.Run(() =>
                    {

                        FastZip fastZip = new FastZip();
                        fastZip.Password = md5.encrypt("123456");
                        fastZip.CreateZip(Form2.FolderName + ".zip", Form2.FolderName, true, "");
                        

                    });
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
            form2.FolderEnabled = false;
            form2.ShowDialog();
            
        }


        
        /// <summary>
        /// 加密压缩
        /// </summary>
        /// <param name="pathName">文件完整路径</param>
        /// <param name="password">加密密码</param>
        private void CreateZip(string pathName, string password)
        {
            // 创建一个压缩文件流
            
            using (FileStream fileStream = new FileStream(Path.GetDirectoryName(pathName) + "\\" + Path.GetFileNameWithoutExtension(pathName) + ".zip", FileMode.Create))
            {
                ZipOutputStream zipOutputStream = new ZipOutputStream(fileStream);
                zipOutputStream.Password = md5.encrypt(password);

                // 当前目录
                DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(pathName));
                // 当前目录的文件列表
                FileInfo[] fileInfos = directoryInfo.GetFiles();
                byte[] buffer = new byte[10 * 1024];

                foreach (FileInfo fileInfo in fileInfos)
                {
                    if (fileInfo.FullName != fileStream.Name)
                    {
                        ZipEntry zipEntry = new ZipEntry(fileInfo.Name);
                        zipEntry.Size = fileInfo.Length;
                        zipOutputStream.PutNextEntry(zipEntry);

                        // 写入文件的内容
                        int length = 0;
                        Stream stream = fileInfo.Open(FileMode.Open);
                        while ((length = stream.Read(buffer, 0, 10 * 1024)) > 0)
                        {
                            zipOutputStream.Write(buffer, 0, length);
                        }
                        stream.Close();
                    }
                }

                zipOutputStream.Finish();
                zipOutputStream.Close();
                this.zipFile = Path.GetDirectoryName(pathName) + Path.GetFileNameWithoutExtension(pathName) + ".zip";

            }


        }

       
        
        /// <summary>
        /// 解密解压
        /// </summary>
        /// <param name="pathName">需要解密解压的压缩包完整路径</param>
        /// <param name="password">密码</param>
        private void UnZipFile(string pathName, string password)
        {
            ZipFile zf = new ZipFile(pathName);
            zf.Password = md5.encrypt(password);

            //一个压缩文件内，包括多个被压缩的文件
            foreach (ZipEntry entry in zf)
            {
                //一个被压缩文件,称为一个条目
                Console.WriteLine("压缩包内文件名:" + entry.Name);
                Console.WriteLine("压缩包大小:" + entry.Size);

                //解压出被压缩的文件
                FileStream fs = new FileStream(Path.GetDirectoryName(pathName) + "\\" + Path.GetFileNameWithoutExtension(pathName) + "\\" + entry.Name, FileMode.Create);

                //获取从压缩包中读取数据的流
                Stream input = zf.GetInputStream(entry);

                byte[] buffer = new byte[10 * 1024];

                int length = 0;
                while ((length = input.Read(buffer, 0, 10 * 1024)) > 0)
                {
                    fs.Write(buffer, 0, length);

                }
                fs.Close();
                input.Close();
            }
            zf.Close();
        }



        }

    class md5
    {
        #region "MD5加密"
        /// <summary>
        ///32位 MD5加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <returns></returns>
        public static string encrypt(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }
        #endregion
    }
}
