using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameApp.API.Data;
using GameApp.API.Helpers;
using GameApp.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GameApp.API
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
            // postgresql db connection
            services.AddEntityFrameworkNpgsql().AddDbContext<MyWebApiContext>(opt => opt.UseNpgsql(
            Configuration.GetConnectionString("MyWebApiConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opt =>
                {
                    // ingore reference loop
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });


            services.AddCors();
            // NuGet package: AutoMapper.Extensions.Microsoft.DependencyInjection
            services.AddAutoMapper();
            services.AddTransient<Seed>();
            // add service to this app
            /*
                Three types: 
                    Singleton: only have one instance and shared in the whole program, but it gets trouble 
                        for multi-concurrent-requests
                    Transient: Create one instance everytime it was called. Light weight but when request is
                        very often, then it waste much time
                    Scoped:
                        Create an instance for every kind of request not every request. And this instance 
                        will be used for this kind of request all along.                      
            */
            // first generic is interface and second is the concrete implementation
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                /*
                    Understand the lambda function:
                        Lambda expression is an anomyous method, which is a short way to
                        write a delegate. Delegate is a function pointer.
                        So when can we use lambda? One argument of this function is a 
                        delegate(function pointer).
                        UseExceptionHandler(this app, Action<>), first arugment is this 
                        which we don't need to write. Second optional parameter is an 
                        action, its definition is a delegate : public delegate void Action<in T>(T obj);
                        . So that is why we can use a lambda here which must take one argument is app in
                        this case, and also return type is void.
                 */

                /*
                    public static void Run(this IApplicationBuilder app, RequestDelegate handler);
                    public delegate Task RequestDelegate(HttpContext context);
                    definition of Run function takes this as an argument, and takes a delegate
                    as second parameter. This delegate should take in an HttpContext object and
                    then return a Task
                 */

                Console.WriteLine("here this is production");
                app.UseExceptionHandler(builder => {
                    builder.Run(async context =>
                    {
                        Console.WriteLine("got an 500 error");
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            // cors strategy: only allow request from some certain domains or simple allow all
            // and the order matters, have to use cors before mvc

            // method below is for inserting some data into database
            //seeder.SeedUsers();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseMvc();

        }
    }
}
