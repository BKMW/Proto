using Application.Tools;
using FluentAssertions;

namespace Application.Test
{
    public class OperationsTests
    {
        [Fact]
        public void Operations_MaskWallet_ReturnString()
        {
            //Arrange- Vaiiables, classes, mocks

           // TextOperations operations = new TextOperations();
            var wallet = "1231231231231238123";
            var maskWallet = "123123**********123";
            //Act

            var result = TextOperations.MaskWallet(wallet);
            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().Be(maskWallet);

        }

        [Theory]
        [InlineData("1231231231231238123", "123123**********123")]
        [InlineData("1231231231231238125", "123123**********125")]

        public void Operations_MaskWallet_ReturnMask(string wallet, string maskWallet)
        {
            //Arrange- Vaiiables, classes, mocks

            // TextOperations operations = new TextOperations();

            //Act

            var result = TextOperations.MaskWallet(wallet);
            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().Be(maskWallet);

        }

    }
}