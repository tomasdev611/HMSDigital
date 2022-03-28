namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class QueryRequest
    {
        public int? Limit { get; set; }
       
        public int? Offset { get; set; }
        
        public string Query { get; set; }
    }
}
