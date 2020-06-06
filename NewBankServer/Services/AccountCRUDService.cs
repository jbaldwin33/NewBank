//using BankServer.Services;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Routing;
using NewBankServer.Models;
using NewBankServer.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBankServer.Services
{
  public class AccountCRUDService : AccountCRUD.AccountCRUDBase
  {
    public override Task<Accounts> GetAccounts(Empty request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      var accounts = new Accounts();
      var query = from a in db.Accounts 
                  select AccountModel.ConvertAccount(a);
      accounts.Items.AddRange(query.ToArray());
      return Task.FromResult(accounts);
    }

    public override Task<AccountResponse> GetByID(AccountFilter request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      if (db.Sessions.FirstOrDefault(s => s.ID == Guid.Parse(request.SessionId)) == null)
        throw new RpcException(new Status(StatusCode.PermissionDenied, "Session is invalid"));

      var data = db.Accounts.FirstOrDefault(a => a.ID == Guid.Parse(request.Id));
      if (data == null)
        throw new RpcException(new Status(StatusCode.NotFound, "User not found"));

      return Task.FromResult(new AccountResponse { Account = AccountModel.ConvertAccount(data) });
    }

    public override Task<AccountResponse> GetByUserID(AccountRequest request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      if (db.Sessions.FirstOrDefault(s => s.ID == Guid.Parse(request.SessionId)) == null)
        throw new RpcException(new Status(StatusCode.PermissionDenied, "Session is invalid"));

      var account = db.Accounts.FirstOrDefault(a => a.UserID == Guid.Parse(request.UserId));
      if (account == null)
        throw new RpcException(new Status(StatusCode.NotFound, "No account exists"));

      return Task.FromResult(new AccountResponse { Account = AccountModel.ConvertAccount(account) });
    }

    public override Task<Empty> Insert(Account request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      db.Accounts.Add(AccountModel.ConvertAccount(request));
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }

    public override Task<Empty> Update(Account request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      db.Accounts.Update(AccountModel.ConvertAccount(request));
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }

    public override Task<Empty> Delete(AccountFilter request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      var account = db.Accounts.FirstOrDefault(a => a.ID == Guid.Parse(request.Id));
      if (account == null)
        throw new RpcException(new Status(StatusCode.NotFound, "Account not found"));

      db.Accounts.Remove(account);
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }

    public override Task<Empty> Deposit(DepositRequest request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      if (db.Sessions.FirstOrDefault(s => s.ID == Guid.Parse(request.SessionId)) == null)
        throw new RpcException(new Status(StatusCode.PermissionDenied, "Session is invalid"));

      if (request.Amount < 1)
        throw new RpcException(new Status(StatusCode.InvalidArgument, "Amount cannot be less than 0"));
      
      var account = db.Accounts.FirstOrDefault(a => a.ID == Guid.Parse(request.AccountId));
      if (account == null)
        throw new RpcException(new Status(StatusCode.NotFound, "Account not found"));

      account.Balance += request.Amount;
      db.Accounts.Attach(account);
      db.Entry(account).Property(a => a.Balance).IsModified = true;
      var t = TransactionModel.CreateDepositTransaction(request, account);

      db.Transactions.Add(t);
      db.SaveChanges();

      return Task.FromResult(new Empty());
    }

    public override Task<Empty> Withdraw(WithdrawRequest request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      if (db.Sessions.FirstOrDefault(s => s.ID == Guid.Parse(request.SessionId)) == null)
        throw new RpcException(new Status(StatusCode.PermissionDenied, "Session is invalid"));

      if (request.Amount < 1)
        throw new RpcException(new Status(StatusCode.InvalidArgument, "Amount cannot be less than 0"));

      var account = db.Accounts.FirstOrDefault(a => a.ID == Guid.Parse(request.AccountId));
      if (account == null)
        throw new RpcException(new Status(StatusCode.NotFound, "Account not found"));

      account.Balance -= request.Amount;
      db.Accounts.Attach(account);
      db.Entry(account).Property(a => a.Balance).IsModified = true;

      db.Transactions.Add(TransactionModel.CreateWithdrawTransaction(request, account));
      db.SaveChanges();

      return Task.FromResult(new Empty());
    }

    public override Task<Empty> Transfer(TransferRequest request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      if (db.Sessions.FirstOrDefault(s => s.ID == Guid.Parse(request.SessionId)) == null)
        throw new RpcException(new Status(StatusCode.PermissionDenied, "Session is invalid"));

      var toUser = db.Users.FirstOrDefault(u => u.Username == request.ToUsername);
      var fromUser = db.Users.FirstOrDefault(u => u.Username == request.FromUsername);
      var toAccount = db.Accounts.FirstOrDefault(a => a.UserID == toUser.ID);
      var fromAccount = db.Accounts.FirstOrDefault(a => a.UserID == fromUser.ID);

      if (toUser == null)
        throw new RpcException(new Status(StatusCode.NotFound, $"User {toUser.Username} not found"));
      if (fromUser == null)
        throw new RpcException(new Status(StatusCode.NotFound, $"User {fromUser.Username} not found"));
      if (toAccount == null)
        throw new RpcException(new Status(StatusCode.NotFound, $"Account for user {toUser.Username} not found"));
      if (fromAccount == null)
        throw new RpcException(new Status(StatusCode.NotFound, $"Account for user {fromUser.Username} not found"));

      toAccount.Balance += request.Amount;
      db.Accounts.Attach(toAccount);
      db.Entry(toAccount).Property(a => a.Balance).IsModified = true;

      fromAccount.Balance -= request.Amount;
      db.Accounts.Attach(fromAccount);
      db.Entry(fromAccount).Property(a => a.Balance).IsModified = true;

      db.Transactions.Add(TransactionModel.CreateTransferToTransaction(request, fromUser));
      db.Transactions.Add(TransactionModel.CreateTransferFromTransaction(request, toUser));
      db.SaveChanges();

      return Task.FromResult(new Empty());
    }
  }
}
