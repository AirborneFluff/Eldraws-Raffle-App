using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaffleApi.Migrations
{
    /// <inheritdoc />
    public partial class IncludeClanMembersEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClanMember_AspNetUsers_MemberId",
                table: "ClanMember");

            migrationBuilder.DropForeignKey(
                name: "FK_ClanMember_Clans_ClanId",
                table: "ClanMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClanMember",
                table: "ClanMember");

            migrationBuilder.RenameTable(
                name: "ClanMember",
                newName: "ClanMembers");

            migrationBuilder.RenameIndex(
                name: "IX_ClanMember_MemberId",
                table: "ClanMembers",
                newName: "IX_ClanMembers_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_ClanMember_ClanId",
                table: "ClanMembers",
                newName: "IX_ClanMembers_ClanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClanMembers",
                table: "ClanMembers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClanMembers_AspNetUsers_MemberId",
                table: "ClanMembers",
                column: "MemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClanMembers_Clans_ClanId",
                table: "ClanMembers",
                column: "ClanId",
                principalTable: "Clans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClanMembers_AspNetUsers_MemberId",
                table: "ClanMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ClanMembers_Clans_ClanId",
                table: "ClanMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClanMembers",
                table: "ClanMembers");

            migrationBuilder.RenameTable(
                name: "ClanMembers",
                newName: "ClanMember");

            migrationBuilder.RenameIndex(
                name: "IX_ClanMembers_MemberId",
                table: "ClanMember",
                newName: "IX_ClanMember_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_ClanMembers_ClanId",
                table: "ClanMember",
                newName: "IX_ClanMember_ClanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClanMember",
                table: "ClanMember",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClanMember_AspNetUsers_MemberId",
                table: "ClanMember",
                column: "MemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClanMember_Clans_ClanId",
                table: "ClanMember",
                column: "ClanId",
                principalTable: "Clans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
