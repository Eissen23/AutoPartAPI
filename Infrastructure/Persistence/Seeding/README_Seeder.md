# Database Seeder Documentation

## Overview
The `ApplicationDbContextSeeder` provides automatic seeding of initial database data including permissions, roles, and an admin user.

## What Gets Seeded

### 1. **Permissions**
The seeder creates the following broad permission categories:

- **WarehouseManagement** (Create, Read, Update, Delete)
- **PartProcessing** (Create, Read, Update, Delete)
- **UserManagement** (Create, Read, Update, Delete)
- **RoleManagement** (Create, Read, Update, Delete)
- **ProductManagement** (Create, Read, Update, Delete)
- **InvoiceManagement** (Create, Read, Update, Delete)
- **CustomerManagement** (Create, Read, Update, Delete)
- **SystemAdministration** (AuditLog, SystemSettings)

### 2. **Admin Role**
Creates a system role with the following properties:
- **Name**: Admin
- **Description**: System administrator with full access
- **AccessLevel**: 1 (highest priority)
- **IsSystemRole**: true
- **Permissions**: All permissions in the system

### 3. **Admin User**
Creates a default administrator account:
- **Username**: admin
- **Email**: admin@autopart.com
- **Password**: Admin@123456
- **FirstName**: System
- **LastName**: Administrator
- **Role**: Admin

## Setup Instructions

### Step 1: Register the Seeder in Program.cs

```csharp
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// ... other configurations ...

builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddDatabaseSeeder();  // Add this line

var app = builder.Build();

// ... middleware setup ...

// Seed the database
await app.SeedDatabaseAsync();  // Add this line before app.Run()

app.Run();
```

### Step 2: Ensure Infrastructure Service Registration

Make sure your `Infrastructure/ServiceCollectionExtensions.cs` or similar includes:

```csharp
public static IServiceCollection AddInfrastructure(this IServiceCollection services)
{
    // ... other registrations ...
    services.AddDatabaseSeeder();
    return services;
}
```

Alternatively, add it directly in Program.cs.

## Usage

The seeding happens automatically when the application starts. The seeder performs the following:

1. **Runs pending migrations** - Ensures the database schema is up-to-date
2. **Checks for existing data** - Doesn't create duplicates
3. **Seeds permissions** - Creates all permission records
4. **Seeds admin role** - Creates the Admin role with all permissions
5. **Seeds admin user** - Creates the admin account

## Logging

The seeder logs all operations using the standard ILogger interface:

```
Starting database seeding...
Seeded XX permissions.
Created 'Admin' role.
Assigned XX permissions to 'Admin' role.
Created admin user 'admin@autopart.com'.
Assigned 'Admin' role to user 'admin@autopart.com'.
Database seeding completed successfully.
```

## Security Notes

⚠️ **Important**: Change the default admin credentials in production:

1. Modify the password in `ApplicationDbContextSeeder.SeedAdminUserAsync()`:
   ```csharp
   const string adminPassword = "YourSecurePassword123!";
   ```

2. Or change it manually after first login via the API

3. Consider using environment variables for sensitive data:
   ```csharp
   var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") 
       ?? "Admin@123456";
   ```

## Idempotency

The seeder is **idempotent** - running it multiple times is safe:
- Existing permissions won't be duplicated
- Existing roles won't be recreated
- Existing users won't be duplicated (checked by email)
- Only missing permissions are added to existing roles

## Customization

### Adding New Permissions

Edit `SeedPermissionsAsync()` to add new permission categories:

```csharp
new() { Id = Guid.NewGuid(), Name = "YourModule.Create", Description = "Create your resources" },
new() { Id = Guid.NewGuid(), Name = "YourModule.Read", Description = "View your resources" },
```

### Adding Additional Roles

Create a new method `SeedOtherRolesAsync()`:

```csharp
private async Task SeedOtherRolesAsync()
{
    var roles = new[] { "Manager", "Editor", "Viewer" };
    
    foreach (var roleName in roles)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            var role = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                Description = $"{roleName} role"
            };
            
            await _roleManager.CreateAsync(role);
        }
    }
}
```

Then call it in `SeedAsync()`.

## Troubleshooting

### Seeding not running?
- Ensure `await app.SeedDatabaseAsync();` is called before `app.Run()`
- Check that `AddDatabaseSeeder()` is registered
- Check the logs for errors

### Duplicate permissions?
- Permissions are checked by name, so duplicates should not occur
- If issues persist, manually clean the database and reseed

### User creation failed?
- Check that UserManager is properly configured
- Verify password meets complexity requirements
- Check logs for specific error messages
