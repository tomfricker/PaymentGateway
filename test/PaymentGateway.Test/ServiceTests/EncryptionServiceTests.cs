using Microsoft.Extensions.Configuration;
using Moq;
using PaymentGateway.API.Services;
using System;
using Xunit;

namespace PaymentGateway.Test.ServiceTests
{
    public class EncryptionServiceTests
    {
        [Fact]
        public void EncryptDecryptTest()
        {
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x["Key"]).Returns("5678\\#t7N3TVNs12");
            mockConfig.Setup(x => x["Iv"]).Returns("1234\\Eup3==dZM34");

            var encryptionService = new AesEncryptionService(mockConfig.Object);

            var cardNumber = "4111111111111111";
            var encrypted = encryptionService.Encrypt(cardNumber);
            var decrypted = encryptionService.Decrypt(encrypted);

            Assert.Equal(cardNumber, decrypted);
        }

        [Fact]
        public void KeyNot16Characters_ThrowsArgumentOutOfRangeException()
        {
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x["Key"]).Returns("5678\\#t7N3TV");
            mockConfig.Setup(x => x["Iv"]).Returns("1234\\Eup3==dZM34");

            Assert.Throws<ArgumentOutOfRangeException>(() => new AesEncryptionService(mockConfig.Object));
        }

        [Fact]
        public void IvNot16Characters_ThrowsArgumentOutOfRangeException()
        {
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x["Key"]).Returns("5678\\#t7N3TVNs12");
            mockConfig.Setup(x => x["Iv"]).Returns("1234\\Eup3==dZM34123");

            Assert.Throws<ArgumentOutOfRangeException>(() => new AesEncryptionService(mockConfig.Object));
        }
    }
}
