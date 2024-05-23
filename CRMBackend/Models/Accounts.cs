using Postgrest.Attributes;
using Postgrest.Models;

namespace CRMBackend.Models;

public class Accounts : BaseModel
{
    [PrimaryKey("account_id",false)]
    public string account_id { get; set; }
    [Column("company_name")]
    public string company_name { get; set; }
    [Column("revenue")]
    public int revenue { get; set; }
    [Column("industry")]
    public string industry { get; set; }
    [Column("employee_count")]
    public int employee_count { get; set; }
    [Column("founded_date")]
    public string founded_date { get; set; }
    [Column("phone_number")]
    public string phone_number { get; set; }
}