namespace PhotoManager.WebApp.Models
{
    public class ErrorVM
    { 
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }
    }
}