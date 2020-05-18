using Grpc.Core;
using GrpcGreeter.Models;
using GrpcGreeter.Protos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.Services
{
  public class UserCRUDService : UserCRUD.UserCRUDBase
  {
    private readonly AppDbContext db = null;
    public UserCRUDService(AppDbContext db)
    {
      this.db = db;
    }

    public override Task<User> GetByID(UserFilter request, ServerCallContext context)
    {
      var data = db.Users.FirstOrDefault(p => p.ID == Guid.Parse(request.Id));
      if (data == null)
        throw new ArgumentNullException();

      var person = new User
      {
        Username = data.Username,
        PasswordHash = data.PasswordHash,
        PasswordSalt = data.PasswordSalt,
        FirstName = data.FirstName,
        LastName = data.LastName,
        Id = data.ID.ToString(),
        Age = data.Age,
        SkillId = data.SkillID.ToString(),
        AccountId = data.AccountID.ToString()
      };
      return Task.FromResult(person);

    }

    public override Task<Users> GetByFilter(UserFilter request, ServerCallContext context)
    {
      UserModel[] users = new UserModel[0];
      if (!string.IsNullOrEmpty(request.FirstName))
        users = db.Users.Where(p => p.FirstName == request.FirstName).ToArray();
      else if (!string.IsNullOrEmpty(request.LastName))
        users = db.Users.Where(p => p.LastName == request.LastName).ToArray();
      else if (!string.IsNullOrEmpty(request.Username))
        users = db.Users.Where(p => p.Username == request.Username).ToArray();
      else if (request.Age != 0)
        users = db.Users.Where(p => p.Age == request.Age).ToArray();

      if (users.Length == 0)
        throw new ArgumentNullException();
      
      var output = new Users();
      var newUsers = users.Select(u => new User
      {
        Username = u.Username,
        PasswordHash = u.PasswordHash,
        PasswordSalt = u.PasswordSalt,
        FirstName = u.FirstName,
        LastName = u.LastName,
        Id = u.ID.ToString(),
        Age = u.Age,
        SkillId = u.SkillID.ToString(),
        AccountId = u.AccountID.ToString(),
        UserType = UserModel.ConvertToUserProtoType(u.UserType)
      });

      output.Items.AddRange(newUsers);

      return Task.FromResult(output);
    }

    public override Task<Users> GetUsers(Empty request, ServerCallContext context)
    {
      var persons = new Users();
      var query = from p in db.Users
                  select new User
                  {
                    Username = p.Username,
                    PasswordHash = p.PasswordHash,
                    PasswordSalt = p.PasswordSalt,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Id = p.ID.ToString(),
                    Age = p.Age,
                    SkillId = p.SkillID.ToString(),
                    AccountId = p.AccountID.ToString(),
                    UserType = UserModel.ConvertToUserProtoType(p.UserType)
                  };
      persons.Items.AddRange(query.ToArray());
      return Task.FromResult(persons);
    }

    public override Task<Empty> Insert(User request, ServerCallContext context)
    {
      db.Users.Add(new UserModel
      {
        Username = request.Username,
        PasswordHash = request.PasswordHash,
        PasswordSalt = request.PasswordSalt,
        FirstName = request.FirstName,
        LastName = request.LastName,
        ID = Guid.Parse(request.Id),
        Age = request.Age,
        SkillID = Guid.Parse(request.SkillId),
        AccountID = Guid.Parse(request.AccountId),
        UserType = UserModel.ConvertToUserDbType(request.UserType)
      });
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }

    public override Task<Empty> Update(User request, ServerCallContext context)
    {
      db.Users.Update(new UserModel
      {
        Username = request.Username,
        PasswordHash = request.PasswordHash,
        PasswordSalt = request.PasswordSalt,
        FirstName = request.FirstName,
        LastName = request.LastName,
        ID = Guid.Parse(request.Id),
        Age = request.Age,
        SkillID = Guid.Parse(request.SkillId),
        AccountID = Guid.Parse(request.AccountId),
        UserType = UserModel.ConvertToUserDbType(request.UserType)
      });
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }

    public override Task<Empty> Delete(UserFilter request, ServerCallContext context)
    {
      var person = db.Users.FirstOrDefault(p => p.ID == Guid.Parse(request.Id));
      if (person == null)
        throw new ArgumentNullException();

      db.Users.Remove(person);
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }
  }
}
