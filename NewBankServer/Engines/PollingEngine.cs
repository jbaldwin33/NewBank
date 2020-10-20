using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NewBankServer.Engines
{
  public interface IPollingEngine
  {
    Task Run();
  }
  public class PollingEngine : IPollingEngine
  {
    public PollingEngine()
    {
      Run().GetAwaiter();
    }

    public async Task Run()
    {
      await DoWork();
    }

    private static async Task DoWork()
    {
      while (true)
      {
        var removed = false;
        await using (var db = new AppDbContext())
        {
          foreach (var session in db.Sessions)
          {
            if (DateTime.UtcNow < session.LogInDateTime.AddMinutes(5))
              continue;
            db.Sessions.Remove(session);
            removed = true;
          }

          if (removed)
            db.SaveChangesAsync().Wait();
        }
        await Task.Delay(30000);
      }
    }
  }
}
