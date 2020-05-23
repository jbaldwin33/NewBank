using GrpcGreeter.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.Models
{
  public enum AccountType
  {
    Checking,
    Saving
  }

  public class AccountModel
  {
    public Guid ID { get; set; }
    public double Balance { get; set; }
    public AccountType AccountType { get; set; }
    public Guid UserID { get; set; }

    public static AccountType ConvertFromProtoType(Protos.AccountType accountType)
    {
      return accountType switch
      {
        Protos.AccountType.Checking => AccountType.Checking,
        Protos.AccountType.Saving => AccountType.Saving,
        _ => throw new NotSupportedException(),
      };
    }

    public static Protos.AccountType ConvertFromDbType(AccountType accountType)
    {
      return accountType switch
      {
        AccountType.Checking => Protos.AccountType.Checking,
        AccountType.Saving =>   Protos.AccountType.Saving,
        _ => throw new NotSupportedException(),
      };
    }

    public static Account ConvertAccount(AccountModel accountModel) => new Account
    {
      AccountType = ConvertFromDbType(accountModel.AccountType),
      Balance = accountModel.Balance,
      Id = accountModel.ID.ToString(),
      UserId = accountModel.UserID.ToString()
    };

    public static AccountModel ConvertAccount(Account account) => new AccountModel
    {
      AccountType = ConvertFromProtoType(account.AccountType),
      Balance = account.Balance,
      ID = Guid.Parse(account.Id),
      UserID = Guid.Parse(account.UserId)
    };
  }

}
