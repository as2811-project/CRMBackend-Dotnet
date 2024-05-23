namespace CRMBackend.Contracts;

public class DealRequests
{
    public class CreateDealRequest
    {
        public int deal_id { get; set; }
        public string deal_name { get; set; }
    
        public float value { get; set; }
    
        public string product { get; set; }
    
        public string deal_stage { get; set; }
    
    }

    public class NewDealResponse
    {
        public int deal_id { get; set; }
        public string deal_name { get; set; }  
        public float value { get; set; }
        public string deal_stage { get; set; }
    }
}