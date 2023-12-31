using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDBExample.Models;
using Microsoft.AspNetCore;
using EAD_Backend.JWTAuthentication;
using EAD_Backend.Services;

namespace EAD_Backend
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
            services.Configure<MongoDBSettings>(Configuration.GetSection("MongoDB"));
            services.AddSingleton<UserService>();  // Register the UserService
            services.AddSingleton< TrainService>();
            services.AddSingleton< TrainScheduleService>();
            services.AddSingleton< ReservationService>();
            services.AddSingleton<TicketService>(); // Register the TicketService
            //services.AddSingleton<ITokenService, JwtAuthenticationService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EADBackend", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                        .WithOrigins("http://localhost:3000") // Add your frontend URL here
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials(); // Allow credentials (cookies, headers) to be sent cross-origin if needed
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EADBackend v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
           .UseUrls()
           .UseKestrel()
           .UseStartup<Startup>();

    }
}
