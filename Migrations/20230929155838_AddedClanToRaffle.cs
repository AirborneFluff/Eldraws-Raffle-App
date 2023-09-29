using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaffleApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedClanToRaffle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClanId",
                table: "Raffles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Raffles_ClanId",
                table: "Raffles",
                column: "ClanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Raffles_Clans_ClanId",
                table: "Raffles",
                column: "ClanId",
                principalTable: "Clans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Raffles_Clans_ClanId",
                table: "Raffles");

            migrationBuilder.DropIndex(
                name: "IX_Raffles_ClanId",
                table: "Raffles");

            migrationBuilder.DropColumn(
                name: "ClanId",
                table: "Raffles");
        }
    }
}
