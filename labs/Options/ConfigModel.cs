using System;
using System.IO;

namespace Options
{
    public class ConfigModel
    {
        private bool encryption;
        private bool compression;
        private string encryptionKey;
        private string sourcePath;
        private string targetPath;
        public bool Encryption
        {
            get => encryption;
            set => encryption = value;
        }
        public bool Compression
        {
            get => compression;
            set => compression = value;
        }
        public string EncryptionKey
        {
            get => encryptionKey;
            set
            {
                if (value != null && value.Length != 8)
                {
                    throw new Exception("Encryption key must consist of 8 symbols");
                }
                encryptionKey = value;
            }
        }
        public string SourcePath
        {
            get => sourcePath;
            set
            {
                if (!Directory.Exists(value)) throw new Exception("Source directory doesnt exist");
                sourcePath = value;
            }
        }
        public string TargetPath
        {
            get => targetPath;
            set
            {
                if (!Directory.Exists(value))
                {
                    Directory.CreateDirectory(value);
                }
                targetPath = value;
            }
        }
        public ConfigModel(string sourcePath, string targetPath, bool compression, bool encryption, string encryptionKey)
        {
            if (!Directory.Exists(sourcePath))
            {
                throw new Exception("Source directory doesnt exist");
            }
            this.sourcePath = sourcePath;
            this.targetPath = targetPath;
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            this.compression = compression;
            this.encryption = encryption;
            if (encryptionKey != null)
            {
                this.encryptionKey = encryptionKey;
            }
        }
        public ConfigModel() { }
    }
}
