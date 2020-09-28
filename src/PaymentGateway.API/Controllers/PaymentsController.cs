using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Models;

namespace PaymentGateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        [HttpPost]
        public string Post([FromBody]PaymentRequest request)
        {
            return "5678";
        }

        [HttpGet]
        public string Get(string paymentId)
        {
            return "1234";
        }
    }
}
