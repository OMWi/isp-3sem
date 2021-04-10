namespace ServiceLayer
{
    public class ServiceModel
    {
        public string ConnectionString { get; set; }
        public string TargetXmlPath { get; set; }
        public ServiceModel(string connectionString, string targetXmlPath)
        {
            ConnectionString = connectionString;
            TargetXmlPath = targetXmlPath;
        }
        public ServiceModel() { }
    }
}