using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StartmenuRecoveryWS.Migrations
{
    /// <inheritdoc />
    public partial class Shorcutmodified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shortcuts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LnkName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LnkPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shortcuts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shortcuts");
        }
    }
}
