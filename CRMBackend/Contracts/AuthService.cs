namespace CRMBackend.Contracts;

public class AuthService
{
    public class CreateUser
    {
        public string email { get; set; }
        public string password { get; set; }
        public string username { get; set; }
    
    }

    public class Login
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}