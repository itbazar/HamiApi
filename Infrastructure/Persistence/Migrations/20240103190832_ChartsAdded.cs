using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChartsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChartId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ChartId",
                table: "AspNetRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Chart",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Parameters = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValidForMilliseconds = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chart", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ChartId",
                table: "AspNetUsers",
                column: "ChartId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_ChartId",
                table: "AspNetRoles",
                column: "ChartId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_Chart_ChartId",
                table: "AspNetRoles",
                column: "ChartId",
                principalTable: "Chart",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Chart_ChartId",
                table: "AspNetUsers",
                column: "ChartId",
                principalTable: "Chart",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_Chart_ChartId",
                table: "AspNetRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Chart_ChartId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Chart");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ChartId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_ChartId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "ChartId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ChartId",
                table: "AspNetRoles");
        }
    }
}
