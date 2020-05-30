using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewBankServer.Models;
using NewBankServer.Services;

namespace NewBankServer
{
  public class AppDbContext : DbContext
  {
    public DbSet<UserModel> Users { get; set; }
    public DbSet<AccountModel> Accounts { get; set; }
    public DbSet<SessionModel> Sessions { get; set; }
    public DbSet<TransactionModel> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;initial catalog=NewBank;Trusted_Connection=true;");
    }
  }
}
