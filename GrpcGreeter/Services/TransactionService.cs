using Grpc.Core;
using GrpcGreeter.Models;
using GrpcGreeter.Protos;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.Services
{
  public class TransactionService : TransactionCRUD.TransactionCRUDBase
  {
    private readonly AppDbContext db;

    public TransactionService(AppDbContext db)
    {
      this.db = db;
    }
    public async override Task<Empty> CreateTransaction(CreateTransactionRequest request, ServerCallContext context)
    {
      await db.Transactions.AddAsync(TransactionModel.ConvertTransaction(request.Transaction));
      await db.SaveChangesAsync();

      return await Task.FromResult(new Empty());
    }

    public async override Task GetAllUserTransactions(GetAllUserTransactionsRequest request, IServerStreamWriter<Transaction> responseStream, ServerCallContext context)
    {
      while (!context.CancellationToken.IsCancellationRequested)
      {
        var transactions = db.Transactions;
        foreach (var transaction in transactions)
          await responseStream.WriteAsync(TransactionModel.ConvertTransaction(transaction));
      }
    }

    public override Task<Transactions> GetTransactionsByFilter(GetTransactionsByFilterRequest request, ServerCallContext context)
    {
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
