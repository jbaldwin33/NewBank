using Google.Protobuf.WellKnownTypes;
using NewBankServer.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBankServer.Models
{
  public enum TransactionDbEnum
  {
    Deposit,
    Withdraw,
    Transfer,
    LogIn,
    LogOut
  }

  public class TransactionModel
  {
    public Guid ID { get; set; }
    public DateTime TransactionCreatedTime { get; set; }
    public string Message { get; set; }
    public Guid UserID { get; set; }
    public TransactionDbEnum TransactionDbEnum { get; }
    public TransactionDbEnum TransactionType { get; set; }
    public double Amount { get; set; }

    public TransactionModel() { }

    public TransactionModel(Guid id, DateTime transactionCreatedTime, string message, Guid userID, TransactionDbEnum transactionDbEnum, double amount)
    {
      ID = id;
      TransactionCreatedTime = transactionCreatedTime;
      Message = message;
      UserID = userID;
      TransactionDbEnum = transactionDbEnum;
      Amount = amount;
    }
    public static TransactionDbEnum ConvertTransactionType(TransactionProtoEnum transactionType)
      => transactionType switch
      {
        TransactionProtoEnum.Deposit => TransactionDbEnum.Deposit,
        TransactionProtoEnum.Withdraw => TransactionDbEnum.Withdraw,
        TransactionProtoEnum.Transfer => TransactionDbEnum.Transfer,
        TransactionProtoEnum.Login => TransactionDbEnum.LogIn,
        TransactionProtoEnum.Logout => TransactionDbEnum.LogOut,
        _ => throw new NotSupportedException($"{transactionType} not supported")
      };

    public static TransactionProtoEnum ConvertTransactionType(TransactionDbEnum transactionType)
      => transactionType switch
      {
        TransactionDbEnum.Deposit => TransactionProtoEnum.Deposit,
        TransactionDbEnum.Withdraw => TransactionProtoEnum.Withdraw,
        TransactionDbEnum.Transfer => TransactionProtoEnum.Transfer,
        TransactionDbEnum.LogIn => TransactionProtoEnum.Login,
        TransactionDbEnum.LogOut => TransactionProtoEnum.Logout,
        _ => throw new NotSupportedException($"{transactionType} not supported")
      };

    public static Transaction ConvertTransaction(TransactionModel model) => new Transaction
    {
      Id = model.ID.ToString(),
      Amount = model.Amount,
      Message = model.Message,
      TransactionCreatedTime = Timestamp.FromDateTime(DateTime.SpecifyKind(model.TransactionCreatedTime, DateTimeKind.Utc)),
      TransactionType = ConvertTransactionType(model.TransactionType),
      UserId = model.UserID.ToString()
    };

    public static TransactionModel ConvertTransaction(Transaction transaction) => new TransactionModel(
      Guid.Parse(transaction.Id),
      DateTime.SpecifyKind(transaction.TransactionCreatedTime.ToDateTime(), DateTimeKind.Utc),
      transaction.Message,
      Guid.Parse(transaction.UserId),
      ConvertTransactionType(transaction.TransactionType),
      transaction.Amount);

    public static TransactionModel CreateDepositTransaction(DepositRequest request, AccountModel account) => new TransactionModel(
      Guid.NewGuid(),
      DateTime.UtcNow,
      $"${request.Amount}.00 deposited into the account",
      account.UserID,
      TransactionDbEnum.Deposit,
      request.Amount);

    public static TransactionModel CreateWithdrawTransaction(WithdrawRequest request, AccountModel account) => new TransactionModel(
      Guid.NewGuid(),
      DateTime.UtcNow,
      $"${request.Amount}.00 withdrawn from the account",
      account.UserID,
      TransactionDbEnum.Withdraw,
      request.Amount);

    public static TransactionModel CreateTransferToTransaction(TransferRequest request, UserModel user) => new TransactionModel(
      Guid.NewGuid(),
      DateTime.UtcNow,
      $"${request.Amount}.00 transferred to the user {request.ToUsername}",
      user.ID,
      TransactionDbEnum.Transfer,
      request.Amount);

    public static TransactionModel CreateTransferFromTransaction(TransferRequest request, UserModel user) => new TransactionModel(
      Guid.NewGuid(),
      DateTime.UtcNow,
      $"${request.Amount}.00 transferred from user {request.FromUsername}",
      user.ID,
      TransactionDbEnum.Transfer,
      request.Amount);

    public static TransactionModel CreateLoginTransaction(UserModel user) => new TransactionModel(
      Guid.NewGuid(),
      DateTime.UtcNow,
      $"Logged in",
      user.ID,
      TransactionDbEnum.LogIn,
      0);

    public static TransactionModel CreateLogoutTransaction(UserModel user) => new TransactionModel(
      Guid.NewGuid(),
      DateTime.UtcNow,
      $"Logged out",
      user.ID,
      TransactionDbEnum.LogOut,
      0);
  }

}
