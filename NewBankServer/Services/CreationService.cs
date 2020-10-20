using Grpc.Core;
using NewBankServer.Models;
using NewBankServer.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace NewBankServer.Services
{
  public class CreationService : Creation.CreationBase
  {
    public override Task<SignUpResponse> SignUp(SignUpRequest request, ServerCallContext context)
    {
      using var db = new AppDbContext();
      if (request.User == null)
        throw new RpcException(new Status(StatusCode.InvalidArgument, nameof(request.User)));
      if (request.Account == null)
        throw new RpcException(new Status(StatusCode.InvalidArgument, nameof(request.Account)));
      if (!request.User.Id.Equals(request.Account.UserId) || !request.Account.Id.Equals(request.User.AccountId))
        throw new RpcException(new Status(StatusCode.Aborted, "User is not associated with this account"));
      if (Guid.Parse(request.User.Id) == Guid.Empty ||
        Guid.Parse(request.User.AccountId) == Guid.Empty ||
        Guid.Parse(request.Account.Id) == Guid.Empty ||
        Guid.Parse(request.Account.UserId) == Guid.Empty)
        throw new RpcException(new Status(StatusCode.Aborted, "ID cannot be empty"));

      db.Users.Add(UserModel.ConvertUser(request.User));
      db.Accounts.Add(AccountModel.ConvertAccount(request.Account));
      db.SaveChanges();
      return Task.FromResult(new SignUpResponse());
    }

    public override Task<CreateSaltResponse> CreatePasswordSalt(Empty request, ServerCallContext context)
    {
      byte[] saltBytes = new byte[16];
      RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
      rng.GetBytes(saltBytes);

      var salt = Convert.ToBase64String(saltBytes);
      return Task.FromResult(new CreateSaltResponse { ServerSalt = salt });
    }
  }
}
