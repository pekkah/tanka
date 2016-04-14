namespace Tanka.Web
{
    using Microsoft.AspNetCore.Hosting;
    using System.IO;
    
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                        .UseServer("Microsoft.AspNetCore.Server.Kestrel")
                        .CaptureStartupErrors(captureStartupErrors: true)
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseEnvironment("Development")
                        .UseIISPlatformHandlerUrl()
                        .UseStartup<Startup>()
                        .Build();

            host.Run();
        }
    }
}