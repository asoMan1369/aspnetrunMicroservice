using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Extensions
{
    public static class HostEtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host,int? retry = 0)
        {
            int retryForAvailablity = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating postresql starting ...");
                    using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSetting:ConnectionString"));
                    connection.Open();

                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };

                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"Create table Coupon(Id serial Primary Key,
                                                                ProductName varchar(24) Not null,
                                                                Description TEXT,
                                                                Amount INT)";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductName,Description,Amount) VALUES('IPhone x','IPhone Discount' , 150)";
                    command.CommandText = "INSERT INTO Coupon(ProductName,Description,Amount) VALUES('Samsung 10','Samsung Discount' , 100)";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Migrated postresql database.");
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occured while migrating the postreql database");
                    if (retryForAvailablity < 50)
                    {
                        retryForAvailablity++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailablity);
                    }
                }
            }

            return host;
        }
    }
}
