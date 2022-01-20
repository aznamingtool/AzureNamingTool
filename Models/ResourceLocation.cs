using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AzNamingTool.Models
{
    public class ResourceLocation
    {
        public long Id { get; set; }
        [Required()]
        public string Name { get; set; }
        [Required()]
        public string ShortName { get; set; }
    }
}
