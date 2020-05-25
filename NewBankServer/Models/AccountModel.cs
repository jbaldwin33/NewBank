using NewBankServer.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBankServer.Models
{
  public enum AccountDbEnum
  {
    Checking,
    Saving
  }

  public class AccountModel
  {
    public Guid ID { get; set; }
    public double Balance { get; set; }
    public AccountDbEnum AccountType { get; set; }
    public Guid UserID { get; set; }

    public static AccountDbEnum ConvertAccountType(AccountProtoEnum accountType)
    {
      return accountType switch
      {
        AccountProtoEnum.Checking => AccountDbEnum.Checking,
        AccountProtoEnum.Saving => AccountDbEnum.Saving,
        _ => throw new NotSupportedException(),
      };
    }

    public static AccountProtoEnum ConvertAccountType(AccountDbEnum accountType)
    {
      return accountType switch
      {
        AccountDbEnum.Checking => AccountProtoEnum.Checking,
        AccountDbEnum.Saving => AccountProtoEnum.Saving,
        _ => throw new NotSupportedException(),
      };
    }

    public static Account ConvertAccount(AccountModel accountModel) => new Account
    {
      AccountType = ConvertAccountType(accountModel.AccountType),
      Balance = accountModel.Balance,
      Id = accountModel.ID.ToString(),
      UserId = accountModel.UserID.ToString()
    };

    public static AccountModel ConvertAccount(Account account) => new AccountModel
    {
      AccountType = ConvertAccountType(account.AccountType),
      Balance = account.Balance,
      ID = Guid.Parse(account.Id),
      UserID = Guid.Parse(account.UserId)
    };
  }

}
