using GrpcGreeter.Protos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GrpcGreeter.Models
{
  public enum UserDbType { User, Admin }
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
    public UserDbType UserType { get; set; }

    public static UserDbType ConvertToUserDbType(UserProtoType userProtoType)
    {
      return userProtoType switch
      {
        UserProtoType.Admin => UserDbType.Admin,
        UserProtoType.User => UserDbType.User,
        _ => throw new NotSupportedException()
      };
    }

    public static UserProtoType ConvertToUserProtoType(UserDbType userDbType)
    {
      return userDbType switch
      {
        UserDbType.Admin => UserProtoType.Admin,
        UserDbType.User => UserProtoType.User,
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
      UserType = ConvertToUserProtoType(userModel.UserType)
    };
  }
}
