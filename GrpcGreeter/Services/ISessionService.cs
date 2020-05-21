﻿using GrpcGreeter.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcGreeter.Services
{
  public interface ISessionService
  {
    void AddSession(Session session);
    void RemoveSession(Guid sessionID);
    bool IsValidSession(Guid id);
    int ActiveSessions();
  }
}
