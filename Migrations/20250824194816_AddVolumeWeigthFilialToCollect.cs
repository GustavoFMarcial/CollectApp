using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollectApp.Migrations
{
    /// <inheritdoc />
    public partial class AddVolumeWeigthFilialToCollect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Company",
                table: "Collects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Filial",
                table: "Collects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Volume",
                table: "Collects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Weigth",
                table: "Collects",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Filial",
                table: "Collects");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Collects");

            migrationBuilder.DropColumn(
                name: "Weigth",
                table: "Collects");

            migrationBuilder.AlterColumn<string>(
                name: "Company",
                table: "Collects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
