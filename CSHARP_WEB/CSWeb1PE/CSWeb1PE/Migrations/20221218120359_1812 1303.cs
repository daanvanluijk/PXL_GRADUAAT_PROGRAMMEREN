using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSWeb1PE.Migrations
{
    public partial class _18121303 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ВременнаРоляId",
                table: "Gebruikers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Gebruikers_ВременнаРоляId",
                table: "Gebruikers",
                column: "ВременнаРоляId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gebruikers_AspNetRoles_ВременнаРоляId",
                table: "Gebruikers",
                column: "ВременнаРоляId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gebruikers_AspNetRoles_ВременнаРоляId",
                table: "Gebruikers");

            migrationBuilder.DropIndex(
                name: "IX_Gebruikers_ВременнаРоляId",
                table: "Gebruikers");

            migrationBuilder.DropColumn(
                name: "ВременнаРоляId",
                table: "Gebruikers");
        }
    }
}
