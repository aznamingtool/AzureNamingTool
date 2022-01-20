# AzNamingTool

## Disclaimer

This Naming Tool was developed using a naming pattern based on [Microsoft's best practices ](https://docs.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/naming-and-tagging), and uses a PowerShell script to define your organization's preferred naming configuration. Once the organizational components have been defined, CSV files are used to further define the values for Azure naming components. These values are imported and hard coded into a JavaScript object for reference documentation.

Further documentation on the script can be found on [Microsoft's Cloud Adoption Framework GitHub repo](https://github.com/microsoft/CloudAdoptionFramework/tree/master/ready/AzNamingTool).

## Configuration

The Configuration page shows the current Name Generation configuration. This page also provides an Admin section for updating the configuration. 

## References

The References tab provides examples for each type of Azure resource. The example values do not include any excluded naming components. Optional components are always displayed and are identified below the example . Since unique names are only required at specific scopes, the examples provided are only generated for the scopes above the resource scope: resource group, resource group & region, region, global, subscription, and tenant.

## Generator

The Generator tab provides a drop down menu to select an Azure resource. Once a resource is selected, naming component options are provided. Read-only components cannot be changed, like the value for a resource type or organization. Optional components, if left blank, will be null and not shown in the output. Required components do not allow a null value, and the first value in the array is set as the default.
