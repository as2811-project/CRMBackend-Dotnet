using Postgrest.Attributes;
using Postgrest.Models;

namespace CRMBackend.Models;
[Table("Contacts")]
public class Contacts : BaseModel
{
    [PrimaryKey("contact_id",true)]
    public int contact_id { get; set; }
    
    [Column("first_name")]
    public string first_name { get; set; }
    
    [Column("last_name")]
    public string last_name { get; set; }
    
    [Column("email")]
    public string email { get; set; }
    
    [Column("phone_number")]
    public string phone_number { get; set; }
    
    [Column("address")]
    public string address { get; set; }
    
    [Column("Related Account")]
    public string related_account { get; set; }
    
    [Column("Agent")]
    public string Agent { get; set; }
}