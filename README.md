# Smart Inventory Management System

A comprehensive inventory management system built with ASP.NET Core MVC.

## Features

- User Authentication and Authorization
- Role-based Access Control
- Product Management
- Category Management
- Order Management
- Real-time Search with AJAX
- Email Notifications
- Responsive Design

## Prerequisites

- .NET 8.0 SDK
- PostgreSQL Database
- Azure Account (for deployment)

## Local Development

1. Clone the repository
2. Update the connection string in `appsettings.json`
3. Run the following commands:
   ```bash
   dotnet restore
   dotnet ef database update
   dotnet run
   ```

## Azure Deployment

1. Install Azure CLI
2. Login to Azure:
   ```bash
   az login
   ```
3. Update the following files with your Azure details:
   - `appsettings.Production.json`
   - `Properties/PublishProfiles/azure.pubxml`
4. Run the deployment script:
   ```bash
   ./deploy.ps1
   ```

## Configuration

### Database Connection
Update the connection string in `appsettings.json` or `appsettings.Production.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=your-db;User Id=your-username;Password=your-password;"
  }
}
```

### Email Settings
Configure email settings in `appsettings.json` or `appsettings.Production.json`:
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "your-email@gmail.com",
    "SmtpPassword": "your-app-password",
    "SenderEmail": "your-email@gmail.com",
    "SenderName": "Smart Inventory Management"
  }
}
```

## Testing

Run the unit tests:
```bash
dotnet test
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License. 