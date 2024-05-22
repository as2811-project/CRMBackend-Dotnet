namespace CRMBackend.Contracts;

public class CreateContactRequest
{
    public string first_name { get; set; }
    
    public string last_name { get; set; }
    
    public string email { get; set; }
    
    public long phone_number { get; set; }
    
    public string address { get; set; }
}

public class NewContactResponse
{
    public int contact_id { get; set; }
    public string first_name { get; set; }  
    public string last_name { get; set; }
    public string email { get; set; }
}