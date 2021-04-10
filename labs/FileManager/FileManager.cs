using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.IO.Compression;

namespace FM_dll
{
    public class FileManager
    {
        public static void SendFile(string sourcePath, string targetPath, bool encryption, string key, bool compression)
        {
            if (!Directory.Exists(Path.GetDirectoryName(targetPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
            }
            if (!encryption)
            {
                SendFile(sourcePath, targetPath, compression);
            }
            DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider();
            cryptic.Key = ASCIIEncoding.ASCII.GetBytes(key);
            cryptic.IV = ASCIIEncoding.ASCII.GetBytes(key);
            using (CryptoStream fs = new CryptoStream(new FileStream(sourcePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite), cryptic.CreateEncryptor(), CryptoStreamMode.Read))
            {
                using (FileStream ts = new FileStream(targetPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    if (compression)
                    {
                        using (GZipStream compressionStream = new GZipStream(ts, CompressionMode.Compress))
                        {
                            fs.CopyTo(compressionStream);
                        }
                    }
                    else
                    {
                        fs.CopyTo(ts);
                    }
                }
            }
        }

        public static void SendFile(string sourcePath, string targetPath, bool compress)
        {
            if (!Directory.Exists(Path.GetDirectoryName(targetPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
            }
            using (FileStream fs = new FileStream(sourcePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (FileStream ts = new FileStream(targetPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    if (compress)
                    {
                        using (GZipStream compressionStream = new GZipStream(ts, CompressionMode.Compress))
                        {
                            fs.CopyTo(compressionStream);
                        }
                    }
                    else
                    {
                        fs.CopyTo(ts);
                    }
                }
            }
        }

        public static void ReceiveFile(string path, string targetFile, bool encryption, string key, bool compression)
        {
            if (!encryption) ReceiveFile(path, targetFile, compression);
            DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider();
            cryptic.Key = ASCIIEncoding.ASCII.GetBytes(key);
            cryptic.IV = ASCIIEncoding.ASCII.GetBytes(key);
            using (FileStream sourceStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (FileStream targetStream = File.Create(targetFile))
                {
                    if (compression)
                    {
                        using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                        {
                            using (CryptoStream cs = new CryptoStream(decompressionStream, cryptic.CreateDecryptor(), CryptoStreamMode.Read))
                            {
                                cs.CopyTo(targetStream);
                            }
                        }
                    }
                    else
                    {
                        using (CryptoStream cs = new CryptoStream(sourceStream, cryptic.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            cs.CopyTo(targetStream);
                        }
                    }
                }
            }
        }

        public static void ReceiveFile(string path, string targetFile, bool compress)
        {
            using (FileStream sourceStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (FileStream targetStream = File.Create(targetFile))
                {
                    if (compress)
                    {
                        using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(targetStream);
                        }
                    }
                    else
                    {
                        sourceStream.CopyTo(targetStream);
                    }
                }
            }
        }

        public static void MonitorDir(string path, OnCreate OnCreated)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.IncludeSubdirectories = true;
            var handler = new FileSystemEventHandler(OnCreated);
            watcher.Created += handler;
            watcher.EnableRaisingEvents = true;
        }
        public delegate void OnCreate(object source, FileSystemEventArgs e);
        public static OnCreate onCreated;
    }
}