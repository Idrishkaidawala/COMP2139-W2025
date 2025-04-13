# Azure Deployment Script for Smart Inventory Management System
# This script deploys the application to Azure App Service

# Configuration
$resourceGroupName = "SmartInventoryManagement"
$location = "eastus"
$appServicePlanName = "SmartInventoryManagementPlan"
$appServiceName = "smartinventorymanagement"
$databaseServerName = "smartinventorydb"
$databaseName = "SmartInventoryDB"
$adminUsername = "smartinventoryadmin"
$adminPassword = "P@ssw0rd123!" # Change this to a secure password

# Login to Azure
Write-Host "Logging in to Azure..."
az login

# Create Resource Group
Write-Host "Creating Resource Group..."
az group create --name $resourceGroupName --location $location

# Create App Service Plan
Write-Host "Creating App Service Plan..."
az appservice plan create --name $appServicePlanName --resource-group $resourceGroupName --location $location --sku B1

# Create Web App
Write-Host "Creating Web App..."
az webapp create --name $appServiceName --resource-group $resourceGroupName --plan $appServicePlanName --runtime "DOTNETCORE:8.0"

# Create PostgreSQL Server
Write-Host "Creating PostgreSQL Server..."
az postgres flexible-server create --name $databaseServerName --resource-group $resourceGroupName --location $location --admin-user $adminUsername --admin-password $adminPassword --sku-name Standard_B1ms --tier Burstable --storage-size 32

# Create Database
Write-Host "Creating Database..."
az postgres flexible-server db create --name $databaseName --server-name $databaseServerName --resource-group $resourceGroupName

# Configure Connection String
Write-Host "Configuring Connection String..."
$connectionString = "Host=$databaseServerName.postgres.database.azure.com;Database=$databaseName;Username=$adminUsername;Password=$adminPassword;SSL Mode=Require;Trust Server Certificate=true"
az webapp config appsettings set --name $appServiceName --resource-group $resourceGroupName --settings "ConnectionStrings__DefaultConnection=$connectionString"

# Configure App Service
Write-Host "Configuring App Service..."
az webapp config set --name $appServiceName --resource-group $resourceGroupName --startup-file "dotnet SmartInventoryManagement.dll"

# Deploy Application
Write-Host "Deploying Application..."
az webapp deployment source config-zip --name $appServiceName --resource-group $resourceGroupName --src publish.zip

Write-Host "Deployment completed successfully!"
Write-Host "Your application is available at: https://$appServiceName.azurewebsites.net" 