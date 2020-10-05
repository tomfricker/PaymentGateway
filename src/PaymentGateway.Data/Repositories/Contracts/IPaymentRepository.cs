using PaymentGateway.Core.Models;
using PaymentGateway.Data.Models;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Data.Repositories.Contracts
{
    public interface IPaymentRepository
    {
        Task AddPaymentAsync(Payment payment);

        Task<Payment> GetPaymentAsync(Guid paymentId);

        Task UpdatePaymentAsync(Payment payment);
    }
}
