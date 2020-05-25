using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewBankServer.Migrations
{
    public partial class CreatePerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Proficiency = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    SkillID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Persons_Skill_SkillID",
                        column: x => x.SkillID,
                        principalTable: "Skill",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_SkillID",
                table: "Persons",
                column: "SkillID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Skill");
        }
    }
}
