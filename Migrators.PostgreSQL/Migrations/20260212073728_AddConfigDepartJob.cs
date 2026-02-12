using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations;

/// <inheritdoc />
public partial class AddConfigDepartJob : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Users_Departments_DepartmentId",
            table: "Users");

        migrationBuilder.DropForeignKey(
            name: "FK_Users_JobPositions_JobPositionId",
            table: "Users");

        migrationBuilder.DropForeignKey(
            name: "FK_Department_Department_ParentId1",
            table: "Department");

        migrationBuilder.DropPrimaryKey(
            name: "PK_JobPosition",
            table: "JobPosition");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Department",
            table: "Department");

        migrationBuilder.DropIndex(
            name: "IX_Department_ParentId1",
            table: "Department");

        migrationBuilder.DropColumn(
            name: "ParentId1",
            table: "Department");

        migrationBuilder.RenameTable(
            name: "JobPosition",
            newName: "JobPositions",
            newSchema: "public");

        migrationBuilder.RenameTable(
            name: "Department",
            newName: "Departments",
            newSchema: "public");

        migrationBuilder.AlterColumn<decimal>(
            name: "Salary",
            schema: "public",
            table: "JobPositions",
            type: "numeric(18,2)",
            precision: 18,
            scale: 2,
            nullable: true,
            oldClrType: typeof(decimal),
            oldType: "numeric",
            oldNullable: true);

        migrationBuilder.DropColumn(
            name: "DepartmentId",
            schema: "public",
            table: "JobPositions");

        migrationBuilder.AddColumn<Guid>(
            name: "DepartmentId",
            schema: "public",
            table: "JobPositions",
            type: "uuid",
            nullable: true);


        migrationBuilder.AlterColumn<string>(
            name: "AccessLevel",
            schema: "public",
            table: "JobPositions",
            type: "text",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer");


        migrationBuilder.DropColumn(
            name: "ParentId",
            schema: "public",
            table: "Departments");

        migrationBuilder.AddColumn<Guid>(
            name: "ParentId",
            schema: "public",
            table: "Departments",
            type: "uuid",
            nullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_JobPositions",
            schema: "public",
            table: "JobPositions",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Departments",
            schema: "public",
            table: "Departments",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_JobPositions_DepartmentId",
            schema: "public",
            table: "JobPositions",
            column: "DepartmentId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Departments_ParentId",
            schema: "public",
            table: "Departments",
            column: "ParentId",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_Departments_Departments_ParentId",
            schema: "public",
            table: "Departments",
            column: "ParentId",
            principalSchema: "public",
            principalTable: "Departments",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_JobPositions_Departments_DepartmentId",
            schema: "public",
            table: "JobPositions",
            column: "DepartmentId",
            principalSchema: "public",
            principalTable: "Departments",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Departments_DepartmentId",
            table: "Users",
            column: "DepartmentId",
            principalSchema: "public",
            principalTable: "Departments",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Users_JobPositions_JobPositionId",
            table: "Users",
            column: "JobPositionId",
            principalSchema: "public",
            principalTable: "JobPositions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Departments_Departments_ParentId",
            schema: "public",
            table: "Departments");

        migrationBuilder.DropForeignKey(
            name: "FK_JobPositions_Departments_DepartmentId",
            schema: "public",
            table: "JobPositions");

        migrationBuilder.DropPrimaryKey(
            name: "PK_JobPositions",
            schema: "public",
            table: "JobPositions");

        migrationBuilder.DropIndex(
            name: "IX_JobPositions_DepartmentId",
            schema: "public",
            table: "JobPositions");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Departments",
            schema: "public",
            table: "Departments");

        migrationBuilder.DropIndex(
            name: "IX_Departments_ParentId",
            schema: "public",
            table: "Departments");

        migrationBuilder.RenameTable(
            name: "JobPositions",
            schema: "public",
            newName: "JobPosition");

        migrationBuilder.RenameTable(
            name: "Departments",
            schema: "public",
            newName: "Department");

        migrationBuilder.AlterColumn<decimal>(
            name: "Salary",
            table: "JobPosition",
            type: "numeric",
            nullable: true,
            oldClrType: typeof(decimal),
            oldType: "numeric(18,2)",
            oldPrecision: 18,
            oldScale: 2,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "DepartmentId",
            table: "JobPosition",
            type: "text",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "AccessLevel",
            table: "JobPosition",
            type: "integer",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AlterColumn<string>(
            name: "ParentId",
            table: "Department",
            type: "text",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldNullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "ParentId1",
            table: "Department",
            type: "uuid",
            nullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_JobPosition",
            table: "JobPosition",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Department",
            table: "Department",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_Department_ParentId1",
            table: "Department",
            column: "ParentId1");

        migrationBuilder.AddForeignKey(
            name: "FK_Department_Department_ParentId1",
            table: "Department",
            column: "ParentId1",
            principalTable: "Department",
            principalColumn: "Id");
    }
}
