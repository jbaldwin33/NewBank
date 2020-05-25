using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewBankServer.Migrations
{
    public partial class tra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    TransactionCreatedTime = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    UserID = table.Column<Guid>(nullable: false),
                    TransactionType = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
