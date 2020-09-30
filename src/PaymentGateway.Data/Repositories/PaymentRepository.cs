using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Models;
using PaymentGateway.Data.Repositories.Contracts;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentsDbContext _dbContext;

        public PaymentRepository(PaymentsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddPaymentAsync(Payment payment)
        {
            await _dbContext.Payments.AddAsync(payment);

            return await _dbContext.SaveChangesAsync() == 1;
        }

        public async Task<Payment> GetPaymentAsync(Guid paymentId)
        {
            return await _dbContext.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);
        }

        public async Task<bool> UpdatePaymentAsync(Payment payment)
        {
            _dbContext.Payments.Update(payment);

            return await _dbContext.SaveChangesAsync() == 1;
        }
    }
}
