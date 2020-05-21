using Grpc.Core;
using GrpcGreeter.Models;
using GrpcGreeter.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.Services
{
  public class AccountCRUDService : AccountCRUD.AccountCRUDBase
  {
    private readonly AppDbContext db;
    private readonly SessionService sessionService;
    public AccountCRUDService(AppDbContext db, SessionService sessionService)
    {
      this.db = db;
      this.sessionService = sessionService;
    }

    public override Task<Accounts> GetAccounts(Empty request, ServerCallContext context)
    {
      //if (!sessionService.IsValidSession(Guid.Parse(request.SessionId)))
      //  throw new InvalidOperationException("Invalid session");

      var accounts = new Accounts();
      var query = from a in db.Accounts
                  select new Account
                  {
                    Id = a.ID.ToString(),
                    Balance = a.Balance,
                    AccountType = AccountModel.ConvertFromDbType(a.AccountType)
                  };
      accounts.Items.AddRange(query.ToArray());
      return Task.FromResult(accounts);
    }

    public override Task<AccountResponse> GetByID(AccountFilter request, ServerCallContext context)
    {
      //if (!sessionService.IsValidSession(Guid.Parse(request.SessionId)))
      //  throw new InvalidOperationException("Invalid session");

      var data = db.Accounts.FirstOrDefault(a => a.ID == Guid.Parse(request.Id));
      if (data == null)
        throw new ArgumentNullException();

      var account = new Account
      {
        Id = data.ID.ToString(),
        Balance = data.Balance,
        AccountType = AccountModel.ConvertFromDbType(data.AccountType),
        UserId = data.UserID.ToString()
      };
      return Task.FromResult(new AccountResponse { Account = account });
    }

    public override Task<Empty> Insert(Account request, ServerCallContext context)
    {
      //if (!sessionService.IsValidSession(Guid.Parse(request.SessionId)))
      //  throw new InvalidOperationException("Invalid session");

      db.Accounts.Add(new AccountModel
      {
        ID = Guid.Parse(request.Id),
        Balance = request.Balance,
        AccountType = AccountModel.ConvertFromProtoType(request.AccountType),
        UserID = Guid.Parse(request.UserId)
      });
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }

    public override Task<Empty> Update(Account request, ServerCallContext context)
    {
      //if (!sessionService.IsValidSession(Guid.Parse(request.SessionId)))
      //  throw new InvalidOperationException("Invalid session");

      db.Accounts.Update(new AccountModel
      {
        ID = Guid.Parse(request.Id),
        Balance = request.Balance,
        AccountType = AccountModel.ConvertFromProtoType(request.AccountType),
        UserID = Guid.Parse(request.UserId)
      });
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }

    public override Task<Empty> Delete(AccountFilter request, ServerCallContext context)
    {
      //if (!sessionService.IsValidSession(Guid.Parse(request.SessionId)))
      //  throw new InvalidOperationException("Invalid session");

      var account = db.Accounts.FirstOrDefault(a => a.ID == Guid.Parse(request.Id));
      if (account == null)
        throw new ArgumentNullException();

      db.Accounts.Remove(account);
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }
  }
}
