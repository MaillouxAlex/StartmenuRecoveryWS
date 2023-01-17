using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StartmenuRecoveryWS.Migrations
{
    /// <inheritdoc />
    public partial class Shortcutrestored : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Restored",
                table: "Shortcuts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9ef639b9-2157-4f08-a07d-349b0f399999",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "62c89b98-c412-4a93-9b35-820c0b4ab89c", "AQAAAAIAAYagAAAAEK6ePShJnkUmI59YWbzX1l3Ezk+DdndVzxIDuzPmmlHlvF693RrbkTxPTVlYzNfeww==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Restored",
                table: "Shortcuts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9ef639b9-2157-4f08-a07d-349b0f399999",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2ce22a43-fcb8-4454-a4d5-013f9f59d343", "AQAAAAIAAYagAAAAEOXVWwQJdl+ILKvWEP8Ng0nC/kku2ZyJTdFBsnjnIrH11obRdzlm3pILN/6UPHN2Nw==" });
        }
    }
}
