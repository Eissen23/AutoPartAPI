using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDepartmentJobposition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPositions_Departments_DepartmentId",
                schema: "public",
                table: "JobPositions");

            migrationBuilder.DropIndex(
                name: "IX_JobPositions_DepartmentId",
                schema: "public",
                table: "JobPositions");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                schema: "public",
                table: "JobPositions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                schema: "public",
                table: "JobPositions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobPositions_DepartmentId",
                schema: "public",
                table: "JobPositions",
                column: "DepartmentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JobPositions_Departments_DepartmentId",
                schema: "public",
                table: "JobPositions",
                column: "DepartmentId",
                principalSchema: "public",
                principalTable: "Departments",
                principalColumn: "Id");
        }
    }
}
