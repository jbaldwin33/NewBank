using GrpcGreeter.Protos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GrpcGreeter.Models
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
    public int Age { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public Guid SkillID { get; set; }
    public Guid AccountID { get; set; }
    public UserDbEnum UserType { get; set; }

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
      Age = userModel.Age,
      FirstName = userModel.FirstName,
      Id = userModel.ID.ToString(),
      LastName = userModel.LastName,
      PasswordHash = userModel.PasswordHash,
      PasswordSalt = userModel.PasswordSalt,
      Username = userModel.Username,
      UserType = ConvertUserType(userModel.UserType)
    };

    public static UserModel ConvertUser(User userModel) => new UserModel
    {
      AccountID = Guid.Parse(userModel.AccountId),
      Age = userModel.Age,
      FirstName = userModel.FirstName,
      ID = Guid.Parse(userModel.Id),
      LastName = userModel.LastName,
      PasswordHash = userModel.PasswordHash,
      PasswordSalt = userModel.PasswordSalt,
      Username = userModel.Username,
      UserType = ConvertUserType(userModel.UserType)
    };
  }
}
