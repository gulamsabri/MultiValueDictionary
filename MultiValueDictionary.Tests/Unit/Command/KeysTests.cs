using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;
using FluentAssertions;
using Moq;
using MultiValueDictionary.Command;
using Xunit;

namespace MultiValueDictionary.Tests.Unit.Command
{
    public class KeysTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMultiValueDictionary<string, string>> _mockDictionary;

        public KeysTests()
        {
            _fixture = new Fixture();
            _mockDictionary = new Mock<IMultiValueDictionary<string, string>>();
        }

        [Fact]
        public void Execute_WhenNoKeys_ThenReturnsExpectedResult()
        {
            var fakeArgs = new[] {"_ignored"};
            _mockDictionary
                .Setup(x => x.Keys)
                .Returns(new List<string>());

            var sut = new Keys();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(IStringCommand.FormatResult(Constants.Messages.EmptySet));
            _mockDictionary.VerifyAll();
        }

        [Fact]
        public void Execute_WhenKeys_ThenReturnsExpectedResult()
        {
            var fakeArgs = new[] {"_ignored"};
            var fakeKeys = _fixture.CreateMany<string>().ToList();
            var expectedResultData = fakeKeys.Select((mbr, idx) => new {mbr, idx});
            var sb = new StringBuilder();
            foreach (var item in expectedResultData)
                sb.AppendLine($"{item.idx + 1}{Constants.ResponseIndicator}{item.mbr}");

            var expectedResult = sb.ToString();
            _mockDictionary
                .Setup(x => x.Keys)
                .Returns(fakeKeys);

            var sut = new Keys();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(expectedResult);
            _mockDictionary.VerifyAll();
        }
    }
}