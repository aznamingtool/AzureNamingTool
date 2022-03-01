# AzNamingTool

[Overview](#overview)

[Pages](#pages)

[How To Install](#how-to-install)  
* [Run as a Docker image](#run-as-a-docker-image)  
* [Run as an Azure App Service Container](#run-as-an-azure-app-service-container)


## OVERVIEW

This Naming Tool was developed using a naming pattern based on [Microsoft's best practices ](https://docs.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/naming-and-tagging). Once the organizational components have been defined, users can use the tool to geernate aname for the desred Azure resource.

Further documentation on the script can be found on [Microsoft's Cloud Adoption Framework GitHub repo](https://github.com/microsoft/CloudAdoptionFramework/tree/master/ready/AzNamingTool).

## PAGES

## Home Page
The Home Page provides an overview of the tool and the components.

![Home Page](https://github.com/BryanSoltis/AzNamingTool/blob/master/Screenshots/HomePage.png)

## Configuration
The Configuration Page shows the current Name Generation configuration. This page also provides an Admin section for updating the configuration. 

![Configuration Page](https://github.com/BryanSoltis/AzNamingTool/blob/master/Screenshots/ConfigurationPage.png)

## Reference
The References Page provides examples for each type of Azure resource. The example values do not include any excluded naming components. Optional components are always displayed and are identified below the example . Since unique names are only required at specific scopes, the examples provided are only generated for the scopes above the resource scope: resource group, resource group & region, region, global, subscription, and tenant.

![Reference Page](https://github.com/BryanSoltis/AzNamingTool/blob/master/Screenshots/ReferencePage.png)

## Generate
The Generator Page provides a drop down menu to select an Azure resource. Once a resource is selected, naming component options are provided. Read-only components cannot be changed, like the value for a resource type or organization. Optional components, if left blank, will be null and not shown in the output. Required components do not allow a null value, and the first value in the array is set as the default.

![Generate Page](https://github.com/BryanSoltis/AzNamingTool/blob/master/Screenshots/GeneratePage.png)

## HOW TO INSTALL

This project contains a .NET Core application, with Docker support. To use, complete the following:

**NOTE**

The Azure Naming Tool requires persistent storage for the configuration files when run as a container. The following processes will explain how to create this volume in your respective envinroment.  

### Run as a Docker image

Ths process will allow you to deploy the Azure Naming Tool using DOcker to your local environment.

- On the **<>Code** tab, select the **<>Code** button and select **Download ZIP**

- Extract the zipped files to your local machine

- Change directory to the project folder

  **NOTE**  
  - Ensure you can see the project files and are not in the parent folder

- Open a **Command Prompt** and change directory to the current project folder

- Run the follwing **Docker command** to build the image

  *docker build -t aznamingtool .*
  
  **NOTE**  
  - Ensure the '.' is included in the command

- Run the following ##Docker command** to create a new container and mount a new volume
	
	*docker run -d -p 8081:80 --mount source=aznamingtoolvol,target=/app/settings aznamingtool:latest*

  **NOTES**  
    - Substitute 8081 for any port not in use on your machine  
    - You will see warnings in the command prompt regarding DataProtection and keys. These indicate that the keys are not persisted and are only local to the container instances. 

- Access the site using the following URL  

  *http://localhost:8081*
  
  **NOTE**  
  - Substitute 8081 for the port you used in the docker run command

***

### Run as an Azure App Service Container

Ths process will allow you to deploy the Azure Naming Tool to an Azure App Service. 

**NOTE**

For many the steps, a sample proces is provideed, however, there are many ways to accomplsih eash step. 

- On the **<>Code** tab, select the **<>Code** button and select **Download ZIP**

- Extract the zipped files to your local machine

- Change directory to the project folder

  **NOTE**  
  - Ensure you can see the project files and are not in the parent folder

- Open a **Command Prompt** and change directory to the current project folder

- Run the follwing **Docker command** to build the image

  *docker build -t aznamingtool .*
  
  **NOTE**  
  - Ensure the '.' is included in the command
  
- Create an Azure Container Registry  
  [Create an Azure container registry using the Azure portal](https://docs.microsoft.com/en-us/azure/container-registry/container-registry-get-started-portal#:~:text=%20Quickstart%3A%20Create%20an%20Azure%20container%20registry%20using,must%20log%20in%20to%20the%20registry...%20More%20)

- Build and publish you image to the Azure Container Registry  
  [Push your first image to your Azure container registry using the Docker CLI](https://docs.microsoft.com/en-us/azure/container-registry/container-registry-get-started-docker-cli?tabs=azure-cli)

- Create an Azure App Service - Web App  
  [Run a custom container in Azure](https://docs.microsoft.com/en-us/azure/app-service/quickstart-custom-container?tabs=dotnet&pivots=container-linux) 

- Create an Azure Storage Fileshare for the persistent storage  
  [Create an Azure file share](https://docs.microsoft.com/en-us/azure/storage/files/storage-how-to-create-file-share?tabs=azure-portal)
  
  ![FileShare](https://github.com/BryanSoltis/AzNamingTool/blob/master/Screenshots/FileShare.png)

- Mount the fileshare as local storage for the Azure App Service  
  [Mount Azure Storage as a local share in a custom container in App Service](https://docs.microsoft.com/en-us/azure/app-service/configure-connect-to-azure-storage?tabs=portal&pivots=container-linux)
  
  ![MountStorage](https://github.com/BryanSoltis/AzNamingTool/blob/master/Screenshots/MountStorage.png)

- Deploy the image from the Azure Container Registry to the Azure App Service  
  [Continuous deployment with custom containers in Azure App Service](https://docs.microsoft.com/en-us/azure/app-service/deploy-ci-cd-custom-container?tabs=acr&pivots=container-linux)

- Access the site using your Azure App Service URL

***

- On first launch, you will be prompted to set the Admin password

![Admin Password Prompt](https://github.com/BryanSoltis/AzNamingTool/blob/master/Screenshots/AdminPasswordPrompt.png)
