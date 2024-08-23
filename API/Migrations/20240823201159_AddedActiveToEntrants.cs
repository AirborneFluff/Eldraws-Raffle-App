using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaffleApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedActiveToEntrants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Entrants",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Entrants");
        }
    }
}
