using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewBankServer.Migrations
{
    public partial class NewNewUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Skills_SkillID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SkillID",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "SkillID",
                table: "Users",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "SkillID",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateIndex(
                name: "IX_Users_SkillID",
                table: "Users",
                column: "SkillID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Skills_SkillID",
                table: "Users",
                column: "SkillID",
                principalTable: "Skills",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
