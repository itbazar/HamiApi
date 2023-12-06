using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Complaint",
                newName: "RegisteredAt");

            migrationBuilder.AddColumn<int>(
                name: "LastActor",
                table: "Complaint",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChanged",
                table: "Complaint",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastActor",
                table: "Complaint");

            migrationBuilder.DropColumn(
                name: "LastChanged",
                table: "Complaint");

            migrationBuilder.RenameColumn(
                name: "RegisteredAt",
                table: "Complaint",
                newName: "DateTime");
        }
    }
}
