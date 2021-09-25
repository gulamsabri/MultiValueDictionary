using FluentAssertions;
using Moq;
using MultiValueDictionary.Command;
using Xunit;

namespace MultiValueDictionary.Tests.Unit.Command
{
    public class ClearTests
    {
        private readonly Mock<IMultiValueDictionary<string, string>> _mockDictionary;

        public ClearTests()
        {
            _mockDictionary = new Mock<IMultiValueDictionary<string, string>>();
        }

        [Fact]
        public void Clear_ReturnsExpectedResult()
        {
            var fakeArgs = new[] {"_ignored"};

            var sut = new Clear();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(IStringCommand.FormatResult(Constants.Messages.Cleared));
        }
    }
}