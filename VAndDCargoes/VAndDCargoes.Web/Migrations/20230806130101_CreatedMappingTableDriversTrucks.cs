using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAndDCargoes.Web.Migrations
{
    public partial class CreatedMappingTableDriversTrucks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trucks_Drivers_DriverId",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_Trucks_DriverId",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Trucks");

            migrationBuilder.CreateTable(
                name: "DriversTrucks",
                columns: table => new
                {
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TruckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriversTrucks", x => new { x.DriverId, x.TruckId });
                    table.ForeignKey(
                        name: "FK_DriversTrucks_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DriversTrucks_Trucks_TruckId",
                        column: x => x.TruckId,
                        principalTable: "Trucks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriversTrucks_TruckId",
                table: "DriversTrucks",
                column: "TruckId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriversTrucks");

            migrationBuilder.AddColumn<Guid>(
                name: "DriverId",
                table: "Trucks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_DriverId",
                table: "Trucks",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trucks_Drivers_DriverId",
                table: "Trucks",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
