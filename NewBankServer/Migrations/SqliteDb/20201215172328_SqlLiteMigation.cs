using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewBankServer.Migrations.SqliteDb
{
  public partial class SqlLiteMigation : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Accounts",
          columns: table => new
          {
            ID = table.Column<Guid>(type: "TEXT", nullable: false),
            Balance = table.Column<double>(type: "REAL", nullable: false),
            AccountType = table.Column<int>(type: "INTEGER", nullable: false),
            UserID = table.Column<Guid>(type: "TEXT", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Accounts", x => x.ID);
          });

      migrationBuilder.CreateTable(
          name: "Sessions",
          columns: table => new
          {
            ID = table.Column<Guid>(type: "TEXT", nullable: false),
            Username = table.Column<string>(type: "TEXT", nullable: true),
            LogInDateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Sessions", x => x.ID);
          });

      migrationBuilder.CreateTable(
          name: "Transactions",
          columns: table => new
          {
            ID = table.Column<Guid>(type: "TEXT", nullable: false),
            TransactionCreatedTime = table.Column<DateTime>(type: "TEXT", nullable: false),
            Message = table.Column<string>(type: "TEXT", nullable: true),
            UserID = table.Column<Guid>(type: "TEXT", nullable: false),
            TransactionType = table.Column<int>(type: "INTEGER", nullable: false),
            Amount = table.Column<double>(type: "REAL", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Transactions", x => x.ID);
          });

      migrationBuilder.CreateTable(
          name: "Users",
          columns: table => new
          {
            ID = table.Column<Guid>(type: "TEXT", nullable: false),
            FirstName = table.Column<string>(type: "TEXT", nullable: true),
            LastName = table.Column<string>(type: "TEXT", nullable: true),
            Username = table.Column<string>(type: "TEXT", nullable: true),
            PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
            PasswordSalt = table.Column<string>(type: "TEXT", nullable: true),
            AccountID = table.Column<Guid>(type: "TEXT", nullable: false),
            UserType = table.Column<int>(type: "INTEGER", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Users", x => x.ID);
          });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Accounts");

      migrationBuilder.DropTable(
          name: "Sessions");

      migrationBuilder.DropTable(
          name: "Transactions");

      migrationBuilder.DropTable(
          name: "Users");
    }
  }
}
