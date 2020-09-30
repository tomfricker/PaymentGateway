using PaymentGateway.Core.Models;
using PaymentGateway.Mock.BankA.Models;
using System.Threading.Tasks;

namespace PaymentGateway.API.Services.Contracts
{
    public interface IBankRequestService
    {
        Task<BankResponse> PostBankRequestAsync(BankRequest request);
    }
}
