using AzNamingTool.Models;
using System.Collections.Generic;

namespace AzNamingTool.Models
{
    public class ConfigurationData
    {
        public List<ResourceComponent> ResourceComponents { get; set; }
        public List<ResourceEnvironment> ResourceEnvironments { get; set; }
        public List<ResourceLocation> ResourceLocations { get; set; }
        public List<ResourceOrg> ResourceOrgs { get; set; }
        public List<ResourceProjAppSvc> ResourceProjAppSvcs { get; set; }
        public List<ResourceType> ResourceTypes { get; set; }
        public List<ResourceUnitDept> ResourceUnitDepts { get; set; }
        public List<ResourceVmRole> ResourceVmRoles { get; set; }
        public string SALTKey { get; set; }
        public string AdminPassword { get; set; }
        public string APIKey { get; set; }
    }
}
