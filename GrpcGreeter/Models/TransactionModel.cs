using Google.Protobuf.WellKnownTypes;
using GrpcGreeter.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.Models
{
  public enum TransactionDbEnum
  {
    Deposit,
    Withdraw,
    Transfer
  }

  public class TransactionModel
  {
    public Guid ID { get; set; }
    public DateTime TransactionCreatedTime { get; set; }
    public string Message { get; set; }
    public Guid UserID { get; set; }
    public TransactionDbEnum TransactionType { get; set; }
    public double Amount { get; set; }

    public static TransactionDbEnum ConvertTransactionType(TransactionProtoEnum transactionType)
      => transactionType switch
      {
        TransactionProtoEnum.Deposit => TransactionDbEnum.Deposit,
        TransactionProtoEnum.Withdraw => TransactionDbEnum.Withdraw,
        TransactionProtoEnum.Transfer => TransactionDbEnum.Transfer,
        _ => throw new NotSupportedException($"{transactionType} not supported")
      };

    public static TransactionProtoEnum ConvertTransactionType(TransactionDbEnum transactionType)
      => transactionType switch
      {
        TransactionDbEnum.Deposit => TransactionProtoEnum.Deposit,
        TransactionDbEnum.Withdraw => TransactionProtoEnum.Withdraw,
        TransactionDbEnum.Transfer => TransactionProtoEnum.Transfer,
        _ => throw new NotSupportedException($"{transactionType} not supported")
      };

    public static Transaction ConvertTransaction(TransactionModel model) => new Transaction
    {
      Id = model.ID.ToString(),
      Amount = model.Amount,
      Message = model.Message,
      TransactionCreatedTime = Timestamp.FromDateTime(model.TransactionCreatedTime),
      TransactionType = ConvertTransactionType(model.TransactionType),
      UserId = model.UserID.ToString()
    };

    public static TransactionModel ConvertTransaction(Transaction transaction) => new TransactionModel
    {
      ID = Guid.Parse(transaction.Id),
      Amount = transaction.Amount,
      Message = transaction.Message,
      TransactionCreatedTime = transaction.TransactionCreatedTime.ToDateTime(),
      TransactionType = ConvertTransactionType(transaction.TransactionType),
      UserID = Guid.Parse(transaction.UserId)
    };

    public static TransactionModel CreateDepositTransaction(DepositRequest request, AccountModel account) => new TransactionModel
    {
      Amount = request.Amount,
      ID = Guid.NewGuid(),
      Message = $"${request.Amount}.00 deposited into the account",
      TransactionCreatedTime = DateTime.Now,
      TransactionType = TransactionDbEnum.Deposit,
      UserID = account.UserID
    };

    public static TransactionModel CreateWithdrawTransaction(WithdrawRequest request, AccountModel account) => new TransactionModel
    {
      Amount = request.Amount,
      ID = Guid.NewGuid(),
      Message = $"${request.Amount}.00 withdrawn from the account",
      TransactionCreatedTime = DateTime.Now,
      TransactionType = TransactionDbEnum.Withdraw,
      UserID = account.UserID
    };

    public static TransactionModel CreateTransferToTransaction(TransferRequest request, UserModel user) => new TransactionModel
    {
      Amount = request.Amount,
      ID = Guid.NewGuid(),
      Message = $"${request.Amount}.00 transferred to the user {user.Username}",
      TransactionCreatedTime = DateTime.Now,
      TransactionType = TransactionDbEnum.Transfer,
      UserID = user.ID
    };

    public static TransactionModel CreateTransferFromTransaction(TransferRequest request, UserModel user) => new TransactionModel
    {
      Amount = request.Amount,
      ID = Guid.NewGuid(),
      Message = $"${request.Amount}.00 transferred from user {user.Username}",
      TransactionCreatedTime = DateTime.Now,
      TransactionType = TransactionDbEnum.Transfer,
      UserID = user.ID
    };
  }

}
