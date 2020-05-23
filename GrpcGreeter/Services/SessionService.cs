using Grpc.Core;
using GrpcGreeter.Models;
using GrpcGreeter.Protos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcGreeter.Services
{
  public class SessionService : SessionCRUD.SessionCRUDBase
  {
    private readonly AppDbContext db;
    public SessionService(AppDbContext db)
    {
      this.db = db;
    }

    public override Task<Empty> AddSession(SessionRequest request, ServerCallContext context)
    {
      if (Guid.Parse(request.SessionId) == Guid.Empty)
        throw new ArgumentNullException(request.SessionId);
      if (db.Sessions.Any(s => s.ID == Guid.Parse(request.SessionId)))
        throw new ArgumentException("Session already exists");

      db.Sessions.Add(new SessionModel { ID = Guid.Parse(request.SessionId) });
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }

    public override Task<Empty> RemoveSession(SessionRequest request, ServerCallContext context)
    {
      if (Guid.Parse(request.SessionId) == Guid.Empty)
        throw new ArgumentNullException(request.SessionId);

      var session = db.Sessions.FirstOrDefault(s => s.ID == Guid.Parse(request.SessionId));

      if (session == null)
        throw new ArgumentException("Session does not exist");

      db.Sessions.Remove(session);
      db.SaveChanges();
      return Task.FromResult(new Empty());

    }

    public override Task<Sessions> GetSessions(Empty request, ServerCallContext context)
    {
      var sessions = new Sessions();
      var query = from s in db.Sessions
                  select new SessionRequest
                  {
                    SessionId = s.ID.ToString()
                  };
      sessions.Items.AddRange(query.ToArray());
      return Task.FromResult(sessions);
    }

    public override Task<ValidSessionResponse> IsValidSession(SessionRequest request, ServerCallContext context)
    {
      var exists = db.Sessions.FirstOrDefault(s => s.ID == Guid.Parse(request.SessionId)) != null;
      return Task.FromResult(new ValidSessionResponse { Valid = exists });
    }
  } 
}
