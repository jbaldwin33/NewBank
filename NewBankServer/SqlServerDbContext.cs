using Microsoft.EntityFrameworkCore;
using System;
using NewBankServer.Models;
using System.IO;
using ServerShared;
using Microsoft.Extensions.Configuration;

namespace NewBankServer
{
  public class SqlServerDbContext : DbContext
  {
    private static readonly string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NewBank", "database", "ServerConfiguration.xml");
    public DbSet<UserModel> Users { get; set; }
    public DbSet<AccountModel> Accounts { get; set; }
    public DbSet<SessionModel> Sessions { get; set; }
    public DbSet<TransactionModel> Transactions { get; set; }

    public SqlServerDbContext()
    {
      Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .Build();

      if (!File.Exists(filename))
      {
        //show message and exit
        throw new FileNotFoundException();
      }

      var conf = new ConfigurationModel().LoadConfiguration();
      if (conf.UseSqlServer)
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("AppDb"));
      else
        optionsBuilder.UseSqlite(configuration.GetConnectionString("LiteDb"));
    }
  }
}
