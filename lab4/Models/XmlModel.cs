namespace Models
{
    public class XmlModel
    {
        public PersonalInfo PersonalInfo { get; set; }
        public LoginInfo LoginInfo { get; set; }
        public AddressInfo AddressInfo { get; set; }
        public XmlModel(PersonalInfo personalInfo, LoginInfo loginInfo, AddressInfo addressInfo)
        {
            PersonalInfo = personalInfo;
            LoginInfo = loginInfo;
            AddressInfo = addressInfo;
        }
        public XmlModel() { }
    }
}