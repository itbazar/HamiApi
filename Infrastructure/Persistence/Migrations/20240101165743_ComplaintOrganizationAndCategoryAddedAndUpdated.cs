using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ComplaintOrganizationAndCategoryAddedAndUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ComplaintCategory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Complaining",
                table: "Complaint",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ComplaintOrganizationId",
                table: "Complaint",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ComplaintOrganization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintOrganization", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Complaint_ComplaintOrganizationId",
                table: "Complaint",
                column: "ComplaintOrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaint_ComplaintOrganization_ComplaintOrganizationId",
                table: "Complaint",
                column: "ComplaintOrganizationId",
                principalTable: "ComplaintOrganization",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaint_ComplaintOrganization_ComplaintOrganizationId",
                table: "Complaint");

            migrationBuilder.DropTable(
                name: "ComplaintOrganization");

            migrationBuilder.DropIndex(
                name: "IX_Complaint_ComplaintOrganizationId",
                table: "Complaint");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ComplaintCategory");

            migrationBuilder.DropColumn(
                name: "Complaining",
                table: "Complaint");

            migrationBuilder.DropColumn(
                name: "ComplaintOrganizationId",
                table: "Complaint");
        }
    }
}
