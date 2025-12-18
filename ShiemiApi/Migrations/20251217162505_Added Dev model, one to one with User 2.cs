using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiemiApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedDevmodelonetoonewithUser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dev_Users_UserId",
                table: "Dev");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dev",
                table: "Dev");

            migrationBuilder.RenameTable(
                name: "Dev",
                newName: "Devs");

            migrationBuilder.RenameIndex(
                name: "IX_Dev_UserId",
                table: "Devs",
                newName: "IX_Devs_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Devs",
                table: "Devs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Devs_Users_UserId",
                table: "Devs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devs_Users_UserId",
                table: "Devs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Devs",
                table: "Devs");

            migrationBuilder.RenameTable(
                name: "Devs",
                newName: "Dev");

            migrationBuilder.RenameIndex(
                name: "IX_Devs_UserId",
                table: "Dev",
                newName: "IX_Dev_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dev",
                table: "Dev",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dev_Users_UserId",
                table: "Dev",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
