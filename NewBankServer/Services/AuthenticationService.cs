﻿using Grpc.Core;
using NewBankServer.Models;
using NewBankServer.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBankServer.Services
{
    public class AuthenticationService : Authentication.AuthenticationBase
    {
        public override Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            using var db = new SqlServerDbContext();
            if (string.IsNullOrEmpty(request.Username))
                throw new RpcException(new Status(StatusCode.InvalidArgument, nameof(request.Username)));
            if (string.IsNullOrEmpty(request.PasswordHash))
                throw new RpcException(new Status(StatusCode.InvalidArgument, nameof(request.PasswordHash)));

            var user = db.Users.FirstOrDefault(u => u.Username == request.Username && u.PasswordHash == request.PasswordHash);
            if (user == null)
                throw new RpcException(new Status(StatusCode.NotFound, "No user exists"));

            var userSession = db.Sessions.FirstOrDefault(s => s.Username == request.Username);
            if (userSession != null)
                db.Sessions.Remove(userSession);

            var sessionID = Guid.NewGuid();
            db.Sessions.Add(new SessionModel(sessionID, request.Username, DateTime.UtcNow));
            db.Transactions.Add(TransactionModel.CreateLoginTransaction(user));
            db.SaveChanges();
            Console.WriteLine($"Number of active sessions : {db.Sessions.Count()}");

            return Task.FromResult(new LoginResponse { SessionID = sessionID.ToString(), User = UserModel.ConvertUser(user) });
        }

        public override Task<LogoutResponse> Logout(LogoutRequest request, ServerCallContext context)
        {
            using var db = new SqlServerDbContext();
            if (string.IsNullOrEmpty(request.SessionId))
                throw new RpcException(new Status(StatusCode.InvalidArgument, nameof(request.SessionId)));
            var session = db.Sessions.FirstOrDefault(s => s.ID == Guid.Parse(request.SessionId));
            var user = db.Users.FirstOrDefault(u => u.ID == Guid.Parse(request.User.Id));
            if (session == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Session does not exist"));
            if (user == null)
                throw new RpcException(new Status(StatusCode.NotFound, "User does not exist"));

            db.Sessions.Remove(session);
            db.Transactions.Add(TransactionModel.CreateLogoutTransaction(user));
            db.SaveChanges();
            Console.WriteLine($"Number of active sessions : {db.Sessions.Count()}");

            return Task.FromResult(new LogoutResponse());
        }
    }
}
