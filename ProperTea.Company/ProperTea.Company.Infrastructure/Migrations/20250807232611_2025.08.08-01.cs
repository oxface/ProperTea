using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProperTea.Company.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _2025080801 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Companies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "SystemOwnerId",
                table: "Companies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Company_Name",
                table: "Companies",
                columns: new[] { "Name", "SystemOwnerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_SystemOwnerId",
                table: "Companies",
                column: "SystemOwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Company_Name",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Company_SystemOwnerId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "SystemOwnerId",
                table: "Companies");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }
    }
}
