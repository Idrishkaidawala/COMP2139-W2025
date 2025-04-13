# Azure deployment script
param(
    [string]$resourceGroupName = "smart-inventory-rg",
    [string]$webAppName = "smart-inventory-webapp",
    [string]$location = "eastus",
    [string]$sku = "B1"
)

# Login to Azure
Write-Host "Logging in to Azure..."
az login

# Create resource group if it doesn't exist
Write-Host "Creating resource group..."
az group create --name $resourceGroupName --location $location

# Create App Service plan
Write-Host "Creating App Service plan..."
az appservice plan create --name "$webAppName-plan" --resource-group $resourceGroupName --sku $sku --is-linux

# Create web app
Write-Host "Creating web app..."
az webapp create --name $webAppName --resource-group $resourceGroupName --plan "$webAppName-plan" --runtime "DOTNETCORE:8.0"

# Configure web app
Write-Host "Configuring web app..."
az webapp config set --name $webAppName --resource-group $resourceGroupName --linux-fx-version "DOTNETCORE|8.0"

# Enable logging
Write-Host "Enabling logging..."
az webapp log config --name $webAppName --resource-group $resourceGroupName --application-logging filesystem --detailed-error-messages true --failed-request-tracing true --web-server-logging filesystem

# Deploy the application
Write-Host "Deploying application..."
az webapp deployment source config-zip --name $webAppName --resource-group $resourceGroupName --src publish.zip

Write-Host "Deployment completed successfully!"
Write-Host "Web app URL: https://$webAppName.azurewebsites.net" 