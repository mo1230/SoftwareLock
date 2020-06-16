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
        private const string pwd = "(>SR4B]Wqf";
        private const string settingFilePath = "zipfiles.txt";
        private Form2 form2 = new Form2();
        private int flag = 0;   // 0为解密 1为加密
        private string zipFile = "";
        private List<string> settingFileContent = new List<string>();
        public Form1()
        {
            InitializeComponent();

            // zipfiles.txt文件
            this.SettingFile();
            this.listLock.Items.AddRange(this.settingFileContent.ToArray());

            // 定时器 每隔一秒执行
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(listLockAddAndDel);
            timer.Enabled = true;
            timer.Start();

            // 判断时间
            MessageBox.Show("当前时间为：" + DateTime.Now.ToLongTimeString() + "\n19:00前无法解锁！", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DateTime unLockTime = Convert.ToDateTime("19:00");
            int i = DateTime.Now.CompareTo(unLockTime);
            if(i <= 0)
            {
                this.btnUnlock.Enabled = false;
            }
            else
            {
                this.btnUnlock.Enabled = true;
            }   
        }

 
        private void btnLock_Click(object sender, EventArgs e)
        {

            this.flag = 1;
            form2.FolderEnabled = true;
            form2.FileEnabled = false;
            form2.ShowDialog();
        }

        /// <summary>
        /// 列表内容的添加与删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listLockAddAndDel(object sender, EventArgs e)
        {
            

            if (Form2.FileName != null)
                {
                    
                    if (flag == 1)
                    {
                        string fileName = Form2.FileName;
                        Form2.FileName = null;
                        string item = Path.GetDirectoryName(fileName) + "\\" + Path.GetFileNameWithoutExtension(fileName) + ".zip";
                        this.listLock.Items.Add(item);
                        List<string> fileItems = new List<string>();
                        fileItems.Add(item);
                        // 路径写入到zipfiles.txt中
                        File.AppendAllLines(settingFilePath, fileItems);
                        
                            
                        this.CreateZip(fileName, pwd);
                          
                       
                        
                        File.Delete(fileName);
                        
                        
                    }
                    else
                    {
                        string fileName = Form2.FileName;
                        Form2.FileName = null;
                        this.listLock.Items.Remove(fileName);
                        int index = this.settingFileContent.IndexOf(fileName);
                        if(index >= 0)
                        {
                        this.settingFileContent.RemoveAt(index);
                        }
                        
                        File.WriteAllLines(settingFilePath, this.settingFileContent.ToArray<string>());
                        
                        this.UnZipFile(fileName, pwd);

                        
                        File.Delete(fileName);
                            
                    }
                }
            if (Form2.FolderName != null)
                {
                    string folderName = Form2.FolderName;
                    Form2.FolderName = null;
                    string item = folderName + ".zip";
                    this.listLock.Items.Add(item);
                    List<string> fileitems = new List<string>();
                    fileitems.Add(item);
                    File.AppendAllLines(settingFilePath, fileitems);

                    FastZip fastZip = new FastZip();
                    fastZip.Password = md5.encrypt(pwd);
                    fastZip.CreateZip(item, folderName, true, ""); 
                }   
        }


       

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            this.flag = 0;
            form2.FolderEnabled = false;
            form2.FileEnabled = true;
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
                FileStream fs = new FileStream(Path.GetDirectoryName(pathName) + "\\" /*+ Path.GetFileNameWithoutExtension(pathName) + "\\"*/ + entry.Name, FileMode.Create);

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

        private void SettingFile()
        {
            
            bool exist = File.Exists(settingFilePath);
            if (!exist)
            {
                FileStream file = File.Create(settingFilePath);
                file.Close();
            }
            this.settingFileContent = (File.ReadAllLines(settingFilePath)).ToList<string>();
            
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
