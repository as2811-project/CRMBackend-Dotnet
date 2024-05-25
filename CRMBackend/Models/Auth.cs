using Postgrest.Attributes;
using Postgrest.Models;

namespace CRMBackend.Models;

public class Auth : BaseModel
{
    [PrimaryKey("email")]
    public string email { get; set; }
    [Column("password")]
    public string password { get; set; }
    [Column("Display Name")]
    public string username { get; set; }
}