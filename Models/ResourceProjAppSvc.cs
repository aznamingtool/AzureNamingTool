using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AzNamingTool.Models
{
    public class ResourceProjAppSvc
    {
        public long Id { get; set; }
        [Required()]
        public string Name { get; set; }
        [Required()]
        public string ShortName { get; set; }
        public int SortOrder { get; set; } = 0;
    }
}
