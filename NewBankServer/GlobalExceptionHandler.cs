using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBankServer
{
  public class GlobalExceptionHandler
  {
    private static string DefaultErrorMessage { get; set; } = "Unhandled Error Occurred. No details are known.";

    public static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
    {
      try
      {
        var message = DefaultErrorMessage;

        if (e.ExceptionObject is Exception uex)
          message = $"{DefaultErrorMessage} Exception: {uex.Message}";
        else if (sender is Exception ex)
          message = $"{DefaultErrorMessage} Exception: {ex.Message}";

        Console.WriteLine(message);
      }
      catch { } // Swallow exception
    }

    public static void UnobservedTaskExceptionHandler(object sender, UnobservedTaskExceptionEventArgs e)
    {

    }
  }
}
