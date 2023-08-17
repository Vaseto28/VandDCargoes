using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAndDCargoes.Web.Migrations
{
    public partial class AddedCreatorColumnToTheRepairmentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MechanicId",
                table: "Repairments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Repairments_MechanicId",
                table: "Repairments",
                column: "MechanicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Repairments_AspNetUsers_MechanicId",
                table: "Repairments",
                column: "MechanicId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repairments_AspNetUsers_MechanicId",
                table: "Repairments");

            migrationBuilder.DropIndex(
                name: "IX_Repairments_MechanicId",
                table: "Repairments");

            migrationBuilder.DropColumn(
                name: "MechanicId",
                table: "Repairments");
        }
    }
}
