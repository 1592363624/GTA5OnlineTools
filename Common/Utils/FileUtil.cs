﻿using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Controls;

namespace GTA5OnlineTools.Common.Utils
{
    public class FileUtil
    {
        public const string Default_Path = @"C:\ProgramData\GTA5OnlineTools\";
        public const string Config_Path = Default_Path + @"Config\";
        public const string WebCache_Path = Default_Path + @"Cache\";
        public const string Temp_Path = Default_Path + @"Temp\";
        public const string Kiddion_Path = Default_Path + @"Kiddion\";
        public const string KiddionScripts_Path = Kiddion_Path + @"scripts\";
        public const string GTAHaxStat_Path = Temp_Path + @"stat.txt";
        public static string CustomTeleportList_Path = Config_Path + "CustomTPList.json";

        public const string Resource_Path = "GTA5OnlineTools.Features.Files.";

        /// <summary>
        /// 获取当前运行文件完整路径
        /// </summary>
        public static string Current_Path = Process.GetCurrentProcess().MainModule.FileName;

        /// <summary>
        /// 获取当前文件目录，不加文件名及后缀
        /// </summary>
        public static string CurrentDirectory_Path = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 我的文档完整路径
        /// </summary>
        public static string MyDocuments_Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        /// <summary>
        /// 给文件名，得出当前目录完整路径，AppName带文件名后缀
        /// </summary>
        public static string GetCurrFullPath(string AppName)
        {
            return Path.Combine(CurrentDirectory_Path, AppName);
        }

        /// <summary>
        /// 文件重命名
        /// </summary>
        public static void FileReName(string OldPath, string NewPath)
        {
            FileInfo ReName = new FileInfo(OldPath);
            ReName.MoveTo(NewPath);
        }

        public static void ReadTextToTextBox(TextBox textBoxName, string path)
        {
            textBoxName.Clear();

            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                textBoxName.AppendText(line + Environment.NewLine);
            }
        }

        /// <summary>
        /// 保存指定Log文件到本地
        /// </summary>
        /// <param name="logContent">保存内容</param>
        public static void SaveErrorLog(string logContent)
        {
            try
            {
                string path = Config_Path + "ErrorLog";
                Directory.CreateDirectory(path);
                path += $@"\#ErrorLog# { DateTime.Now:yyyyMMdd_HH-mm-ss_ffff}.log";
                File.WriteAllText(path, logContent);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 从资源文件中抽取资源文件
        /// </summary>
        /// <param name="resFileName">资源文件名称（资源文件名称必须包含目录，目录间用“.”隔开,最外层是项目默认命名空间）</param>
        /// <param name="outputFile">输出文件</param>
        public static void ExtractResFile(string resFileName, string outputFile)
        {
            BufferedStream inStream = null;
            FileStream outStream = null;
            try
            {
                // 读取嵌入式资源
                Assembly asm = Assembly.GetExecutingAssembly();
                inStream = new BufferedStream(asm.GetManifestResourceStream(resFileName));
                outStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write);

                byte[] buffer = new byte[1024];
                int length;

                while ((length = inStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outStream.Write(buffer, 0, length);
                }
                outStream.Flush();
            }
            finally
            {
                if (outStream != null)
                {
                    outStream.Close();
                }
                if (inStream != null)
                {
                    inStream.Close();
                }
            }
        }

        /// <summary>
        /// 清空指定文件夹下的文件及文件夹
        /// </summary>
        /// <param name="srcPath">文件夹路径</param>
        public static void DelectDir(string srcPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();   // 返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)                             // 判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);                            // 删除子目录和文件
                    }
                    else
                    {
                        File.Delete(i.FullName);                        // 删除指定文件
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBoxUtil.ExceptionMsgBox(ex);
            }
        }
    }
}