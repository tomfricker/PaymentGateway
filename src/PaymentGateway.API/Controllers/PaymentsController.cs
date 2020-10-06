using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Extensions;
using PaymentGateway.API.Models;
using PaymentGateway.API.Services.Contracts;
using PaymentGateway.Core.Enums;
using PaymentGateway.Core.Models;
using PaymentGateway.Data.Models;
using PaymentGateway.Data.Repositories.Contracts;

namespace PaymentGateway.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBankRequestService _bankRequestService;
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IPaymentRepository paymentRepository, 
            IBankRequestService bankRequestService,
            IEncryptionService encryptionService,
            IMapper mapper, 
            ILogger<PaymentsController> logger)
        {
            _paymentRepository = paymentRepository;
            _bankRequestService = bankRequestService;
            _encryptionService = encryptionService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PostPaymentRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var paymentId = Guid.NewGuid();

            try
            {
                var payment = _mapper.Map<Payment>(request);
                payment.Id = paymentId;
                payment.CardNumber = _encryptionService.Encrypt(request.CardNumber);
                var bankRequest = _mapper.Map<BankRequest>(request);

                await _paymentRepository.AddPaymentAsync(payment);

                _logger.LogInformation($"POST - Starting request to bank for payment {payment.Id}");

                var bankResponse = await _bankRequestService.PostBankRequestAsync(bankRequest);

                payment.BankResponseId = bankResponse.Id;
                payment.PaymentStatus = bankResponse.PaymentStatus;                

                await _paymentRepository.UpdatePaymentAsync(payment);

                var postPaymentResponse = new PostPaymentResponse { PaymentId = payment.Id };

                return Ok(postPaymentResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"POST - Failed due to exception ${ex.GetType()} with error ${ex.Message} for request ${paymentId}");
                return StatusCode(500);
            }            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var payment = await _paymentRepository.GetPaymentAsync(id);

                if (payment == null)
                {
                    _logger.LogWarning($"GET - Requested payment Id {id} not found");
                    return NotFound();
                }

                var response = _mapper.Map<GetPaymentResponse>(payment);
                response.CardNumber = _encryptionService.Decrypt(payment.CardNumber).MaskCard();
                response.PaymentStatus = Enum.GetName(typeof(PaymentStatusCode), payment.PaymentStatus);

                _logger.LogInformation($"GET - Requested payment Id {id} returned OK");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GET - Failed due to exception ${ex.GetType()} with error ${ex.Message} for request on ID ${id}");
                return StatusCode(500);
            }
        }
    }
}
