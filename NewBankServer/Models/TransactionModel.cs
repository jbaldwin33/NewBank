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
    public TransactionDbEnum TransactionType { get; set; }
    public double Amount { get; set; }

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

    public static TransactionModel ConvertTransaction(Transaction transaction) => new TransactionModel
    {
      ID = Guid.Parse(transaction.Id),
      Amount = transaction.Amount,
      Message = transaction.Message,
      TransactionCreatedTime = DateTime.SpecifyKind(transaction.TransactionCreatedTime.ToDateTime(), DateTimeKind.Utc),
      TransactionType = ConvertTransactionType(transaction.TransactionType),
      UserID = Guid.Parse(transaction.UserId)
    };

    public static TransactionModel CreateDepositTransaction(DepositRequest request, AccountModel account) => new TransactionModel
    {
      Amount = request.Amount,
      ID = Guid.NewGuid(),
      Message = $"${request.Amount}.00 deposited into the account",
      TransactionCreatedTime = DateTime.UtcNow,
      TransactionType = TransactionDbEnum.Deposit,
      UserID = account.UserID
    };

    public static TransactionModel CreateWithdrawTransaction(WithdrawRequest request, AccountModel account) => new TransactionModel
    {
      Amount = request.Amount,
      ID = Guid.NewGuid(),
      Message = $"${request.Amount}.00 withdrawn from the account",
      TransactionCreatedTime = DateTime.UtcNow,
      TransactionType = TransactionDbEnum.Withdraw,
      UserID = account.UserID
    };

    public static TransactionModel CreateTransferToTransaction(TransferRequest request, UserModel user) => new TransactionModel
    {
      Amount = request.Amount,
      ID = Guid.NewGuid(),
      Message = $"${request.Amount}.00 transferred to the user {user.Username}",
      TransactionCreatedTime = DateTime.UtcNow,
      TransactionType = TransactionDbEnum.Transfer,
      UserID = user.ID
    };

    public static TransactionModel CreateTransferFromTransaction(TransferRequest request, UserModel user) => new TransactionModel
    {
      Amount = request.Amount,
      ID = Guid.NewGuid(),
      Message = $"${request.Amount}.00 transferred from user {user.Username}",
      TransactionCreatedTime = DateTime.UtcNow,
      TransactionType = TransactionDbEnum.Transfer,
      UserID = user.ID
    };

    public static TransactionModel CreateLoginTransaction(UserModel user) => new TransactionModel
    {
      ID = Guid.NewGuid(),
      Message = $"Logged in",
      TransactionCreatedTime = DateTime.UtcNow,
      TransactionType = TransactionDbEnum.LogIn,
      UserID = user.ID
    };

    public static TransactionModel CreateLogoutTransaction(UserModel user) => new TransactionModel
    {
      ID = Guid.NewGuid(),
      Message = $"Logged out",
      TransactionCreatedTime = DateTime.UtcNow,
      TransactionType = TransactionDbEnum.LogOut,
      UserID = user.ID
    };
  }

}
