using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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
        try
        {
          await using (var db = new SqlServerDbContext())
          //await using(var db = new SqliteDbContext())
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
        }
        catch (AggregateException aex)
        {
          foreach (var e in aex.InnerExceptions)
          {
            Console.WriteLine(e.Message);
          }
        }
        catch (FileNotFoundException fnfe)
        {
          Console.WriteLine("Configuration file not found. Please run the ServerConfiguration application to create run. This application will now close.");
          Environment.Exit(0);
        }
        catch (Exception ex)
        {
          //some other exception
          Console.WriteLine(ex.Message);
        }

        await Task.Delay(30000);
      }
    }
  }
}
