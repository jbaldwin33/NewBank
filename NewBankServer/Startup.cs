using System;
using System.IO;
using System.Threading.Tasks;
using NewBankServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewBankServer.Engines;
using ServerShared;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;

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
      AppDomain.CurrentDomain.UnhandledException += GlobalExceptionHandler.UnhandledExceptionHandler;
      TaskScheduler.UnobservedTaskException += GlobalExceptionHandler.UnobservedTaskExceptionHandler;
    }
    public void ConfigureServices(IServiceCollection services)
    {
      //services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
      //  .AddCertificate(options =>
      //  {
      //    if (isDevelopment)
      //      options.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck;
      //  });
      //services.AddAuthentication();
      services.AddAuthorization();
      services.AddGrpc();



      services.AddDbContext<SqlServerDbContext>();

      services.AddSingleton<IPollingEngine>(x => new PollingEngine());
      services.AddSingleton<ConfigurationModel>();

      //if server goes down clear out sessions
      using var context = new SqlServerDbContext();

      foreach (var session in context.Sessions)
        context.Sessions.Remove(session);
      context.SaveChanges();

      new PollingEngine();
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
