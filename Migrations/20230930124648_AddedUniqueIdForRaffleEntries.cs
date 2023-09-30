using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaffleApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniqueIdForRaffleEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Entries",
                table: "Entries");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Entries",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entries",
                table: "Entries",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_RaffleId",
                table: "Entries",
                column: "RaffleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Entries",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_RaffleId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Entries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entries",
                table: "Entries",
                columns: new[] { "RaffleId", "EntrantId" });
        }
    }
}
