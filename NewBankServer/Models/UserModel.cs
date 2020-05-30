using NewBankServer.Protos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NewBankServer.Models
{
  public enum UserDbEnum 
  {
    User, 
    Admin 
  }
  public class UserModel
  {
    public Guid ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public Guid AccountID { get; set; }
    public UserDbEnum UserType { get; set; }

    public UserModel() { }

    public UserModel(Guid id, string firstName, string lastName, string username, string passwordHash, string passwordSalt, Guid accountID, UserDbEnum userType)
    {
      ID = id;
      FirstName = firstName;
      LastName = lastName;
      Username = username;
      PasswordHash = passwordHash;
      PasswordSalt = passwordSalt;
      AccountID = accountID;
      UserType = userType;
    }

    public static UserDbEnum ConvertUserType(UserProtoEnum userProtoType)
    {
      return userProtoType switch
      {
        UserProtoEnum.Admin => UserDbEnum.Admin,
        UserProtoEnum.User => UserDbEnum.User,
        _ => throw new NotSupportedException()
      };
    }

    public static UserProtoEnum ConvertUserType(UserDbEnum userDbType)
    {
      return userDbType switch
      {
        UserDbEnum.Admin => UserProtoEnum.Admin,
        UserDbEnum.User => UserProtoEnum.User,
        _ => throw new NotSupportedException()
      };
    }

    public static User ConvertUser(UserModel userModel) => new User
    {
      AccountId = userModel.AccountID.ToString(),
      FirstName = userModel.FirstName,
      Id = userModel.ID.ToString(),
      LastName = userModel.LastName,
      PasswordHash = userModel.PasswordHash,
      PasswordSalt = userModel.PasswordSalt,
      Username = userModel.Username,
      UserType = ConvertUserType(userModel.UserType)
    };

    public static UserModel ConvertUser(User userModel) => new UserModel(
      Guid.Parse(userModel.Id),
      userModel.FirstName,
      userModel.LastName,
      userModel.Username,
      userModel.PasswordHash,
      userModel.PasswordSalt,
      Guid.Parse(userModel.AccountId),
      ConvertUserType(userModel.UserType));
  }
}
