using AutoFixture;
using FluentAssertions;
using Moq;
using MultiValueDictionary.Command;
using Xunit;

namespace MultiValueDictionary.Tests.Unit.Command
{
    public class KeyExistsTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMultiValueDictionary<string, string>> _mockDictionary;

        public KeyExistsTests()
        {
            _fixture = new Fixture();
            _mockDictionary = new Mock<IMultiValueDictionary<string, string>>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Execute_ReturnsExpectedResult(bool underlyingResult)
        {
            var fakeKey = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey};
            _mockDictionary.Setup(x => x.KeyExists(fakeKey))
                .Returns(underlyingResult);

            var sut = new KeyExists();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(IStringCommand.FormatResult(underlyingResult.ToString()));
        }
    }
}