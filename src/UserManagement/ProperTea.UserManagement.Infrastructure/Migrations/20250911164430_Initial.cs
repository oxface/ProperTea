using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProperTea.UserManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email_Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FullName_Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserIdentity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIdentity", x => new { x.UserId, x.Id });
                    table.ForeignKey(
                        name: "FK_UserIdentity_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Name",
                table: "Users",
                column: "Email_Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Name1",
                table: "Users",
                column: "FullName_Value",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserIdentity");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
