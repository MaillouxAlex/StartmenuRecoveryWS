using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StartmenuRecoveryWS.Migrations
{
    /// <inheritdoc />
    public partial class AppArgsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppArgs",
                table: "Shortcuts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppArgs",
                table: "Shortcuts");
        }
    }
}
