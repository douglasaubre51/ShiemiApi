using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiemiApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDevmodelandmore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Projects_ProjectId",
                table: "Rooms");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Rooms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DevId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_DevId",
                table: "Rooms",
                column: "DevId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Devs_DevId",
                table: "Rooms",
                column: "DevId",
                principalTable: "Devs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Projects_ProjectId",
                table: "Rooms",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Devs_DevId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Projects_ProjectId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_DevId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "DevId",
                table: "Rooms");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Projects_ProjectId",
                table: "Rooms",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
