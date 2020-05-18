using GrpcGreeter.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcGreeter.Services
{
  public class SessionService : ISessionService
  {
    private SessionProperties sessionProperties;
    public SessionProperties SessionProperties
    {
      get => sessionProperties;
      set => sessionProperties = value;
    }

    public void UpdateCurrentUser(UserModel user)
    {
      SessionProperties.CurrentUser = user;
    }

    public SessionService()
    {
      SessionProperties = new SessionProperties { SessionID = Guid.NewGuid() };
    }
  }

  public class SessionProperties
  {
    public Guid SessionID { get; internal set; }

    public UserModel CurrentUser { get; internal set; }
  }
}
