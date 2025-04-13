# Azure Deployment Guide for Smart Inventory Management System

This guide provides step-by-step instructions for deploying the Smart Inventory Management System to Azure.

## Prerequisites

1. **Azure Account**: You need an Azure account. If you don't have one, sign up for a free trial at [azure.microsoft.com](https://azure.microsoft.com/en-us/free/).

2. **Azure CLI**: Install the Azure CLI to manage Azure resources from the command line. Download it from [here](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli).

3. **PowerShell**: The deployment scripts use PowerShell. Make sure you have PowerShell 5.1 or later installed.

## Deployment Steps

### 1. Configure Deployment Settings

Before deploying, you may want to customize the following settings in the `deploy-azure.ps1` script:

- `$resourceGroupName`: The name of the Azure resource group
- `$location`: The Azure region where resources will be deployed
- `$appServiceName`: The name of your Azure Web App (must be globally unique)
- `$databaseServerName`: The name of your PostgreSQL server
- `$adminUsername` and `$adminPassword`: Database credentials

### 2. Prepare the Application

Run the following command to prepare the application for deployment:

```powershell
.\prepare-deployment.ps1
```

This script will:
- Clean the previous build
- Restore packages
- Build the application in Release mode
- Publish the application
- Create a deployment package (publish.zip)

### 3. Deploy to Azure

Run the following command to deploy the application to Azure:

```powershell
.\deploy-azure.ps1
```

This script will:
- Log in to Azure
- Create a resource group
- Create an App Service plan
- Create a Web App
- Create a PostgreSQL server
- Create a database
- Configure the connection string
- Deploy the application

### 4. Access Your Application

After successful deployment, your application will be available at:

```
https://<appServiceName>.azurewebsites.net
```

Replace `<appServiceName>` with the value of `$appServiceName` from the deployment script.

## Troubleshooting

### Common Issues

1. **Deployment Fails**: Check the Azure portal for detailed error messages. Common issues include:
   - Insufficient permissions
   - Resource name conflicts
   - Invalid connection string

2. **Application Not Starting**: Check the application logs in the Azure portal:
   - Go to your Web App
   - Click on "Log stream" under "Monitoring"
   - Check for any error messages

3. **Database Connection Issues**: Verify that:
   - The connection string is correctly configured
   - The database server is running
   - The firewall rules allow connections from your application

### Getting Help

If you encounter any issues during deployment, you can:

1. Check the Azure documentation at [docs.microsoft.com/azure](https://docs.microsoft.com/en-us/azure/)
2. Search for solutions on [Stack Overflow](https://stackoverflow.com/)
3. Contact Azure support through the Azure portal

## Cleanup

To avoid unnecessary charges, you can delete the resource group when you're done:

```powershell
az group delete --name $resourceGroupName --yes
```

This will delete all resources created during deployment. 