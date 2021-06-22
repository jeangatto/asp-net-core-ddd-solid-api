using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SGP.Infrastructure.Context;
using SGP.PublicApi;
using System;
using System.Linq;

namespace SGP.Tests.Factories
{
    public class WebTestApplicationFactory : WebApplicationFactory<Startup>
    {
        private SqliteConnection _connection;
        private readonly string _connectionString = "DataSource=:memory:";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment(Environments.Development);

            builder.ConfigureServices(services =>
            {
                var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<SgpContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                _connection = new SqliteConnection(_connectionString);
                _connection.Open();

                services.AddDbContext<SgpContext>(options => options.UseSqlite(_connection));

                var serviceProvider = services.BuildServiceProvider(true);

                using (var scope = serviceProvider.CreateScope())
                {
                    var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                    var context = scope.ServiceProvider.GetRequiredService<SgpContext>();

                    try
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        context.EnsureSeedDataAsync(loggerFactory).Wait();
                    }
                    catch (Exception ex)
                    {
                        var logger = loggerFactory.CreateLogger<WebTestApplicationFactory>();
                        logger.LogError(ex, "Ocorreu um erro na propagação do banco de dados.");
                        throw;
                    }
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection?.Dispose();
                _connection = null;
            }
            base.Dispose(disposing);
        }
    }
}