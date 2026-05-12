using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.Infrastructure.Persistence.Context;
using Base.Infrastructure.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Base.Infrastructure.Persistence.Seeding;

public class ApplicationDbContextSeeder(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    ILogger<ApplicationDbContextSeeder> logger)
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ILogger<ApplicationDbContextSeeder> _logger = logger;

    public async Task SeedAsync()
    {
        try
        {
            // Ensure database is created
            await _context.Database.MigrateAsync();

            _logger.LogInformation("Starting database seeding...");

            // Seed permissions
            await SeedPermissionsAsync();

            // Seed roles
            await SeedRolesAsync();

            // Seed admin user
            await SeedAdminUserAsync();

            _logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task SeedPermissionsAsync()
    {
        var permissionsToAdd = new List<Permission>
        {
            // Warehouse Management
            new() { Name = "WarehouseManagement", Description = "Manage and oversee warehouse operations (all CRUD)" },
            
            // Part Processing
            new() { Name = "PartProcessing", Description = "Manage part locations and processing (all CRUD)" },

            // User Management
            new() { Name = "UserManagement", Description = "Manage users (all CRUD)" },

            // Role Management
            new() { Name = "RoleManagement", Description = "Manage roles and permissions (all CRUD)" },

            // Product Management
            new() { Name = "ProductManagement", Description = "Manage products (all CRUD)" },

            // Invoice Management
            new() { Name = "InvoiceManagement", Description = "Manage invoices (all CRUD)" },

            // Customer Management
            new() { Name = "CustomerManagement", Description = "Manage customers (all CRUD)" },

            // System Administration
            new() { Name = "SystemAdministration", Description = "System administration and settings" },
        };

        var existingPermissions = await _context.Permissions.ToListAsync();
        var permissionsToInsert = permissionsToAdd
            .Where(p => !existingPermissions.Any(ep => ep.Name == p.Name))
            .ToList();

        if (permissionsToInsert.Any())
        {
            foreach (var permission in permissionsToInsert)
            {
                permission.CreatedBy = Guid.Empty;
                permission.LastModifiedBy = Guid.Empty;
            }

            await _context.Permissions.AddRangeAsync(permissionsToInsert);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Seeded {permissionsToInsert.Count} permissions.");
        }
        else
        {
            _logger.LogInformation("Permissions already exist in the database.");
        }
    }

    private async Task SeedRolesAsync()
    {
        var adminRoleName = "Admin";

        // Check if Admin role exists
        var adminRoleExists = await _roleManager.RoleExistsAsync(adminRoleName);
        if (!adminRoleExists)
        {
            var adminRole = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = adminRoleName,
                NormalizedName = adminRoleName.ToUpper(),
                Description = "System administrator with full access",
                AccessLevel = 1,
                IsSystemRole = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _roleManager.CreateAsync(adminRole);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Created '{adminRoleName}' role.");

                // Assign all permissions to Admin role
                await AssignPermissionsToRoleAsync(adminRole, null);
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError($"Failed to create '{adminRoleName}' role: {errors}");
            }
        }
        else
        {
            _logger.LogInformation($"Role '{adminRoleName}' already exists.");
        }

        // Seed Manager role
        await SeedRoleAsync("Manager", "Manager with access to most operations", 2, 
            new[] { "WarehouseManagement", "PartProcessing", "ProductManagement", "InvoiceManagement", "CustomerManagement" });

        // Seed Editor role
        await SeedRoleAsync("Editor", "Editor with read and write access to content", 3,
            new[] { "ProductManagement", "CustomerManagement" });

        // Seed Viewer role
        await SeedRoleAsync("Viewer", "Viewer with read-only access", 4,
            new[] { "WarehouseManagement", "PartProcessing", "ProductManagement", "InvoiceManagement", "CustomerManagement" });
    }

    private async Task SeedRoleAsync(string roleName, string description, int accessLevel, string[] permissionNames)
    {
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            var role = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                Description = description,
                AccessLevel = accessLevel,
                IsSystemRole = false,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Created '{roleName}' role.");

                // Assign specific permissions to this role
                await AssignPermissionsToRoleAsync(role, permissionNames);
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError($"Failed to create '{roleName}' role: {errors}");
            }
        }
        else
        {
            _logger.LogInformation($"Role '{roleName}' already exists.");
        }
    }

    private async Task AssignPermissionsToRoleAsync(ApplicationRole role, string[]? permissionNames = null)
    {
        var allPermissions = await _context.Permissions.ToListAsync();
        var existingRolePermissions = await _context.RolePermissions
            .Where(rp => rp.RoleId == role.Id)
            .ToListAsync();

        // If no specific permissions provided, assign all (for Admin role)
        var permissionsToAssign = permissionNames == null
            ? allPermissions.Where(p => !existingRolePermissions.Any(rp => rp.PermissionId == p.Id)).ToList()
            : allPermissions.Where(p => permissionNames.Contains(p.Name) && !existingRolePermissions.Any(rp => rp.PermissionId == p.Id)).ToList();

        if (permissionsToAssign.Any())
        {
            var rolePermissions = permissionsToAssign.Select(p => new RolePermission
            {
                RoleId = role.Id,
                PermissionId = p.Id,
                CreatedBy = Guid.Empty,
                LastModifiedBy = Guid.Empty
            }).ToList();

            await _context.RolePermissions.AddRangeAsync(rolePermissions);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Assigned {permissionsToAssign.Count} permissions to '{role.Name}' role.");
        }
    }

    private async Task SeedAdminUserAsync()
    {
        const string adminEmail = "admin@autopart.com";
        const string adminUsername = "admin";
        const string adminPassword = "Admin@123456";
        const string adminFirstName = "System";
        const string adminLastName = "Administrator";

        // Check if admin user exists
        var adminUserExists = await _userManager.FindByEmailAsync(adminEmail);
        if (adminUserExists == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminUsername,
                NormalizedUserName = adminUsername.ToUpper(),
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpper(),
                EmailConfirmed = true,
                FirstName = adminFirstName,
                LastName = adminLastName,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Created admin user '{adminEmail}'.");

                // Assign Admin role to the user
                var roleResult = await _userManager.AddToRoleAsync(adminUser, "Admin");
                if (roleResult.Succeeded)
                {
                    _logger.LogInformation($"Assigned 'Admin' role to user '{adminEmail}'.");
                }
                else
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    _logger.LogError($"Failed to assign 'Admin' role: {errors}");
                }
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError($"Failed to create admin user: {errors}");
            }
        }
        else
        {
            _logger.LogInformation($"Admin user '{adminEmail}' already exists.");
        }
    }
}
