using Grpc.Core;
using NewBankServer.Models;
using NewBankServer.Protos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace NewBankServer.Services
{
  public class SessionService : SessionCRUD.SessionCRUDBase
  {
    public override Task<Empty> AddSession(SessionRequest request, ServerCallContext context)
    {
      using var db = new SqlServerDbContext();
      if (Guid.Parse(request.SessionId) == Guid.Empty)
        throw new RpcException(new Status(StatusCode.InvalidArgument, request.SessionId));
      if (db.Sessions.Any(s => s.ID == Guid.Parse(request.SessionId)))
        throw new RpcException(new Status(StatusCode.AlreadyExists, "Session already exists"));

      db.Sessions.Add(SessionModel.ConvertSession(request));
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }

    public override Task<Empty> RemoveSession(SessionRequest request, ServerCallContext context)
    {
      using var db = new SqlServerDbContext();
      if (Guid.Parse(request.SessionId) == Guid.Empty)
        throw new RpcException(new Status(StatusCode.InvalidArgument, request.SessionId));

      var session = db.Sessions.FirstOrDefault(s => s.ID == Guid.Parse(request.SessionId));

      if (session == null)
        throw new RpcException(new Status(StatusCode.NotFound, "Session does not exist"));

      db.Sessions.Remove(session);
      db.SaveChanges();
      return Task.FromResult(new Empty());

    }

    public override Task<Sessions> GetSessions(Empty request, ServerCallContext context)
    {
      using var db = new SqlServerDbContext();
      var sessions = new Sessions();
      var query = from s in db.Sessions
                  select SessionModel.ConvertSession(s);
      sessions.Items.AddRange(query.ToArray());
      return Task.FromResult(sessions);
    }

    public override Task<ValidSessionResponse> IsValidSession(SessionRequest request, ServerCallContext context)
    {
      using var db = new SqlServerDbContext();
      var exists = db.Sessions.FirstOrDefault(s => s.ID == Guid.Parse(request.SessionId)) != null;
      return Task.FromResult(new ValidSessionResponse { Valid = exists });
    }

    public override Task<Empty> ClearSessions(Empty request, ServerCallContext context)
    {
      using var db = new SqlServerDbContext();
      foreach (var session in db.Sessions)
        db.Sessions.Remove(session);

      db.SaveChanges();

      return Task.FromResult(new Empty());
    }
  }
}
