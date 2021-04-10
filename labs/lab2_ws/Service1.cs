using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using FM_dll;
using CP_dll;
using Options;

namespace lab2_ws
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //xml
            ValidateXml(@"D:\Projects\cs\labs\lab3\Config\config.xml", @"D:\Projects\cs\labs\lab3\Config\config.xsd");
            var provider = new ConfigProvider(@"D:\Projects\cs\labs\lab3\Config\config.xml");
            var xmlConfig = provider.GetConfig<ConfigModel>();
            //json
            provider = new ConfigProvider(@"D:\Projects\cs\labs\lab3\Config\appsettings.json");
            var jsonConfig = provider.GetConfig<ConfigModel>();

            FileManager.MonitorDir(jsonConfig.SourcePath, CreateHandler);
            FileManager.MonitorDir(xmlConfig.TargetPath, CreateHandler);
        }

        protected override void OnStop()
        {
        }

        private void ValidateXml(string xml, string xsd)
        {
            var schema = new XmlSchemaSet();
            schema.Add(string.Empty, xsd);
            XDocument doc = XDocument.Load(xml);
            doc.Validate(schema, ValidationHandler);

        }
        private static void ValidationHandler(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.WriteLine("\tWarning: Xsd schema not found. No validation occurred. " + args.Message);
            else
                Console.WriteLine("\tValidation error: " + args.Message);
        }

        private static void OnCreated(object source, FileSystemEventArgs args)
        {
            try
            {
                var provider = new ConfigProvider(@"D:\Projects\cs\labs\lab3\Config\config.xml");
                var xmlConfig = provider.GetConfig<ConfigModel>();
                var key = xmlConfig.EncryptionKey;
                var encryption = xmlConfig.Encryption;
                var compression = xmlConfig.Compression;
                var sourcePath = xmlConfig.SourcePath;
                var targetPath = xmlConfig.TargetPath;
                var ext = Path.GetExtension(args.FullPath);                

                if (ext == ".txt")
                {
                    var time = File.GetCreationTime(args.FullPath);
                    var newFilePath = Path.Combine(targetPath, time.Year.ToString());
                    newFilePath = Path.Combine(newFilePath, time.Month.ToString());
                    newFilePath = Path.Combine(newFilePath, time.Day.ToString());
                    newFilePath = Path.Combine(newFilePath, Path.GetFileName(args.FullPath));
                    var newName = Path.ChangeExtension(newFilePath, ".gz");
                    using (StreamWriter logStr = new StreamWriter(@"D:\Projects\cs\labs\lab3\Logs\logs.txt"))
                    {
                        logStr.Write(newFilePath);
                    }
                    if (!Directory.Exists(Path.GetDirectoryName(newFilePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(newFilePath));
                        File.Move(args.FullPath, newFilePath);
                        return;
                    }
                    FileManager.SendFile(args.FullPath, newName, encryption, key, compression);
                }
                else if (ext == ".gz")
                {
                    FileManager.ReceiveFile(args.FullPath, Path.ChangeExtension(args.FullPath, ".txt"), encryption, key, compression);
                }
            }
            catch (Exception expt)
            {
                using (StreamWriter sw = File.AppendText(@"D:\Projects\cs\labs\lab3\Logs\logs.txt"))
                {
                    sw.WriteLine(expt.Message);
                }
            }
        }
        public readonly FileManager.OnCreate CreateHandler = OnCreated;
    }
}
