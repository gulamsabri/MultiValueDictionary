using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using Moq;
using MultiValueDictionary.Command;
using Xunit;

namespace MultiValueDictionary.Tests.Unit.Command
{
    public class RemoveAllTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMultiValueDictionary<string, string>> _mockDictionary;

        public RemoveAllTests()
        {
            _fixture = new Fixture();
            _mockDictionary = new Mock<IMultiValueDictionary<string, string>>();
        }

        [Fact]
        public void Execute_WhenKeyValid_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey};

            var sut = new RemoveAll();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(IStringCommand.FormatResult(Constants.Messages.Removed));
        }

        [Fact]
        public void Execute_WhenKeyNotFoundException_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey};
            _mockDictionary
                .Setup(x => x.RemoveAll(fakeKey))
                .Throws<KeyNotFoundException>();

            var sut = new RemoveAll();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(IStringCommand.FormatResult(Constants.Messages.KeyNotFound));

            _mockDictionary.VerifyAll();
        }
    }
}