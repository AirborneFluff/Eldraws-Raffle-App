using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaffleApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedDynamicPrizes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "DonationPercentage",
                table: "Prizes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<string>(
                name: "Tickets",
                table: "Entries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonationPercentage",
                table: "Prizes");

            migrationBuilder.AlterColumn<string>(
                name: "Tickets",
                table: "Entries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
