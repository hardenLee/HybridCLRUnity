﻿using System;
using System.Collections.Generic;
using System.IO;

public static class FileHelper
{
    public static List<string> GetAllFiles(string dir, string searchPattern = "*")
    {
        List<string> list = new List<string>();
        GetAllFiles(list, dir, searchPattern);
        return list;
    }

    public static void GetAllFiles(List<string> files, string dir, string searchPattern = "*")
    {
        string[] fls = Directory.GetFiles(dir);
        foreach (string fl in fls)
        {
            files.Add(fl);
        }

        string[] subDirs = Directory.GetDirectories(dir);
        foreach (string subDir in subDirs)
        {
            GetAllFiles(files, subDir, searchPattern);
        }
    }

    public static void CleanDirectory(string dir)
    {
        if (!Directory.Exists(dir))
        {
            return;
        }

        foreach (string subdir in Directory.GetDirectories(dir))
        {
            Directory.Delete(subdir, true);
        }

        foreach (string subFile in Directory.GetFiles(dir))
        {
            File.Delete(subFile);
        }
    }

    public static void CopyDirectory(string srcDir, string tgtDir)
    {
        DirectoryInfo source = new DirectoryInfo(srcDir);
        DirectoryInfo target = new DirectoryInfo(tgtDir);

        if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new Exception("父目录不能拷贝到子目录！");
        }

        if (!source.Exists)
        {
            return;
        }

        if (!target.Exists)
        {
            target.Create();
        }

        FileInfo[] files = source.GetFiles();

        for (int i = 0; i < files.Length; i++)
        {
            File.Copy(files[i].FullName, Path.Combine(target.FullName, files[i].Name), true);
        }

        DirectoryInfo[] dirs = source.GetDirectories();

        for (int j = 0; j < dirs.Length; j++)
        {
            CopyDirectory(dirs[j].FullName, Path.Combine(target.FullName, dirs[j].Name));
        }
    }

    public static void ReplaceExtensionName(string srcDir, string extensionName, string newExtensionName)
    {
        if (Directory.Exists(srcDir))
        {
            string[] fls = Directory.GetFiles(srcDir);

            foreach (string fl in fls)
            {
                if (fl.EndsWith(extensionName))
                {
                    File.Move(fl, fl.Substring(0, fl.IndexOf(extensionName)) + newExtensionName);
                    File.Delete(fl);
                }
            }

            string[] subDirs = Directory.GetDirectories(srcDir);

            foreach (string subDir in subDirs)
            {
                ReplaceExtensionName(subDir, extensionName, newExtensionName);
            }
        }
    }

    /// <summary>
    /// 字节单位换算
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string ByteConversion(double length)
    {
        string temp;
        if (length < 1024)
        {
            temp = Math.Round(length, 0) + "B";
        }
        else if (length < 1024 * 1024)
        {
            temp = Math.Round(length / (1024), 0) + "KB";
        }
        else if (length < 1024 * 1024 * 1024)
        {
            temp = Math.Round(length / (1024 * 1024), 2) + "MB";
        }
        else
        {
            temp = Math.Round(length / (1024 * 1024 * 1024), 2) + "GB";
        }

        return temp;
    }

    /// <summary>
    /// 文件是否大于xxxMB
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="mbNum"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public static bool IsFileLargerThanMB(string filePath, int mbNum = 30)
    {
        // 获取文件的大小
        long fileSize = new FileInfo(filePath).Length;

        // 将字节转换为兆字节，1MB = 1024 * 1024 字节
        double fileSizeInMB = fileSize / (1024.0 * 1024.0);

        // 判断文件是否超过30MB
        return fileSizeInMB > mbNum;
    }
}