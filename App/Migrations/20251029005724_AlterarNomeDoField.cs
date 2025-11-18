using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollectApp.Migrations
{
    /// <inheritdoc />
    public partial class AlterarNomeDoField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Products",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "Description");
        }
    }
}
