using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRecurrenceAndNextOccurrenceToTestPeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TestInstance",
                table: "TestPeriodResult",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextOccurrence",
                table: "TestPeriod",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Recurrence",
                table: "TestPeriod",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TestPeriod",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "NextOccurrence", "Recurrence" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TestPeriod",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "NextOccurrence", "Recurrence" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestInstance",
                table: "TestPeriodResult");

            migrationBuilder.DropColumn(
                name: "NextOccurrence",
                table: "TestPeriod");

            migrationBuilder.DropColumn(
                name: "Recurrence",
                table: "TestPeriod");
        }
    }
}
