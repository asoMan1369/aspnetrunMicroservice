using Microsoft.Extensions.Configuration;
using Ordering.Application.Contracts.Persistance;
using Microsoft.EntityFrameworkCore.SqlServer;
using Ordering.infrasructure.Persistence;
using Ordering.infrasructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Models;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.infrasructure.Mail;
using Microsoft.EntityFrameworkCore;

namespace Ordering.infrasructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<OrderContext>(options =>
                     options.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString")));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();
            return services;
        }
    }
}
