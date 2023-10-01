using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaffleApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedClanOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Clans",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Clans_OwnerId",
                table: "Clans",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clans_AspNetUsers_OwnerId",
                table: "Clans",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clans_AspNetUsers_OwnerId",
                table: "Clans");

            migrationBuilder.DropIndex(
                name: "IX_Clans_OwnerId",
                table: "Clans");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Clans");
        }
    }
}
