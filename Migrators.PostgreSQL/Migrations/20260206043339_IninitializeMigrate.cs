using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations;

/// <inheritdoc />
public partial class IninitializeMigrate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "public");

        migrationBuilder.CreateTable(
            name: "Categories",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Description = table.Column<string>(type: "text", nullable: true),
                Type = table.Column<string>(type: "text", nullable: true),
                DefaultMarkupPercentage = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Categories", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Customers",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                PhoneNumber = table.Column<string>(type: "text", nullable: true),
                Email = table.Column<string>(type: "text", nullable: true),
                CustomerType = table.Column<string>(type: "text", nullable: false),
                CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Customers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "WarehouseLocations",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ZoneCode = table.Column<string>(type: "text", nullable: false),
                Aisle = table.Column<int>(type: "integer", nullable: false),
                Shelf = table.Column<int>(type: "integer", nullable: false),
                Bin = table.Column<string>(type: "text", nullable: true),
                IsOverstocked = table.Column<bool>(type: "boolean", nullable: false),
                CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WarehouseLocations", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Products",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                PartNumber = table.Column<string>(type: "text", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Description = table.Column<string>(type: "text", nullable: true),
                UnitCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                RetailPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Products", x => x.Id);
                table.ForeignKey(
                    name: "FK_Products_Categories_CategoryId",
                    column: x => x.CategoryId,
                    principalSchema: "public",
                    principalTable: "Categories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Invoices",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                SaleDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                TaxAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                TotalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Invoices", x => x.Id);
                table.ForeignKey(
                    name: "FK_Invoices_Customers_CustomerId",
                    column: x => x.CustomerId,
                    principalSchema: "public",
                    principalTable: "Customers",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "PartLocations",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                PartId = table.Column<Guid>(type: "uuid", nullable: false),
                WarehouseLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                QuantityAtLocation = table.Column<int>(type: "integer", nullable: false),
                CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PartLocations", x => x.Id);
                table.ForeignKey(
                    name: "FK_PartLocations_Products_PartId",
                    column: x => x.PartId,
                    principalSchema: "public",
                    principalTable: "Products",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PartLocations_WarehouseLocations_WarehouseLocationId",
                    column: x => x.WarehouseLocationId,
                    principalSchema: "public",
                    principalTable: "WarehouseLocations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "InvoiceItems",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                InvoiceId = table.Column<Guid>(type: "uuid", nullable: false),
                ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                Quantity = table.Column<int>(type: "integer", nullable: false),
                UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_InvoiceItems", x => x.Id);
                table.ForeignKey(
                    name: "FK_InvoiceItems_Invoices_InvoiceId",
                    column: x => x.InvoiceId,
                    principalSchema: "public",
                    principalTable: "Invoices",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_InvoiceItems_Products_ProductId",
                    column: x => x.ProductId,
                    principalSchema: "public",
                    principalTable: "Products",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Category_Code",
            schema: "public",
            table: "Categories",
            column: "CategoryCode",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_InvoiceItems_InvoiceId",
            schema: "public",
            table: "InvoiceItems",
            column: "InvoiceId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_InvoiceItems_ProductId",
            schema: "public",
            table: "InvoiceItems",
            column: "ProductId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Invoices_CustomerId",
            schema: "public",
            table: "Invoices",
            column: "CustomerId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PartLocations_PartId",
            schema: "public",
            table: "PartLocations",
            column: "PartId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PartLocations_WarehouseLocationId",
            schema: "public",
            table: "PartLocations",
            column: "WarehouseLocationId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_WarehouseLocation_Product",
            schema: "public",
            table: "PartLocations",
            columns: new[] { "PartId", "WarehouseLocationId" });

        migrationBuilder.CreateIndex(
            name: "IX_Product_Number",
            schema: "public",
            table: "Products",
            column: "PartNumber",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Products_CategoryId",
            schema: "public",
            table: "Products",
            column: "CategoryId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Zonecode",
            schema: "public",
            table: "WarehouseLocations",
            column: "ZoneCode",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "InvoiceItems",
            schema: "public");

        migrationBuilder.DropTable(
            name: "PartLocations",
            schema: "public");

        migrationBuilder.DropTable(
            name: "Invoices",
            schema: "public");

        migrationBuilder.DropTable(
            name: "Products",
            schema: "public");

        migrationBuilder.DropTable(
            name: "WarehouseLocations",
            schema: "public");

        migrationBuilder.DropTable(
            name: "Customers",
            schema: "public");

        migrationBuilder.DropTable(
            name: "Categories",
            schema: "public");
    }
}
