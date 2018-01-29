using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace Xamarinday2018.Azure
{
    public class Startup
    {
        private static readonly string[] Colors = { "fff", "000", "f00" };

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerfactory, IApplicationLifetime applicationLifetime)
        {
            app.UseStaticFiles();
            app.UseSignalR(routes =>
            {
                routes.MapHub<XamarinDayHub>("hubs");
            });

            var hub = serviceProvider.GetService<IHubContext<XamarinDayHub>>();
            void TimerCallback(object x)
            {
                var random = new Random();
                int index = random.Next(Colors.Length);
                var color = Colors[index];
                hub.Clients.All.InvokeAsync("Color", color);
            }

            var timer = new Timer(TimerCallback);
            timer.Change(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello XamarinDay!");
            });
        }
    }
}
