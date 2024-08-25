using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaffleApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexingForGamertag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Entrants_Active_NormalizedGamertag",
                table: "Entrants",
                columns: new[] { "Active", "NormalizedGamertag" });

            migrationBuilder.CreateIndex(
                name: "IX_Entrants_Active_TotalDonations",
                table: "Entrants",
                columns: new[] { "Active", "TotalDonations" });

            migrationBuilder.CreateIndex(
                name: "IX_Entrants_ClanId",
                table: "Entrants",
                column: "ClanId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrants_NormalizedGamertag",
                table: "Entrants",
                column: "NormalizedGamertag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Entrants_Active_NormalizedGamertag",
                table: "Entrants");

            migrationBuilder.DropIndex(
                name: "IX_Entrants_Active_TotalDonations",
                table: "Entrants");

            migrationBuilder.DropIndex(
                name: "IX_Entrants_ClanId",
                table: "Entrants");

            migrationBuilder.DropIndex(
                name: "IX_Entrants_NormalizedGamertag",
                table: "Entrants");
        }
    }
}
