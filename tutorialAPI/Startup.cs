using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using tutorialAPI.Data;
using tutorialAPI.Models;
using tutorialAPI.Models.Dtos;

namespace tutorialAPI
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
            services.CustomeDbContext(Configuration).CustomeAutoMapper(Configuration).CustomeSwagger(Configuration);
            

            services.AddControllers();
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

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/Department/swagger.json", "Department");
                options.SwaggerEndpoint("/swagger/Teacher/swagger.json", "Teacher");
                options.RoutePrefix = "";
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }

    static class CustomeMethod
    {
        public static IServiceCollection CustomeDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        public static IServiceCollection CustomeAutoMapper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(options =>
            {
                options.CreateMap<Department, DepartmentCreateDto>().ReverseMap();
                options.CreateMap<Teacher, TeacherCreateDto>().ReverseMap();
            });
            return services;
        }

        public static IServiceCollection CustomeSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("Department", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Department Management",
                    Description = "Simple to manage department system",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "muqhrizmarzuki@gmail.com",
                        Name = "Muqhriz bin Marzuki",
                    },

                });

                options.SwaggerDoc("Teacher", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Teacher Management",
                    Description = "Simple to manage teacher system",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "muqhrizmarzuki@gmail.com",
                        Name = "Muqhriz bin Marzuki",
                    },

                });

                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                options.IncludeXmlComments(xmlCommentsFullPath);
            });
            return services;
        }
        
    }
}
