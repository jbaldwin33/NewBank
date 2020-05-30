using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewBankServer.Protos;
using NewBankServer.Services;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewBankServer.Engines;

namespace NewBankServer
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

    private readonly bool isDevelopment;

    public Startup(IWebHostEnvironment env)
    {
      isDevelopment = env.IsDevelopment();
    }
    public void ConfigureServices(IServiceCollection services)
    {
      //services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
      //  .AddCertificate(options =>
      //  {
      //    if (isDevelopment)
      //      options.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck;
      //  });
      services.AddAuthorization();
      services.AddGrpc();

      var dbOptions = new DbContextOptionsBuilder<AppDbContext>();

      services.AddDbContext<AppDbContext>(options => dbOptions.UseSqlServer("data source=.\\SQLEXPRESS; initial catalog=NewBank;integrated security=true"));
      services.AddSingleton<IPollingEngine>(x => new PollingEngine(dbOptions));

      //if server goes down clear out sessions
      using var context = new AppDbContext(dbOptions.Options);
      foreach (var session in context.Sessions)
        context.Sessions.Remove(session);
      context.SaveChanges();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (isDevelopment)
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseRouting();
      //app.UseAuthentication();
      //app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapGrpcService<UserCRUDService>();
        endpoints.MapGrpcService<AccountCRUDService>();
        endpoints.MapGrpcService<AuthenticationService>();
        endpoints.MapGrpcService<CreationService>();
        endpoints.MapGrpcService<SessionService>();
        endpoints.MapGrpcService<TransactionService>();

        endpoints.MapGet("/", async context =>
              {
            await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
          });
      });
    }
  }
}
