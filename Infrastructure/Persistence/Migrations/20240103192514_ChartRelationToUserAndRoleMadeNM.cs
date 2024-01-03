using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChartRelationToUserAndRoleMadeNM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_Chart_ChartId",
                table: "AspNetRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Chart_ChartId",
                table: "AspNetUsers");

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

            migrationBuilder.CreateTable(
                name: "ApplicationRoleChart",
                columns: table => new
                {
                    ChartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RolesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRoleChart", x => new { x.ChartId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_ApplicationRoleChart_AspNetRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationRoleChart_Chart_ChartId",
                        column: x => x.ChartId,
                        principalTable: "Chart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserChart",
                columns: table => new
                {
                    ChartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserChart", x => new { x.ChartId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserChart_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserChart_Chart_ChartId",
                        column: x => x.ChartId,
                        principalTable: "Chart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoleChart_RolesId",
                table: "ApplicationRoleChart",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserChart_UsersId",
                table: "ApplicationUserChart",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationRoleChart");

            migrationBuilder.DropTable(
                name: "ApplicationUserChart");

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
    }
}
