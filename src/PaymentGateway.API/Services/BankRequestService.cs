using Newtonsoft.Json;
using PaymentGateway.API.Models;
using PaymentGateway.API.Services.Contracts;
using PaymentGateway.Core.Models;
using PaymentGateway.Mock.BankA.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.API.Services
{
    public class BankRequestService : IBankRequestService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BankRequestService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<BankResponse> PostBankRequestAsync(BankRequest bankRequest)
        {
            var client = _httpClientFactory.CreateClient("BankA");

            var json = JsonConvert.SerializeObject(bankRequest);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/payment", data);

            var responseString = await response.Content.ReadAsStringAsync();
            var bankResponse = JsonConvert.DeserializeObject<BankResponse>(responseString);

            return bankResponse;
        }
    }
}
