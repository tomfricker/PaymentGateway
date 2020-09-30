using AutoMapper;
using PaymentGateway.Core.Models;
using PaymentGateway.Data.Models;
using PaymentGateway.Mock.BankA.Models;

namespace PaymentGateway.API.Models.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<PostPaymentRequest, Payment>();
            CreateMap<PostPaymentRequest, BankRequest>();
            CreateMap<BankResponse, PostPaymentResponse>();
            CreateMap<Payment, GetPaymentResponse>();
        }
    }
}
