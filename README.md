# AutoPart Manager API

A Clean Architecture-based ASP.NET Core Web API project for managing auto parts, built with .NET 10.0.

## 🏗️ Project Structure

This solution follows **Clean Architecture** principles with 6 projects:

```
├── global.json
├── Shared/                      (Base library - shared contracts)
├── Domain/                      (Domain entities & business logic)
├── Application/                 (Application services & MediatR)
├── Infrastructure/              (Data access, logging, external services)
├── Migrators.PostgreSQL/        (EF Core migrations for PostgreSQL)
└── Host/                        (Web API entry point)
    ├── Configurations/
    │   ├── database.json        (⚠️ Not in source control)
    │   ├── database.example.json
    │   ├── logger.json
    │   └── logger.staging.json
    ├── appsettings.json
    └── appsettings.Development.json
```

### Project Dependencies

```
Host → Application, Infrastructure, Migrators.PostgreSQL, Shared
Application → (no internal dependencies)
Infrastructure → Domain, Shared
Domain → Shared
Migrators.PostgreSQL → Infrastructure
Shared → (no dependencies)
```

## 📋 Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- PostgreSQL, MySQL, or SQL Server (depending on your configuration)
- Optional: Elasticsearch for logging
- Optional: Seq for structured logging

## 🚀 Quick Start

### 1. Clone the Repository

```bash
git clone <your-repository-url>
cd AutoPart
```

### 2. Configure Database Connection

Copy the example database configuration and update it with your connection details:

```bash
cp Host/Configurations/database.example.json Host/Configurations/database.json
```

Edit `Host/Configurations/database.json` with your database credentials:

```json
{
  "DatabaseSettings": {
    "DBProvider": "postgresql",
    "ConnectionString": "Host=localhost;Port=5432;Database=autopart;Username=youruser;Password=yourpassword;",
    "SchemaName": "public"
  }
}
```

Supported `DBProvider` values:
- `postgresql` - PostgreSQL database
- `mysql` - MySQL/MariaDB database
- `sqlserver` - Microsoft SQL Server

### 3. Restore Dependencies

```bash
dotnet restore
```

### 4. Run Database Migrations

```bash
dotnet ef database update --project Migrators.PostgreSQL --startup-project Host
```

### 5. Run the Application

```bash
dotnet run --project Host
```

The API will be available at:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`

## 📦 How to Recreate This Project from Scratch

### Step 1: Create Solution Structure

```bash
# Create solution directory
mkdir AutoPart
cd AutoPart

# Create global.json
cat > global.json << 'EOF'
{
  "sdk": {
    "version": "10.0.0",
    "rollForward": "latestFeature",
    "allowPrerelease": false
  }
}
EOF
```

### Step 2: Create Projects

```bash
# Create projects in dependency order
dotnet new classlib -n Shared -f net10.0
dotnet new classlib -n Domain -f net10.0
dotnet new classlib -n Application -f net10.0
dotnet new classlib -n Infrastructure -f net10.0
dotnet new classlib -n Migrators.PostgreSQL -f net10.0
dotnet new webapi -n Host -f net10.0

# Create solution file
dotnet new sln -n AutoPart

# Add all projects to solution
dotnet sln add Shared/Shared.csproj
dotnet sln add Domain/Domain.csproj
dotnet sln add Application/Application.csproj
dotnet sln add Infrastructure/Infrastructure.csproj
dotnet sln add Migrators.PostgreSQL/Migrators.PostgreSQL.csproj
dotnet sln add Host/Host.csproj
```

### Step 3: Add Project References

```bash
# Domain references
dotnet add Domain/Domain.csproj reference Shared/Shared.csproj

# Infrastructure references
dotnet add Infrastructure/Infrastructure.csproj reference Shared/Shared.csproj
dotnet add Infrastructure/Infrastructure.csproj reference Domain/Domain.csproj

# Migrators.PostgreSQL references
dotnet add Migrators.PostgreSQL/Migrators.PostgreSQL.csproj reference Infrastructure/Infrastructure.csproj

# Host references
dotnet add Host/Host.csproj reference Shared/Shared.csproj
dotnet add Host/Host.csproj reference Application/Application.csproj
dotnet add Host/Host.csproj reference Infrastructure/Infrastructure.csproj
dotnet add Host/Host.csproj reference Migrators.PostgreSQL/Migrators.PostgreSQL.csproj
```

### Step 4: Install NuGet Packages

#### Domain Project
```bash
dotnet add Domain/Domain.csproj package NewId -v 4.0.1
```

#### Application Project
```bash
dotnet add Application/Application.csproj package FluentValidation.DependencyInjectionExtensions -v 11.5.2
dotnet add Application/Application.csproj package MediatR.Extensions.Microsoft.DependencyInjection -v 11.1.0
```

#### Infrastructure Project
```bash
dotnet add Infrastructure/Infrastructure.csproj package Azure.Identity -v 1.17.1
dotnet add Infrastructure/Infrastructure.csproj package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore -v 9.0.10
dotnet add Infrastructure/Infrastructure.csproj package Microsoft.EntityFrameworkCore -v 9.0.12
dotnet add Infrastructure/Infrastructure.csproj package Microsoft.EntityFrameworkCore.InMemory -v 9.0.10
dotnet add Infrastructure/Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools -v 9.0.2
dotnet add Infrastructure/Infrastructure.csproj package Microsoft.Data.SqlClient -v 5.1.5
dotnet add Infrastructure/Infrastructure.csproj package Microsoft.Identity.Client -v 4.82.0
dotnet add Infrastructure/Infrastructure.csproj package Microsoft.IdentityModel.JsonWebTokens -v 8.15.0
dotnet add Infrastructure/Infrastructure.csproj package Npgsql.EntityFrameworkCore.PostgreSQL -v 9.0.4
dotnet add Infrastructure/Infrastructure.csproj package Pomelo.EntityFrameworkCore.MySql -v 9.0.0
dotnet add Infrastructure/Infrastructure.csproj package System.IdentityModel.Tokens.Jwt -v 8.15.0
dotnet add Infrastructure/Infrastructure.csproj package Figgle -v 0.5.1
dotnet add Infrastructure/Infrastructure.csproj package Serilog.AspNetCore -v 10.0.0
dotnet add Infrastructure/Infrastructure.csproj package Serilog.Enrichers.Process -v 2.0.2
dotnet add Infrastructure/Infrastructure.csproj package Serilog.Enrichers.Thread -v 3.1.0
dotnet add Infrastructure/Infrastructure.csproj package Serilog.Expressions -v 3.4.1
dotnet add Infrastructure/Infrastructure.csproj package Serilog.Enrichers.Environment -v 2.2.0
dotnet add Infrastructure/Infrastructure.csproj package Serilog.Extensions.Hosting -v 10.0.0
dotnet add Infrastructure/Infrastructure.csproj package Serilog.Formatting.Compact -v 3.0.0
dotnet add Infrastructure/Infrastructure.csproj package Serilog.Settings.Configuration -v 10.0.0
dotnet add Infrastructure/Infrastructure.csproj package Serilog.Sinks.Async -v 1.5.0
dotnet add Infrastructure/Infrastructure.csproj package Serilog.Sinks.Console -v 6.1.1
dotnet add Infrastructure/Infrastructure.csproj package Serilog.Sinks.File -v 7.0.0
dotnet add Infrastructure/Infrastructure.csproj package Serilog.Sinks.MSSqlServer -v 6.3.0
dotnet add Infrastructure/Infrastructure.csproj package Serilog.Sinks.Elasticsearch -v 9.0.0
dotnet add Infrastructure/Infrastructure.csproj package Serilog.Sinks.Seq -v 5.2.2
dotnet add Infrastructure/Infrastructure.csproj package Serilog.Exceptions -v 8.4.0
```

#### Migrators.PostgreSQL Project
```bash
dotnet add Migrators.PostgreSQL/Migrators.PostgreSQL.csproj package Microsoft.EntityFrameworkCore.Tools -v 9.0.12
dotnet add Migrators.PostgreSQL/Migrators.PostgreSQL.csproj package Microsoft.EntityFrameworkCore.Design -v 9.0.12
dotnet add Migrators.PostgreSQL/Migrators.PostgreSQL.csproj package Npgsql.EntityFrameworkCore.PostgreSQL -v 9.0.4
```

#### Host Project
```bash
dotnet add Host/Host.csproj package Microsoft.EntityFrameworkCore.Design -v 9.0.12
dotnet add Host/Host.csproj package Azure.Identity -v 1.17.1
dotnet add Host/Host.csproj package Microsoft.AspNetCore.OpenApi -v 9.0.10
dotnet add Host/Host.csproj package Microsoft.Identity.Client -v 4.82.0
dotnet add Host/Host.csproj package Microsoft.IdentityModel.JsonWebTokens -v 8.15.0
dotnet add Host/Host.csproj package Serilog.AspNetCore -v 10.0.0
dotnet add Host/Host.csproj package System.IdentityModel.Tokens.Jwt -v 8.15.0
```

### Step 5: Create Configuration Directory

```bash
mkdir Host/Configurations
```

## ⚙️ Configuration Files

### Required Configuration Files

#### 1. `global.json` (Root directory)

```json
{
  "sdk": {
    "version": "10.0.0",
    "rollForward": "latestFeature",
    "allowPrerelease": false
  }
}
```

#### 2. `Host/appsettings.json`

```json
{
  "AllowedHosts": "*"
}
```

#### 3. `Host/appsettings.Development.json` (Optional)

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

#### 4. `Host/Configurations/database.json` ⚠️ (REQUIRED - Not in source control)

```json
{
  "DatabaseSettings": {
    "DBProvider": "postgresql",
    "ConnectionString": "Host=localhost;Port=5432;Database=autopart;Username=youruser;Password=yourpassword;",
    "SchemaName": "public"
  }
}
```

**Connection String Examples:**

**PostgreSQL:**
```
Host=localhost;Port=5432;Database=autopart;Username=postgres;Password=yourpassword;
```

**MySQL:**
```
Server=localhost;Port=3306;Database=autopart;Uid=root;Pwd=yourpassword;CharSet=utf8mb4;
```

**SQL Server:**
```
Server=localhost;Database=autopart;User Id=sa;Password=yourpassword;TrustServerCertificate=True;
```

#### 5. `Host/Configurations/database.example.json` (Template for developers)

```json
{
  "DatabaseSettings": {
    "DBProvider": "postgresql",
    "ConnectionString": "Host=localhost;Port=5432;Database=autopart;Username=youruser;Password=yourpassword;",
    "SchemaName": "public"
  }
}
```

#### 6. `Host/Configurations/logger.json` (REQUIRED)

```json
{
  "LoggerSettings": {
    "AppName": "AutoPart",
    "ElasticSearchUrl": "http://localhost:9200",
    "WriteToFile": true,
    "StructuredConsoleLogging": false
  }
}
```

#### 7. `Host/Configurations/logger.staging.json` (Optional - Staging environment)

```json
{
  "LoggerSettings": {
    "AppName": "AutoPart-Staging",
    "ElasticSearchUrl": "http://staging-elasticsearch:9200",
    "WriteToFile": true,
    "StructuredConsoleLogging": true
  }
}
```

### Configuration Loading Order

The application loads configuration files in the following order (later files override earlier ones):

1. `appsettings.json`
2. `appsettings.{Environment}.json`
3. `Configurations/logger.json`
4. `Configurations/logger.{Environment}.json`
5. `Configurations/database.json`
6. `Configurations/database.{Environment}.json`
7. Environment variables

## 🔒 Security & .gitignore

**Important:** The `database.json` file contains sensitive connection strings and should **NEVER** be committed to source control.

Add the following to your `.gitignore`:

```gitignore
# Database configuration (contains secrets)
**/Configurations/database.json
**/Configurations/database.*.json
!**/Configurations/database.example.json

# Logs
**/Logs/
*.log

# Build results
[Dd]ebug/
[Rr]elease/
x64/
x86/
[Aa]rm/
[Aa]rm64/
bld/
[Bb]in/
[Oo]bj/

# User-specific files
*.suo
*.user
*.userosscache
*.sln.docstates
```

## 🏛️ Architecture Overview

### Shared Layer
- Contains common contracts and interfaces used across all layers
- Example: `IEvent` interface

### Domain Layer
- Contains business entities, value objects, and domain events
- No dependencies on other layers (except Shared)
- Uses `NewId` for generating distributed IDs
- Entities:
  - Customer
  - Category
  - Product
  - Invoice & InvoiceItem
  - WarehouseLocation & PartLocation

### Application Layer
- Contains application business logic and use cases
- Uses MediatR for CQRS pattern implementation
- Uses FluentValidation for input validation
- No dependencies on Infrastructure

### Infrastructure Layer
- Implements interfaces defined in Application layer
- Contains:
  - EF Core DbContext and configurations
  - Logging setup with Serilog
  - External service integrations
- Supports multiple database providers: PostgreSQL, MySQL, SQL Server

### Migrators.PostgreSQL Layer
- Contains EF Core migrations specific to PostgreSQL
- Separate from Infrastructure to isolate migration code

### Host Layer
- ASP.NET Core Web API entry point
- Configures dependency injection
- Sets up middleware pipeline
- Configures Serilog logging

## 🗃️ Database Migrations

### Create a New Migration

```bash
dotnet ef migrations add YourMigrationName --project Migrators.PostgreSQL --startup-project Host
```

### Apply Migrations

```bash
dotnet ef database update --project Migrators.PostgreSQL --startup-project Host
```

### Remove Last Migration

```bash
dotnet ef migrations remove --project Migrators.PostgreSQL --startup-project Host
```

### Generate SQL Script

```bash
dotnet ef migrations script --project Migrators.PostgreSQL --startup-project Host --output migration.sql
```

## 🔧 Development

### Build the Solution

```bash
dotnet build
```

### Run Tests

```bash
dotnet test
```

### Run in Development Mode

```bash
dotnet run --project Host --environment Development
```

### Watch Mode (Hot Reload)

```bash
dotnet watch --project Host
```

## 📝 Logging

The application uses Serilog with multiple sinks:

- **Console** - For development
- **File** - Rotating daily log files in `Host/Logs/`
- **Elasticsearch** - For centralized logging (optional)
- **Seq** - For structured log viewing (optional)
- **MSSqlServer** - For database logging (optional)

Configure logging in `Host/Configurations/logger.json`.

## 🌍 Environment Variables

You can override any configuration using environment variables:

```bash
# Linux/macOS
export DatabaseSettings__ConnectionString="Host=localhost;Database=autopart;..."
export LoggerSettings__AppName="AutoPart-Dev"

# Windows PowerShell
$env:DatabaseSettings__ConnectionString="Host=localhost;Database=autopart;..."
$env:LoggerSettings__AppName="AutoPart-Dev"

# Windows Command Prompt
set DatabaseSettings__ConnectionString=Host=localhost;Database=autopart;...
set LoggerSettings__AppName=AutoPart-Dev
```

## 🚢 Deployment

### Docker (Coming Soon)

```bash
docker build -t autopart-api .
docker run -p 8080:80 autopart-api
```

### Azure App Service

1. Ensure all configuration is set via Azure Configuration or Key Vault
2. Deploy using:
   ```bash
   dotnet publish -c Release -o ./publish
   ```
3. Upload to Azure App Service

## 🤝 Contributing

1. Create a feature branch
2. Make your changes
3. Ensure all tests pass
4. Submit a pull request

## 📄 License

[Your License Here]

## 📧 Contact

[Your Contact Information]

---

**Note:** This project uses .NET 10.0, which may be a preview version. Ensure you have the correct SDK installed.
