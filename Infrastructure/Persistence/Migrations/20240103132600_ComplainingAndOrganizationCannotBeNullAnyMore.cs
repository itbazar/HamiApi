using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ComplainingAndOrganizationCannotBeNullAnyMore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaint_ComplaintOrganization_ComplaintOrganizationId",
                table: "Complaint");

            migrationBuilder.AlterColumn<Guid>(
                name: "ComplaintOrganizationId",
                table: "Complaint",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Complaining",
                table: "Complaint",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaint_ComplaintOrganization_ComplaintOrganizationId",
                table: "Complaint",
                column: "ComplaintOrganizationId",
                principalTable: "ComplaintOrganization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaint_ComplaintOrganization_ComplaintOrganizationId",
                table: "Complaint");

            migrationBuilder.AlterColumn<Guid>(
                name: "ComplaintOrganizationId",
                table: "Complaint",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Complaining",
                table: "Complaint",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaint_ComplaintOrganization_ComplaintOrganizationId",
                table: "Complaint",
                column: "ComplaintOrganizationId",
                principalTable: "ComplaintOrganization",
                principalColumn: "Id");
        }
    }
}
