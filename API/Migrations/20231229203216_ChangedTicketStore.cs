using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaffleApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTicketStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Tickets",
                table: "Entries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "HighTicket",
                table: "Entries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LowTicket",
                table: "Entries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighTicket",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "LowTicket",
                table: "Entries");

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
    }
}
