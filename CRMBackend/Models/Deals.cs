using Postgrest.Attributes;
using Postgrest.Models;

namespace CRMBackend.Models;
[Table("Deals")]
public class Deals : BaseModel
{
    [PrimaryKey("id",false)]
    public int deal_id { get; set; }
    
    [Column("value")]
    public float value { get; set; }
    
    [Column("deal_date")]
    public string deal_date { get; set; }
    
    [Column("productName")]
    public string product { get; set; }
    
    [Column("column")]
    public string deal_stage { get; set; }
    
    [Column("title")]
    public string deal_name { get; set; }
    
    [Column("customer_id")]
    public string related_contact { get; set; }
    
    [Column("Deal Owner")]
    public string deal_owner { get; set; }
}