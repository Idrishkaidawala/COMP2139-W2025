# Prepare Application for Azure Deployment
# This script builds and publishes the application for deployment

# Configuration
$configuration = "Release"
$outputPath = ".\publish"

# Clean previous build
Write-Host "Cleaning previous build..."
dotnet clean

# Restore packages
Write-Host "Restoring packages..."
dotnet restore

# Build the application
Write-Host "Building the application..."
dotnet build -c $configuration

# Publish the application
Write-Host "Publishing the application..."
dotnet publish -c $configuration -o $outputPath

# Create deployment package
Write-Host "Creating deployment package..."
Compress-Archive -Path "$outputPath\*" -DestinationPath "publish.zip" -Force

Write-Host "Deployment package created successfully!"
Write-Host "You can now run deploy-azure.ps1 to deploy to Azure." 