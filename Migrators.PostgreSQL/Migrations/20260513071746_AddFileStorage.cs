using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddFileStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileStorageData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FileExtension = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TargetTable = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TargetId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RelatedTable = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RelatedId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FileType = table.Column<int>(type: "integer", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    FileVisibility = table.Column<int>(type: "integer", nullable: false),
                    AllowedRoles = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    FileSize = table.Column<float>(type: "real", nullable: false),
                    StorageKey = table.Column<string>(type: "text", nullable: true),
                    CheckSum = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileStorageData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileStorageData_IsPublic",
                table: "FileStorageData",
                column: "IsPublic");

            migrationBuilder.CreateIndex(
                name: "IX_FileStorageData_TargetId",
                table: "FileStorageData",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_FileStorageData_TargetTable",
                table: "FileStorageData",
                column: "TargetTable");

            migrationBuilder.CreateIndex(
                name: "IX_FileStorageData_TargetTable_TargetId",
                table: "FileStorageData",
                columns: new[] { "TargetTable", "TargetId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileStorageData");
        }
    }
}
