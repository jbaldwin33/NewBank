using Grpc.Core;
using NewBankServer.Models;
using NewBankServer.Protos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NewBankServer.Services
{
  public class UserCRUDService : UserCRUD.UserCRUDBase
  {

    public override Task<UserResponse> GetByCredential(UserCredential request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      if (db.Sessions.FirstOrDefault(s => s.ID == Guid.Parse(request.SessionId)) == null)
        throw new RpcException(new Status(StatusCode.PermissionDenied, "Session is invalid"));

      var user = db.Users.FirstOrDefault(u => u.Username == request.Username && u.PasswordHash == request.PasswordHash);
      if (user == null)
        throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
      return Task.FromResult(new UserResponse { User = UserModel.ConvertUser(user) });
    }
    public override Task<UserResponse> GetByID(UserFilter request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      var data = db.Users.FirstOrDefault(p => p.ID == Guid.Parse(request.Id));
      if (data == null)
        throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
      return Task.FromResult(new UserResponse { User = UserModel.ConvertUser(data) });
    }

    public override Task<Users> GetByFilter(UserFilter request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      UserModel[] users = new UserModel[0];
      if (!string.IsNullOrEmpty(request.FirstName))
        users = db.Users.Where(p => p.FirstName == request.FirstName).ToArray();
      else if (!string.IsNullOrEmpty(request.LastName))
        users = db.Users.Where(p => p.LastName == request.LastName).ToArray();
      else if (!string.IsNullOrEmpty(request.Username))
        users = db.Users.Where(p => p.Username == request.Username).ToArray();

      if (users.Length == 0)
        throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
      
      var output = new Users();
      var newUsers = users.Select(u => UserModel.ConvertUser(u));

      output.Items.AddRange(newUsers.ToArray());

      return Task.FromResult(output);
    }

    public override Task<Users> GetUsers(Empty request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      var persons = new Users();
      var query = from p in db.Users
                  select UserModel.ConvertUser(p);
      persons.Items.AddRange(query.ToArray());
      return Task.FromResult(persons);
    }

    public override Task<Empty> Insert(User request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      db.Users.Add(UserModel.ConvertUser(request));
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }

    public override Task<Empty> Update(User request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      db.Users.Update(UserModel.ConvertUser(request));
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }

    public override Task<Empty> Delete(UserFilter request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      var person = db.Users.FirstOrDefault(p => p.ID == Guid.Parse(request.Id));
      if (person == null)
        throw new RpcException(new Status(StatusCode.InvalidArgument, "User not found"));

      db.Users.Remove(person);
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }
  }
}
