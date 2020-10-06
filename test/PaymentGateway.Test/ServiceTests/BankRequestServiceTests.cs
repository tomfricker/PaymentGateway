using Moq;
using Moq.Protected;
using PaymentGateway.API.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Test.ServiceTests
{
    public class BankRequestServiceTests
    {
        [Fact]
        public async Task PostBankRequestAsyncTest()
        {
            var mockClientFactory = new Mock<IHttpClientFactory>();
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent("{'id':'00000000-0000-0000-0000-000000000000','paymentStatus':0}")
               })
               .Verifiable();

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            mockClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var bankRequestService = new BankRequestService(mockClientFactory.Object);

            var response = await bankRequestService.PostBankRequestAsync(new Core.Models.BankRequest());

            Assert.Equal(new Guid("00000000-0000-0000-0000-000000000000"), response.Id);
            Assert.Equal(0, (int)response.PaymentStatus);
        }
    }
}
