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
            CreateMap<PostPaymentRequest, BankRequest>();

            CreateMap<BankResponse, PostPaymentResponse>();

            CreateMap<PostPaymentRequest, Payment>()
                .ForMember(x => x.CardNumber, opt => opt.Ignore());

            CreateMap<Payment, GetPaymentResponse>()
                .ForMember(x => x.CardNumber, opt => opt.Ignore())
                .ForMember(x => x.PaymentStatus, opt => opt.Ignore());
        }
    }
}
