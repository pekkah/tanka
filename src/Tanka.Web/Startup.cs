namespace Tanka.Web
{
    using System.Linq;
    using Infrastructure;
    using Microsoft.AspNet.Authentication;
    using Microsoft.AspNet.Authentication.Cookies;
    using Microsoft.AspNet.Builder;
    using Microsoft.AspNet.Hosting;
    using Microsoft.AspNet.Mvc;
    using Microsoft.Framework.Configuration;
    using Microsoft.Framework.DependencyInjection;
    using Microsoft.Framework.Logging;
    using Microsoft.Framework.Runtime;
    using Newtonsoft.Json.Converters;

    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            // Setup configuration sources.

            var builder = new ConfigurationBuilder(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            Config.GetValueFunc = Configuration.Get;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Entity Framework services to the services container.
            //services.AddEntityFramework()
            //  .AddSqlServer()
            //.AddDbContext<ApplicationDbContext>(options =>
            //  options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            // Add Identity services to the services container.
            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //  .AddEntityFrameworkStores<ApplicationDbContext>()
            //.AddDefaultTokenProviders();

            // Configure the options for the authentication middleware.
            // You can add options for Google, Twitter and other middleware as shown below.
            // For more information see http://go.microsoft.com/fwlink/?LinkID=532715


            // Add MVC services to the services container
            services.AddRaven(Configuration["Data:RavenDb"]);
            services.AddMvc().Configure<MvcOptions>(options =>
            {
                var json = options.OutputFormatters.OfType<JsonOutputFormatter>().First();

                json.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            services.AddAuthentication();
            services.Configure<ExternalAuthenticationOptions>(options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            services.Configure<CookieAuthenticationOptions>(options =>
            {
                options.AutomaticAuthentication = true;
                options.LoginPath = "/admin/login";
                options.LogoutPath = "/admin/logout";
            });

            services.ConfigureOpenIdConnectAuthentication(options =>
            {
                options.AutomaticAuthentication = true;
                options.ClientId = Configuration.Get("Security:ClientId");
                options.Authority = Configuration.Get("Security:Authority");
                options.RedirectUri = Configuration.Get("Security:RedirectUri");
            });
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();

            // Configure the HTTP request pipeline.

            // Add the following to the request pipeline only in development environment.
            if (env.IsDevelopment())
            {
                app.UseErrorPage();
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // sends the request to the following path or controller action.
                app.UseErrorHandler("/error");
            }

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add cookie-based authentication to the request pipeline.
            // app.UseIdentity();

            // Add authentication middleware to the request pipeline. You can configure options such as Id and Secret in the ConfigureServices method.
            // For more information see http://go.microsoft.com/fwlink/?LinkID=532715
            // app.UseFacebookAuthentication();
            // app.UseGoogleAuthentication();
            // app.UseMicrosoftAccountAuthentication();
            // app.UseTwitterAuthentication();

            app.UseCookieAuthentication();
            app.UseOpenIdConnectAuthentication();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
            });
        }
    }
}
