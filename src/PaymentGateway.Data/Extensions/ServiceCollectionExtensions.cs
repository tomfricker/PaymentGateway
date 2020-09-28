using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Data.Repositories;
using PaymentGateway.Data.Repositories.Contracts;

namespace PaymentGateway.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddData(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<PaymentsDbContext>(options => options.UseSqlServer(connectionString));

            services.AddTransient<IPaymentRepository, PaymentRepository>();
        }
    }
}
