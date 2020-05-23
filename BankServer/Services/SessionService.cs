//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace BankServer.Services
//{
//  public class SessionService : ISessionService
//  {
//    private static readonly Lazy<SessionService> instance = new Lazy<SessionService>(() => new SessionService(Guid.NewGuid()));
//    private readonly List<Session> sessionRepository = new List<Session>();
//    public Guid ID { get; }

//    public SessionService(Guid id)
//    {
//      ID = id;
//    }

//    public static SessionService Instance => instance.Value;

//    public void AddSession(Session session)
//    {
//      sessionRepository.Add(session);
//    }
//    public void RemoveSession(Guid sessionID)
//    {
//      sessionRepository.RemoveAll(s => s.SessionID == sessionID);
//    }

//    public bool IsValidSession(Guid id) => sessionRepository.Any(s => s.SessionID == id);

//    public int ActiveSessions() => sessionRepository.Count;
//  }

//  public class Session
//  {
//    public Guid SessionID { get; }

//    public Session(Guid sessionID)
//    {
//      SessionID = sessionID;
//    }
//  }
//}
