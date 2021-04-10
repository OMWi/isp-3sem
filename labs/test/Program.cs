using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Options;
using CP_dll;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace test
{
    class Program
    {
        static void Main()
        {
            ConfigModel config = new ConfigModel(@"D:\Projects\cs\labs\Files\SourceDir", 
                @"D:\Projects\cs\labs\Files\TargetDir", true, false, "11111111");

            ConfigProvider provider = new ConfigProvider(@"D:\Projects\cs\labs\lab3\Config\config.xml");
            var xmlConfig = provider.GetConfig<ConfigModel>();
            var xmlEConfing = provider.GetConfig<EncryptionOptions>();



            /*ConfigProvider provider = new ConfigProvider("D:\\Projects\\cs\\labs\\lab3\\Config\\appsettings.json");
            var jsonConfig = provider.GetConfig<ConfigModel>();
            var jsonEConfig = provider.GetConfig<EncryptionOptions>();*/



        }
    }
}
