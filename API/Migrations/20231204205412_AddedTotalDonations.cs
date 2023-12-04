using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaffleApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedTotalDonations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalDonations",
                table: "Entrants",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalDonations",
                table: "Entrants");
        }
    }
}
