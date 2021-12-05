namespace Warehouse.ClientApp.Handlers.Web.Utils
{
    public class WebRequestResult
    {
        public WebRequestResultStatus Status { get; set; }
        public object Content { get; set; }

        public WebRequestResult(WebRequestResultStatus status)
        {
            Status = status;
        }

        public WebRequestResult(WebRequestResultStatus status, object content)
        {
            Status = status;
            Content = content;
        }
    }
}
