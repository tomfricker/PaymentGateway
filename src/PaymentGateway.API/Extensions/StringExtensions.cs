using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.API.Extensions
{
    public static class StringExtensions
    {
        public static string MaskCard(this string str)
        {
            var xLength = str.Length - 4;
            var mask = new String('*', xLength);

            return $"{mask}{str.Substring(xLength)}";
        }
    }
}
