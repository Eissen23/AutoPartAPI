using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Categories;
using Domain.Entities.Common.Contracts;
using Domain.Entities.Customers;
using Domain.Entities.Invoices;
using Domain.Entities.Products;
using Domain.Entities.Warehouses;
using Infrastructure.Identities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    public DbSet<WarehouseLocation> WarehouseLocations => Set<WarehouseLocation>();
    public DbSet<PartLocation> PartLocations => Set<PartLocation>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.DeletedOn == null);

        base.OnModelCreating(modelBuilder);

        //This scans the assembly and applies all IEntityTypeConfiguration<T> implementations in Configuring
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

}
