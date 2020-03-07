using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Funq;
using ServiceStack;
using Microsoft.Extensions.Configuration;
using ServiceStack.Web;
using ServiceStack.Auth;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using ServiceStack.OrmLite;
using MySql.Data.MySqlClient;
using ServiceStack.Data;

namespace SS
{
    public class Program
    {
        // Comment
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     config.SetBasePath(Directory.GetCurrentDirectory());
                     config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                 })
                .UseKestrel(options =>
                {
                    options.Limits.MaxConcurrentConnections = 1000;
                    options.Limits.MaxConcurrentUpgradedConnections = 1000;
                    options.Limits.MaxRequestBodySize = 100 * 1024;
                    options.Limits.MinRequestBodyDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    options.Limits.MinResponseDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
           

            host.Run();
        }
    }

    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var settings = new NetCoreAppSettings(Configuration);

            //TraceManager.Instance.Log(System.Diagnostics.TraceLevel.Info, "Connect.txt", "Connect - Begin Config");

            app.UseServiceStack(new SSSelfHost
            {
                AppSettings = settings
            });

            app.Run(context =>
            {
                context.Response.Redirect("/metadata");
                return Task.FromResult(0);
            });
        }

    }

    public static class SSHostHelper
    {
        public static void Configure(Funq.Container container, ServiceStackHost host)
        {
            host.SetConfig(new HostConfig { UseCamelCase = false, DefaultContentType = MimeTypes.Json });
            //TraceManager.Instance.Log(System.Diagnostics.TraceLevel.Info, "Connect.txt", "Connect - Begin Config");

            host.Plugins.Add(new CorsFeature(allowedOrigins: "*",
                allowedMethods: "GET, POST, PUT, DELETE, OPTIONS",
                allowedHeaders: "Authorization, Content-Type",
                allowCredentials: false));

            OrmLiteConnectionFactory dbFactory = null;

            var connectionMaster = "";
            try
            {
                connectionMaster = "server=localhost; database=localhost; User Id=root; password=abcd; SslMode=None";
            }
            catch (Exception ex)
            {

            }

            if (connectionMaster != null)
            {
                connectionMaster = connectionMaster;
                dbFactory = new OrmLiteConnectionFactory(
                    connectionMaster,
                    ServiceStack.OrmLite.MySqlDialect.Provider);

                dbFactory.RegisterConnection("Master", connectionMaster, ServiceStack.OrmLite.MySqlDialect.Provider);
            }

            container.Register<IDbConnectionFactory>(c => dbFactory);

            //{ HtmlRedirect = null });
        }
    }

    public class SSSelfHost : AppHostBase
    {
        public NetCoreAppSettings AppSettings { get; internal set; }

        public SSSelfHost()
            : base("SS API", typeof(SS.ServiceInterface.SS).Assembly)
        {

        }

        //public override void OnSaveSession(IRequest httpReq, IAuthSession session, TimeSpan? expiresIn = null)
        //{
        //}

        public override void Configure(Container container)
        {

            // Special case, as Account Service needs to register the CustomCredentialsAuthProvider
            //base.ConfigureWithoutAuth(container);

            SSHostHelper.Configure(container, this);


        }
    }
}