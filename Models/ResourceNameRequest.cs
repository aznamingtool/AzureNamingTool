using System.ComponentModel.DataAnnotations;

namespace AzNamingTool.Models
{
    public class ResourceNameRequest
    {
        public string ResourceDelimiter { get; set; }
        public string ResourceEnvironment { get; set; }
        public string ResourceInstance { get; set; }
        public string ResourceLocation { get; set; }
        public string ResourceOrg { get; set; }
        public string ResourceProjAppSvc { get; set; }
        [Required()]
        public int ResourceType { get; set; }
        public string ResourceUnitDept { get; set; }
        public string ResourceVmRole { get; set; }
    }
}
