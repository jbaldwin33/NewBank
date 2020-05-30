using Grpc.Core;
using NewBankServer.Models;
using NewBankServer.Protos;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBankServer.Services
{
  public class TransactionService : TransactionCRUD.TransactionCRUDBase
  {
    public async override Task<Empty> CreateTransaction(CreateTransactionRequest request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      await db.Transactions.AddAsync(TransactionModel.ConvertTransaction(request.Transaction));
      await db.SaveChangesAsync();

      return await Task.FromResult(new Empty());
    }

    public async override Task GetAllUserTransactions(GetAllUserTransactionsRequest request, IServerStreamWriter<Transaction> responseStream, ServerCallContext context)
    {
      using var db = new AppDbContext();
      if (db.Sessions.FirstOrDefault(s => s.ID == Guid.Parse(request.SessionId)) == null)
        throw new RpcException(new Status(StatusCode.PermissionDenied, "Session is invalid"));

      var transactions = db.Transactions.Where(t => t.UserID == Guid.Parse(request.UserId));
      foreach (var transaction in transactions)
        await responseStream.WriteAsync(TransactionModel.ConvertTransaction(transaction));
    }

    public override Task<Transactions> GetTransactionsByFilter(GetTransactionsByFilterRequest request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      if (db.Sessions.FirstOrDefault(s => s.ID == Guid.Parse(request.SessionId)) == null)
        throw new RpcException(new Status(StatusCode.PermissionDenied, "Session is invalid"));

      TransactionModel[] transactions;
      if (request.Amount > 0)
        transactions = db.Transactions.Where(t => t.Amount == request.Amount).ToArray();
      //other filters
      var allTransactions = new Transactions();
      var output = from t in db.Transactions
                   select TransactionModel.ConvertTransaction(t);
      allTransactions.Items.AddRange(output);
      return Task.FromResult(allTransactions);
    }
  }
}
