using AzNamingTool.Helpers;
using System.ComponentModel;

namespace AzNamingTool.Models
{
    public class ResponseMessage
    {
        public MessageTypesEnum Type { get; set; } = MessageTypesEnum.INFORMATION;
        public string Header { get; set; } = "Message";
        public string Message { get; set; }
    }
}
