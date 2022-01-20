using AzNamingTool.Helpers;
using AzNamingTool.Models;

namespace AzNamingTool.Services
{
    public class ImportExportService
    {
        private static ServiceResponse serviceResponse = new();

        public static async Task<ServiceResponse> ExportConfig()
        {
            try
            {
                ConfigurationData configdata = new();
                // Get the current data
                //ResourceComponents
                var resourceComponents = await GeneralHelper.GetList<ResourceComponent>();
                configdata.ResourceComponents = resourceComponents.OrderBy(y => y.SortOrder).ToList();

                //ResourceEnvironments
                var resourceEnvironments = await GeneralHelper.GetList<ResourceEnvironment>();
                configdata.ResourceEnvironments = resourceEnvironments.OrderBy(y => y.SortOrder).ToList();

                // ResourceLocations
                var resourceLocations = await GeneralHelper.GetList<ResourceLocation>();
                configdata.ResourceLocations = resourceLocations.ToList();

                // ResourceOrgs
                var resourceOrgs = await GeneralHelper.GetList<ResourceOrg>();
                configdata.ResourceOrgs = resourceOrgs.OrderBy(y => y.SortOrder).ToList();

                // ResourceProjAppSvc
                var resourceProjAppSvcs = await GeneralHelper.GetList<ResourceProjAppSvc>();
                configdata.ResourceProjAppSvcs = resourceProjAppSvcs.OrderBy(y => y.SortOrder).ToList();

                // ResourceTypes
                var resourceTypes = await GeneralHelper.GetList<ResourceType>();
                configdata.ResourceTypes = resourceTypes.ToList();

                // ResourceUnitDepts
                var resourceUnitDepts = await GeneralHelper.GetList<ResourceUnitDept>();
                configdata.ResourceUnitDepts = resourceUnitDepts.OrderBy(y => y.SortOrder).ToList();

                // ResourceVmRoles
                var resourceVmRoles = await GeneralHelper.GetList<ResourceVmRole>();
                configdata.ResourceVmRoles = resourceVmRoles.OrderBy(y => y.SortOrder).ToList();


                serviceResponse.ResponseObject = configdata;
                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.ResponseObject = ex;
            }
            return serviceResponse;
        }

        public static async Task<ServiceResponse> PostConfig(ConfigurationData configdata)
        {
            try
            {
                // Write all the configurations
                await GeneralHelper.WriteList<ResourceComponent>(configdata.ResourceComponents);
                await GeneralHelper.WriteList<ResourceEnvironment>(configdata.ResourceEnvironments);
                await GeneralHelper.WriteList<ResourceLocation>(configdata.ResourceLocations);
                await GeneralHelper.WriteList<ResourceOrg>(configdata.ResourceOrgs);
                await GeneralHelper.WriteList<ResourceProjAppSvc>(configdata.ResourceProjAppSvcs);
                await GeneralHelper.WriteList<ResourceType>(configdata.ResourceTypes);
                await GeneralHelper.WriteList<ResourceUnitDept>(configdata.ResourceUnitDepts);
                await GeneralHelper.WriteList<ResourceVmRole>(configdata.ResourceVmRoles);

                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.ResponseObject = ex;
            }
            return serviceResponse;
        }

    }
}
