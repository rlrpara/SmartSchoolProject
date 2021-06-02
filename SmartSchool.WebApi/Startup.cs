using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SmartSchool.Domain.interfaces.IRepositories;
using SmartSchool.Domain.interfaces.IServices;
using SmartSchool.Infra.Data.Repositories;
using SmartSchool.Service.Services;
using System;
using System.IO;
using System.Reflection;

namespace SmartSchool.WebApi
{
    public class Startup
    {
        private readonly string _baseUrl = "http://www.rlrsistemas.com.br";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped<IBaseService, BaseService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SmartSchool.WebApi",
                    Version = "v1",
                    TermsOfService = new Uri(_baseUrl),
                    Description = "Descrição da webapi do Pague Pouco",
                    License = new OpenApiLicense
                    {
                        Name = "PaguePouco Licence",
                        Url = new Uri(_baseUrl)
                    },
                    Contact = new OpenApiContact
                    {
                        Name = "Rodrigo de L. Ribeiro",
                        Email = "rlr.para@gmail.com",
                        Url = new Uri(_baseUrl)
                    }
                });

                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartSchool.WebApi v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
