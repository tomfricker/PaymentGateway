using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Extensions;
using PaymentGateway.API.Models;
using PaymentGateway.API.Services.Contracts;
using PaymentGateway.Core.Models;
using PaymentGateway.Data.Models;
using PaymentGateway.Data.Repositories.Contracts;

namespace PaymentGateway.API.Controllers
{
    [Authorize]
    [Route("api/[controller]/{id?}")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBankRequestService _bankRequestService;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IPaymentRepository paymentRepository, IBankRequestService bankRequestService, IMapper mapper, ILogger<PaymentsController> logger)
        {
            _paymentRepository = paymentRepository;
            _bankRequestService = bankRequestService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<PostPaymentResponse> Post([FromBody]PostPaymentRequest request)
        {
            var payment = _mapper.Map<Payment>(request);
            payment.Id = Guid.NewGuid();
            var bankRequest = _mapper.Map<BankRequest>(request);

            var dbUpdate = await _paymentRepository.AddPaymentAsync(payment);

            if (dbUpdate)
            {
                _logger.LogInformation($"POST - Starting request to bank for payment {payment.Id}");

                var bankResponse =  await _bankRequestService.PostBankRequestAsync(bankRequest);

                payment.BankResponseId = bankResponse.Id;
                payment.PaymentStatus = bankResponse.PaymentStatus;

                await _paymentRepository.UpdatePaymentAsync(payment);

                var postPaymentResponse = new PostPaymentResponse { PaymentId = payment.Id };

                return postPaymentResponse;
            }

            _logger.LogError($"POST - Failed to write to database for request ${request.Name} {request.CardNumber.MaskCard()}");
            return new PostPaymentResponse();
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var payment = await _paymentRepository.GetPaymentAsync(id);

            if (payment == null)
            {
                _logger.LogWarning($"GET - Requested payment Id {id} not found");
                return NotFound();
            }

            var response = _mapper.Map<GetPaymentResponse>(payment);
            response.CardNumber = response.CardNumber.MaskCard();

            _logger.LogWarning($"GET - Requested payment Id {id} returned OK");
            return Ok(response);
        }
    }
}
