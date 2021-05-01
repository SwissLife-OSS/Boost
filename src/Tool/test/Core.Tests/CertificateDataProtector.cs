using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boost.Infrastructure;
using FluentAssertions;
using Xunit;

namespace Boost.Core.Tests
{
    public class CertificateDataProtectorTests
    {
        [Fact]
        public void ProtectAndUnProtect_ShouldBeEquivalent()
        {
            //// Arrange
            //var keyContext = new KeyContext(Guid.NewGuid(), new Dictionary<string, string>());

            //var protector = new CertificateDataProtector();
            //var plainData = Encoding.UTF8.GetBytes("Foo_Bar_Baz");

            //// Act
            //var cipherData = protector.Protect(plainData);

            //var roundTrip = protector.UnProtect(cipherData);

            //// Assert
            //roundTrip.Should().BeEquivalentTo(plainData);
        }
    }
}
