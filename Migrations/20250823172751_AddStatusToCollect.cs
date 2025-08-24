using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollectApp.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToCollect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Collects",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Collects");
        }
    }
}
