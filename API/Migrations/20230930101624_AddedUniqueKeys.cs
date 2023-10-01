using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaffleApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniqueKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Entrants_ClanId",
                table: "Entrants");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedGamertag",
                table: "Entrants",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Clans",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Entrants_ClanId_NormalizedGamertag",
                table: "Entrants",
                columns: new[] { "ClanId", "NormalizedGamertag" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clans_NormalizedName",
                table: "Clans",
                column: "NormalizedName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Entrants_ClanId_NormalizedGamertag",
                table: "Entrants");

            migrationBuilder.DropIndex(
                name: "IX_Clans_NormalizedName",
                table: "Clans");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Clans");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedGamertag",
                table: "Entrants",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Entrants_ClanId",
                table: "Entrants",
                column: "ClanId");
        }
    }
}
