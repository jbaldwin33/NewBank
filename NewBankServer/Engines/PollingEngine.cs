using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NewBankServer.Engines
{
  public interface IPollingEngine
  {
    void Run();
  }
  public class PollingEngine : IPollingEngine
  {
    public PollingEngine() => Run();
    public void Run()
    {
      Task.Factory.StartNew(() =>
      {
        while (true)
        {
          using var db = new AppDbContext();
          foreach (var session in db.Sessions)
          {
            if (DateTime.UtcNow >= session.LogInDateTime.AddSeconds(10))
            {

              db.Sessions.Remove(session);
            }
          }
          db.SaveChanges();
          Task.Delay(30000);
        }
      });
    }
  }
}
