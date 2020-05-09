using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Middleware;
using Application.Activities;
using Application.Interfaces;
using Domain;
using FluentValidation.AspNetCore;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API
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
      services
          .AddDbContext<DataContext>(opt =>
          {
            opt.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
          });
      services.AddMediatR(typeof(List.Handler).Assembly);
      services
          .AddCors(opt =>
          {
            opt
                .AddPolicy("CorsPolicy",
                policy =>
                {
                  policy
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .WithOrigins("http://localhost:3000");
                });
          });

      services
          .AddMvc()
          .AddFluentValidation(cfg =>
          {
            cfg.RegisterValidatorsFromAssemblyContaining<Create>();
          });
      var builder = services.AddIdentityCore<AppUser>();
      var identityBuilder =
          new IdentityBuilder(builder.UserType, builder.Services);
      identityBuilder.AddEntityFrameworkStores<DataContext>();
      identityBuilder.AddSignInManager<SignInManager<AppUser>>();

      services.AddAuthentication();
      services.AddScoped<IJwtGenerator, JwtGenerator>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseMiddleware<ErrorHandlingMiddleware>();
      if (env.IsDevelopment())
      {
        // app.UseDeveloperExceptionPage();
      }

      // Comment this out for now
      // app.UseHttpsRedirection();
      // Remove https://localhost:5001 for now from launchSettings.json
      app.UseRouting();
      app.UseAuthorization();
      app.UseCors("CorsPolicy");
      app
          .UseEndpoints(endpoints =>
          {
            endpoints.MapControllers();
          });
    }
  }
}
