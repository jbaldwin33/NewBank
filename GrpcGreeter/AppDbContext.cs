using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrpcGreeter.Models;
using GrpcGreeter.Services;

namespace GrpcGreeter
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {
      var c = 1;
    }

    public DbSet<UserModel> Users { get; set; }
    public DbSet<AccountModel> Accounts { get; set; }
    public DbSet<SessionModel> Sessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;initial catalog=NewBank;Trusted_Connection=true;");
    }
  }
}
