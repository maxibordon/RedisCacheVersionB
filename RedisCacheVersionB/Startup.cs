using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisCacheVersionB
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
            services.AddControllers();
             var section = Configuration.GetSection("Redis:Default");
             string _connectionString = section.GetSection("Connection").Value; // cadena de conexión
          //  string _instanceName = section.GetSection("InstanceName").Value; // Nombre de instancia
          //  int _defaultDB = int.Parse(section.GetSection("DefaultDB").Value ?? "0"); // base de datos predeterminada           
          //  services.AddSingleton(new RedisHelper(_connectionString, _instanceName, _defaultDB));
            var multiplexer = ConnectionMultiplexer.Connect(_connectionString);
           
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
