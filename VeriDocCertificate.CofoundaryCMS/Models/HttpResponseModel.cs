namespace VeriDocCertificate.CofoundaryCMS.Models
{
    public class HttpResponseModel
    {
        public int StatusCode { get; set; }
        public object Content { get; set; }
        public bool IsSuccess { get; set; }
    }
}
