using Google.Protobuf.WellKnownTypes;
using NewBankServer.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBankServer.Models
{
  public class SessionModel
  {
    public Guid ID { get; set; }
    public string Username { get; set; }
    public DateTime LogInDateTime { get; set; }

    public SessionModel() { }

    public SessionModel(Guid id, string username, DateTime logInDateTime)
    {
      ID = id;
      Username = username;
      LogInDateTime = logInDateTime;
    }

    public static SessionModel ConvertSession(SessionRequest session) => new SessionModel(
      Guid.Parse(session.SessionId), 
      session.Username, 
      DateTime.SpecifyKind(session.LogInDateTime.ToDateTime(), DateTimeKind.Utc));

    public static SessionRequest ConvertSession(SessionModel model) => new SessionRequest
    {
      SessionId = model.ID.ToString(),
      Username = model.Username,
      LogInDateTime = Timestamp.FromDateTime(DateTime.SpecifyKind(model.LogInDateTime, DateTimeKind.Utc))
    };
  }
}
