using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Constraints;
using NewBankServer.Models;
using NewBankServer.Services;
using System.IO;
using System.Reflection;
using ServerShared;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;

namespace NewBankServer
{
  public class AppDbContext : DbContext
  {
    private static readonly string filename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ServerConfiguration.xml");
    public DbSet<UserModel> Users { get; set; }
    public DbSet<AccountModel> Accounts { get; set; }
    public DbSet<SessionModel> Sessions { get; set; }
    public DbSet<TransactionModel> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .Build();
      var model = LoadConfiguration();
      if (model.UseSqlServer)
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("AppDb"));
      else
      {
        var connectionString = configuration.GetConnectionString("LiteDb");
        if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "MyLiteDb.db")))
        {
          SQLiteConnection.CreateFile("MyLiteDb.db");
          using SQLiteConnection conn = new SQLiteConnection(connectionString);
          //conn.
        }

        optionsBuilder.UseSqlite(connectionString);
      }

    }

    private ConfigurationModel LoadConfiguration()
    {
      var serializer = new XmlSerializer(typeof(ConfigurationModel));
      using var stream = new StreamReader(filename);
      return serializer.Deserialize(stream) as ConfigurationModel;
    }
  }
}
