//using BankServer.Services;
using Grpc.Core;
using GrpcGreeter.Models;
using GrpcGreeter.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.Services
{
  public class AuthenticationService : Authentication.AuthenticationBase
  {
    private readonly AppDbContext db;
    //private readonly BankServer.Services.SessionService sessionService;
    public AuthenticationService(AppDbContext db/*, BankServer.Services.SessionService sessionService*/)
    {
      this.db = db;
      //this.sessionService = sessionService;
    }

    public override Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
      if (string.IsNullOrEmpty(request.Username))
        throw new ArgumentNullException(nameof(request.Username));
      if (string.IsNullOrEmpty(request.PasswordHash))
        throw new ArgumentNullException(nameof(request.PasswordHash));

      var user = db.Users.FirstOrDefault(u => u.Username == request.Username && u.PasswordHash == request.PasswordHash);
      if (user == null)
        throw new ArgumentOutOfRangeException("No user exists");

      var sessionID = Guid.NewGuid();
      //sessionService.AddSession(new BankServer.Services.Session(sessionID));
      //Console.WriteLine($"Number of active sessions : {sessionService.ActiveSessions()}");

      return Task.FromResult(new LoginResponse { SessionID = sessionID.ToString(), User = UserModel.ConvertUser(user) });
    }

    public override Task<LogoutResponse> Logout(LogoutRequest request, ServerCallContext context)
    {
      if (string.IsNullOrEmpty(request.SessionId))
        throw new ArgumentNullException(nameof(request.SessionId));
      //if (sessionService.IsValidSession(Guid.Parse(request.SessionId)))
      //  throw new ArgumentOutOfRangeException("Session does not exist");

      //sessionService.RemoveSession(Guid.Parse(request.SessionId));
      //Console.WriteLine($"Number of active sessions : {sessionService.ActiveSessions()}");

      return Task.FromResult(new LogoutResponse());
    }
  }
}
