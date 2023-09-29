using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaffleApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedClansToEntrant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClanId",
                table: "Entrants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Entrants_ClanId",
                table: "Entrants",
                column: "ClanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entrants_Clans_ClanId",
                table: "Entrants",
                column: "ClanId",
                principalTable: "Clans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entrants_Clans_ClanId",
                table: "Entrants");

            migrationBuilder.DropIndex(
                name: "IX_Entrants_ClanId",
                table: "Entrants");

            migrationBuilder.DropColumn(
                name: "ClanId",
                table: "Entrants");
        }
    }
}
