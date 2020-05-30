using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBankServer.Engines
{
  public interface IPollingEngine
  {
    void Run();
  }
  public class PollingEngine : IPollingEngine
  {
    //private static readonly Lazy<PollingEngine> instance = new Lazy<PollingEngine>(() => new PollingEngine());
    //public static PollingEngine Instance => instance.Value;
    private readonly DbContextOptionsBuilder<AppDbContext> dbOptions;
    public PollingEngine(DbContextOptionsBuilder<AppDbContext> dbOptions)
    {
      this.dbOptions = dbOptions;
      Run();
    }
    public void Run()
    {
      var db = new AppDbContext(dbOptions.Options);
      Task.Factory.StartNew(() =>
      {
        while (true)
        {
          foreach (var session in db.Sessions)
          {
            if (session.LogInDateTime.AddMinutes(2) > DateTime.UtcNow)
            {
              db.Sessions.Remove(session);
              db.SaveChanges();
            }
          }
          Task.Delay(30000);
        }
      });
    }
  }
}
