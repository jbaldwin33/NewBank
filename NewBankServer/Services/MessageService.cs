using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBankServer.Services
{
  public enum MessageSeverity
  {
    Information = 0,
    Warning = 1,
    Error = 2
  }
  public interface IMessageService
  {
    void ShowMessage(string message, string caption, MessageSeverity severity);
  }
  public class MessageService
  {
  }
}
