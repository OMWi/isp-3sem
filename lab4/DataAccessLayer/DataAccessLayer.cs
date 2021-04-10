namespace DataAccessLayer
{
    public interface IReadablePerson
    {
        void SetName(string connectionString, int businessId);
        void SetPhoneNumber(string connectionString, int businessId);      
    }

    public interface IReadableLogin
    {
        void SetEmail(string connectionString, int businessId);
        void SetPassword(string connectionString, int businessId);    
    }  
    
    public interface IReadableAddress
    {
        void SetAddress(string connectionString, int addressId);
    }
}