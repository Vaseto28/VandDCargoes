using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAndDCargoes.Web.Migrations
{
    public partial class UpdatedTruckTrailersAndCargoesTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trailers_Cargoes_CargoId",
                table: "Trailers");

            migrationBuilder.DropForeignKey(
                name: "FK_Trucks_Trailers_TraillerId",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_Trucks_TraillerId",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_Trailers_CargoId",
                table: "Trailers");

            migrationBuilder.DropColumn(
                name: "TraillerId",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "CargoId",
                table: "Trailers");

            //migrationBuilder.DropColumn(
            //    name: "UserName",
            //    table: "AspNetUsers");

            //migrationBuilder.RenameColumn(
            //    name: "Username",
            //    table: "AspNetUsers",
            //    newName: "UserName");

            //migrationBuilder.AlterColumn<string>(
            //    name: "UserName",
            //    table: "AspNetUsers",
            //    type: "nvarchar(256)",
            //    maxLength: 256,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(20)",
            //    oldMaxLength: 20,
            //    oldDefaultValue: "Test123");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "UserName",
            //    table: "AspNetUsers",
            //    newName: "Username");

            migrationBuilder.AddColumn<Guid>(
                name: "TraillerId",
                table: "Trucks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CargoId",
                table: "Trailers",
                type: "uniqueidentifier",
                nullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Username",
            //    table: "AspNetUsers",
            //    type: "nvarchar(20)",
            //    maxLength: 20,
            //    nullable: false,
            //    defaultValue: "Test123",
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(256)",
            //    oldMaxLength: 256,
            //    oldNullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "UserName",
            //    table: "AspNetUsers",
            //    type: "nvarchar(256)",
            //    maxLength: 256,
            //    nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_TraillerId",
                table: "Trucks",
                column: "TraillerId");

            migrationBuilder.CreateIndex(
                name: "IX_Trailers_CargoId",
                table: "Trailers",
                column: "CargoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trailers_Cargoes_CargoId",
                table: "Trailers",
                column: "CargoId",
                principalTable: "Cargoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trucks_Trailers_TraillerId",
                table: "Trucks",
                column: "TraillerId",
                principalTable: "Trailers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
