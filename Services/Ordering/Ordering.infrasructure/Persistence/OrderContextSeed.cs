using Microsoft.Extensions.Logging;
using Ordering.Doman.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.infrasructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext,ILogger<OrderContext> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreConfigureOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext));
            }
        }

        private static IEnumerable<Order> GetPreConfigureOrders()
        {
            return new List<Order>
            {
                new Order()
                {
                    UserName = "swn",
                    FirstName="Mehmet",
                    LastName = "Ozkaya",
                    EmailAddress ="ezozkme@gmail.com",
                    AddressLine="Bahceliveler",
                    Country="Turkey",
                    TotalPrice = 350
                }
            };
        }
    }

}
