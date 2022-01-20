using System.ComponentModel.DataAnnotations;

namespace AzNamingTool.Models
{
    public class ResourceInstance
    {
        public long Id { get; set; }
        [Required()]
        public string Name { get; set; }
        public int SortOrder { get; set; } = 0;
    }
}
