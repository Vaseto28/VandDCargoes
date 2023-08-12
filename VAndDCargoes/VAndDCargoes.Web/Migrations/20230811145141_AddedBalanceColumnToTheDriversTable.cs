using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAndDCargoes.Web.Migrations
{
    public partial class AddedBalanceColumnToTheDriversTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Ballance",
                table: "Drivers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ballance",
                table: "Drivers");
        }
    }
}
