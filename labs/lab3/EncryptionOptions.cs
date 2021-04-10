using System;

namespace lab3
{
    public class EncryptionOptions
    {
        private bool encryption;
        private string encryptionKey;        
        public bool Encryption
        {
            get => encryption;
            set => encryption = value;
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
        public EncryptionOptions() { }
    }
}