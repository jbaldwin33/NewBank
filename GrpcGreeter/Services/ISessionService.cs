using GrpcGreeter.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcGreeter.Services
{
  public interface ISessionService
  {
    SessionProperties SessionProperties { get; }
    void UpdateCurrentUser(UserModel user);
  }
}
