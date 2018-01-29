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
        private static string _color = "fff";

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


            void TimerCallback(object x)
            {
                var hub = serviceProvider.GetService<IHubContext<XamarinDayHub>>();
                hub.Clients.All.InvokeAsync("Color", _color);
                _color = _color == "fff" ? "000" : "fff";
            }

            var timer = new Timer(TimerCallback);
            timer.Change(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
