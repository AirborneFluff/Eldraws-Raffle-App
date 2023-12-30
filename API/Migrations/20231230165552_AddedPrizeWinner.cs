using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaffleApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedPrizeWinner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WinnerId",
                table: "Prizes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prizes_WinnerId",
                table: "Prizes",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prizes_Entrants_WinnerId",
                table: "Prizes",
                column: "WinnerId",
                principalTable: "Entrants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prizes_Entrants_WinnerId",
                table: "Prizes");

            migrationBuilder.DropIndex(
                name: "IX_Prizes_WinnerId",
                table: "Prizes");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "Prizes");
        }
    }
}
