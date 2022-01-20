using System.ComponentModel;

namespace AzNamingTool.Models
{
    public class ResponseMessage
    {
        public string Type { get; set; } = "bg-light";
        public string Header { get; set; } = "Message";
        public string Message { get; set; }
    }
}
