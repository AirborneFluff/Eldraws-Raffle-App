using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaffleApi.Migrations
{
    /// <inheritdoc />
    public partial class FixedForeignKeyContrainted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HostId",
                table: "Raffles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Raffles_HostId",
                table: "Raffles",
                column: "HostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Raffles_AspNetUsers_HostId",
                table: "Raffles",
                column: "HostId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Raffles_AspNetUsers_HostId",
                table: "Raffles");

            migrationBuilder.DropIndex(
                name: "IX_Raffles_HostId",
                table: "Raffles");

            migrationBuilder.DropColumn(
                name: "HostId",
                table: "Raffles");
        }
    }
}
