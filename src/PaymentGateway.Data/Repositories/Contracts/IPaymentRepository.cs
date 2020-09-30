using PaymentGateway.Core.Models;
using PaymentGateway.Data.Models;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Data.Repositories.Contracts
{
    public interface IPaymentRepository
    {
        Task<bool> AddPaymentAsync(Payment payment);

        Task<Payment> GetPaymentAsync(Guid paymentId);

        Task<bool> UpdatePaymentAsync(Payment payment);
    }
}
