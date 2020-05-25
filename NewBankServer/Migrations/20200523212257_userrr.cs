using Microsoft.EntityFrameworkCore.Migrations;

namespace NewBankServer.Migrations
{
    public partial class userrr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Sessions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Sessions");
        }
    }
}
