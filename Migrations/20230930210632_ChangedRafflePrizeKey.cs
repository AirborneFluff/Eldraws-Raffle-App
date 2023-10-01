using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaffleApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangedRafflePrizeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Prizes",
                table: "Prizes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Prizes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prizes",
                table: "Prizes",
                columns: new[] { "Place", "RaffleId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Prizes",
                table: "Prizes");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Prizes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prizes",
                table: "Prizes",
                column: "Id");
        }
    }
}
