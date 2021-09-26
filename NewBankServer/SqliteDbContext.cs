using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NewBankServer.Models;
using ServerShared;

namespace NewBankServer
{
  public class SqliteDbContext : DbContext
  {
    private static readonly string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NewBank", "database", "ServerConfiguration.xml");
    public DbSet<UserModel> Users { get; set; }
    public DbSet<AccountModel> Accounts { get; set; }
    public DbSet<SessionModel> Sessions { get; set; }
    public DbSet<TransactionModel> Transactions { get; set; }

    public SqliteDbContext()
    {
      this.Database.EnsureCreated();
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

      optionsBuilder.UseSqlite(configuration.GetConnectionString("LiteDb"));
    }
  }
}
