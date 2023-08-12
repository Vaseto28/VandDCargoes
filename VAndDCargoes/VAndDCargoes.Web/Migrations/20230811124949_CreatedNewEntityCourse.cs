using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAndDCargoes.Web.Migrations
{
    public partial class CreatedNewEntityCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartureCity = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ArrivalCity = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Distance = table.Column<int>(type: "int", nullable: false),
                    Reward = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TruckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrailerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CargoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Cargoes_CargoId",
                        column: x => x.CargoId,
                        principalTable: "Cargoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_Trailers_TrailerId",
                        column: x => x.TrailerId,
                        principalTable: "Trailers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_Trucks_TruckId",
                        column: x => x.TruckId,
                        principalTable: "Trucks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CargoId",
                table: "Courses",
                column: "CargoId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DriverId",
                table: "Courses",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TrailerId",
                table: "Courses",
                column: "TrailerId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TruckId",
                table: "Courses",
                column: "TruckId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
