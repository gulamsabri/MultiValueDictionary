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
    public class MembersTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMultiValueDictionary<string, string>> _mockDictionary;

        public MembersTests()
        {
            _fixture = new Fixture();
            _mockDictionary = new Mock<IMultiValueDictionary<string, string>>();
        }

        [Fact]
        public void Execute_WhenNoMembers_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey};
            _mockDictionary
                .Setup(x => x.Members(fakeKey))
                .Throws<KeyNotFoundException>();

            var sut = new Members();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(IStringCommand.FormatResult(Constants.Messages.KeyNotFound));

            _mockDictionary.VerifyAll();
        }

        [Fact]
        public void Execute_WhenMembers_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey};
            var fakeMembers = _fixture.CreateMany<string>().ToList();
            var expectedResultData = fakeMembers.Select((mbr, idx) => new {mbr, idx});
            var sb = new StringBuilder();
            foreach (var item in expectedResultData)
                sb.AppendLine($"{item.idx + 1}{Constants.ResponseIndicator}{item.mbr}");

            var expectedResult = sb.ToString();
            _mockDictionary
                .Setup(x => x.Members(fakeKey))
                .Returns(fakeMembers);

            var sut = new Members();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(expectedResult);

            _mockDictionary.VerifyAll();
        }
    }
}