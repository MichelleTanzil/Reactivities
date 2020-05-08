using System;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var host = CreateHostBuilder(args).Build();

      using (var scope = host.Services.CreateScope())
      { //using calls Dispose() after the using-block is left, even if the code throws an exception.
        var services = scope.ServiceProvider;

        try
        {
          var context = services.GetRequiredService<DataContext>();
          var userManager = services.GetRequiredService<UserManager<AppUser>>();
          context.Database.Migrate(); //checking if the db is existing when we run the app. If not, it will migrate and create one for us.
          Seed.SeedData(context, userManager).Wait();
        }
        catch (Exception ex)
        {
          var logger = services.GetRequiredService<ILogger<Program>>();
          logger.LogError(ex, "An error occured during migration");
        }
      }
      host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder.UseStartup<Startup>();
        });
  }
}