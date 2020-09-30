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

        public PaymentsController(IPaymentRepository paymentRepository, IBankRequestService bankRequestService, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _bankRequestService = bankRequestService;
            _mapper = mapper;
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
                var bankResponse =  await _bankRequestService.PostBankRequestAsync(bankRequest);

                payment.BankResponseId = bankResponse.Id;
                payment.PaymentStatus = bankResponse.PaymentStatus;

                await _paymentRepository.UpdatePaymentAsync(payment);

                var postPaymentResponse = new PostPaymentResponse { PaymentId = payment.Id };

                return postPaymentResponse;
            }

            return new PostPaymentResponse();
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var payment = await _paymentRepository.GetPaymentAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<GetPaymentResponse>(payment);
            response.CardNumber = response.CardNumber.MaskCard();

            return Ok(response);
        }
    }
}
