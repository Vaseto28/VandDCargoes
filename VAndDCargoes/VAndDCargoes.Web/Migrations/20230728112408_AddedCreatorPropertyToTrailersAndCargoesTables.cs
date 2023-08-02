using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAndDCargoes.Web.Migrations
{
    public partial class AddedCreatorPropertyToTrailersAndCargoesTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Trailers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("3d7a05d7-8255-4936-9f32-36a07dc4af55"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Cargoes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("3d7a05d7-8255-4936-9f32-36a07dc4af55"));

            migrationBuilder.CreateIndex(
                name: "IX_Trailers_CreatorId",
                table: "Trailers",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cargoes_CreatorId",
                table: "Cargoes",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cargoes_AspNetUsers_CreatorId",
                table: "Cargoes",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trailers_AspNetUsers_CreatorId",
                table: "Trailers",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cargoes_AspNetUsers_CreatorId",
                table: "Cargoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Trailers_AspNetUsers_CreatorId",
                table: "Trailers");

            migrationBuilder.DropIndex(
                name: "IX_Trailers_CreatorId",
                table: "Trailers");

            migrationBuilder.DropIndex(
                name: "IX_Cargoes_CreatorId",
                table: "Cargoes");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Trailers");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Cargoes");
        }
    }
}
