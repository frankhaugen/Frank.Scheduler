using CronQuery.Mvc.DependencyInjection;
using Frank.Scheduler.Api.Jobs;
using Frank.Scheduler.Api.ServiceBus;
using Frank.Scheduler.Data;
using Frank.Scheduler.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Frank.Scheduler.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Scheduled jobs
            services.AddCronQuery(Configuration.GetSection("CronQuery"));
            services.AddScoped<SchedulerJob>();

            // Service bus listeners
            services.AddHostedService<ApplicationRegistrationConsumer>();
            services.AddHostedService<ScheduledTaskCallbackConsumer>();

            // Services
            services.AddScoped<IScheduledTaskService, ScheduledTaskService>();
            services.AddScoped<IServiceBusService, ServiceBusService>();

            // Service bus
            services.Configure<ServiceBusConfiguration>(Configuration.GetSection(nameof(ServiceBusConfiguration)));

            // Database
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlDatabase"));
#if DEBUG
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
#endif
            });

            // MVC
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Frank.Scheduler.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Frank.Scheduler.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
