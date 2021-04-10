using System;
using System.Data.SqlClient;
using Models;
using ServiceLayer;
using CP_dll;
using LoggerLayer;


namespace lab4
{
    class GetData
    {
        static void Main()
        {
            var configPath = @"D:\Projects\cs\lab4\ServiceLayer\ServiceConfig.json";
            var configProvider = new ConfigProvider(configPath);
            var serviceModel = configProvider.GetConfig<ServiceModel>();
            var connectionString = serviceModel.ConnectionString;
            var targetPath = serviceModel.TargetXmlPath;
            var errorLogger = new Logger();
            int id = 5;
            try
            {
                PersonalInfo personalInfo = new PersonalInfo();////////
                personalInfo.Name = "testName";
                personalInfo.PhoneNumber = "testPhone";
                LoginInfo loginInfo = new LoginInfo(connectionString, id);
                AddressInfo addressInfo = new AddressInfo();///////
                addressInfo.Address = "testAddress";
                XmlModel obj = new XmlModel(personalInfo, loginInfo, addressInfo);
                XmlCreator creator = new XmlCreator();
                creator.CreateXmlFile(obj, targetPath);
            }
            catch(Exception e)
            {
                errorLogger.AddError(e, DateTime.Now);
            }            
        }
    }
}