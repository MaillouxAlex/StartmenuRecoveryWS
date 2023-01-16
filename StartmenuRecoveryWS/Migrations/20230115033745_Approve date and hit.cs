using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StartmenuRecoveryWS.Migrations
{
    /// <inheritdoc />
    public partial class Approvedateandhit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedTimestamp",
                table: "Shortcuts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Hit",
                table: "Shortcuts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedTimestamp",
                table: "Shortcuts");

            migrationBuilder.DropColumn(
                name: "Hit",
                table: "Shortcuts");
        }
    }
}
