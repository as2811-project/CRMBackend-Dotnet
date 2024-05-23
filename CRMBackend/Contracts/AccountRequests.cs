namespace CRMBackend.Contracts;

public class AccountRequests
{
    public class CreateAccount
    {
        public string company_name { get; set; }
        public int revenue { get; set; }
        public string industry { get; set; }
        public int employee_count { get; set; }
        public string founded_date { get; set; }
        public string phone_number { get; set; }
    }
    
    public class NewAccountResponse
    {
        public string company_name { get; set; }
        public int revenue { get; set; }
        public string phone_number { get; set; }
    }
}