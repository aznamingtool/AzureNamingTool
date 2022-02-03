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

## TO USE

This project contains a .NET Core application, with Docker support. To use:

### Run as a Docker image

- Fork the **AzNamingTool** repository

- Select your forked repository

- On the **<>Code** tab, select the **<>Code** button and select **Download ZIP**

- Extract the zipped files to your loal machine

- Change directory to the project folder

  **NOTE**  
  - Ensure you can see the project files and are not in the parent folder)

- Open a **Command Prompt** and change directory to the current project folder

- Run the follwing **Docker command** to build the image

  *docker build -t aznamingtool .*
  
  **NOTE**  
  - Ensure the '.' is included in the command

- Run the following **Docker command** to create a new container

	*docker run -p 8081:80 aznamingtool*  
	
  **NOTES**  
    - Substitute 8081 for any used port on your machine  
    - You will see warnings in the command prompt regarding DataProtection and keys. These indicate that the keys are not persisted and are only local to the container instances. 

- Access the site using the following URL  

  *http://localhost:8081*
  
  **NOTE**  
  - Substitute 8081 for the port you used in the docker run command
