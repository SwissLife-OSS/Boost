using System;
using System.Collections.Generic;
using System.Text;
using Boost.Certificates;
using Boost.Infrastructure;
using FluentAssertions;
using Xunit;

namespace Boost.Core.Tests
{
    public class KeyRingUserDataProtectorTests
    {
        [Fact]
        public void Protect()
        {
            //Arrange
            var keyRingProtector = new KeyRingUserDataProtector(new List<IDataProtector>
            {
                new CertificateDataProtector(new CertificateManager(), new SymetricEncryption())
            });

            byte[]? plainData = Encoding.UTF8.GetBytes("Foo_Bar_Baz");

            // Act
            byte[]? cipher = keyRingProtector.Protect(plainData);

            byte[]? roundTrip = keyRingProtector.UnProtect(cipher);

            // Assert
            roundTrip.Should().BeEquivalentTo(plainData);
        }

        [Fact]
        public void Protect_LargeData()
        {
            //Arrange
            var keyRingProtector = new KeyRingUserDataProtector(new List<IDataProtector>
            {
                new CertificateDataProtector(new CertificateManager(), new SymetricEncryption())
            });

            byte[]? plainData = new byte[4096];
            new Random().NextBytes(plainData);

            // Act
            byte[]? cipher = keyRingProtector.Protect(plainData);

            byte[]? roundTrip = keyRingProtector.UnProtect(cipher);

            // Assert
            roundTrip.Should().BeEquivalentTo(plainData);
        }

        [Fact]
        public void ProtectString()
        {
            //Arrange
            var keyRingProtector = new KeyRingUserDataProtector(new List<IDataProtector>
            {
                new CertificateDataProtector(new CertificateManager(), new SymetricEncryption())
            });

            string plainText = "Foo_Bar_Baz";

            // Act
            string cipherText = keyRingProtector.Protect(plainText);

            string roundTrip = keyRingProtector.UnProtect(cipherText);

            // Assert
            roundTrip.Should().BeEquivalentTo(plainText);
        }
    }
}
