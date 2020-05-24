using GrpcGreeter.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.Models
{
  public class SessionModel
  {
    public Guid ID { get; set; }
    public string Username { get; set; }

    public static SessionModel ConvertSession(SessionRequest session) => new SessionModel
    {
      ID = Guid.Parse(session.SessionId),
      Username = session.Username
    };

    public static SessionRequest ConvertSession(SessionModel model) => new SessionRequest
    {
      SessionId = model.ID.ToString(),
      Username = model.Username
    };
  }
}
