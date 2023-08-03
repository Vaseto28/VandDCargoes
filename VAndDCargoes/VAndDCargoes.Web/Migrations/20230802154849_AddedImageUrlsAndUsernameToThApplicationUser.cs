using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAndDCargoes.Web.Migrations
{
    public partial class AddedImageUrlsAndUsernameToThApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "AspNetUsers",
                newName: "Username");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Trucks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "https://cpmr-islands.org/wp-content/uploads/sites/4/2019/07/test.png");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Trailers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "https://cpmr-islands.org/wp-content/uploads/sites/4/2019/07/test.png");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Cargoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "https://cpmr-islands.org/wp-content/uploads/sites/4/2019/07/test.png");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Test123",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "UserName",
            //    table: "AspNetUsers",
            //    type: "nvarchar(256)",
            //    maxLength: 256,
            //    nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Trailers");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Cargoes");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "AspNetUsers",
                newName: "UserName");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Test123");
        }
    }
}
