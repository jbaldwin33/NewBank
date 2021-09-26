//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;

//namespace NewBankServer
//{
//  public class DatabaseFactory : IDisposable
//  {

//    private class SqlServerContext : DbContext
//    {

//    }

//    private class SqliteContext : DbContext
//    {

//    }

//    private abstract class Creator
//    {
//      public abstract DbContext FactoryMethod();
//    }

//    private class ConcreteCreatorA : Creator
//    {
//      public override DbContext FactoryMethod() => new SqlServerContext();
//    }

//    private class ConcreteCreatorB : Creator
//    {
//      public override DbContext FactoryMethod() => new SqliteContext();
//    }

//    public void Dispose()
//    {
//      throw new NotImplementedException();
//    }
//  }
//}
