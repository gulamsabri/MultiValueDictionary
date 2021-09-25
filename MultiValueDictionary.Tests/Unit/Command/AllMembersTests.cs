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
    public class AllMembersTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMultiValueDictionary<string, string>> _mockDictionary;

        public AllMembersTests()
        {
            _fixture = new Fixture();
            _mockDictionary = new Mock<IMultiValueDictionary<string, string>>();
        }

        [Fact]
        public void Execute_WhenNoMembers_ThenReturnsExpectedResult()
        {
            var fakeArgs = new[] {"_ignored"};
            _mockDictionary
                .Setup(x => x.AllMembers)
                .Returns(new List<string>());

            var sut = new AllMembers();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(IStringCommand.FormatResult(Constants.Messages.EmptySet));

            _mockDictionary.VerifyAll();
        }

        [Fact]
        public void Execute_WhenMembers_ThenReturnsExpectedResult()
        {
            var fakeArgs = new[] {"_ignored"};
            var fakeMembers = _fixture.CreateMany<string>().ToList();
            var expectedResultData = fakeMembers.Select((mbr, idx) => new {mbr, idx});
            var sb = new StringBuilder();
            foreach (var item in expectedResultData)
                sb.AppendLine($"{item.idx + 1}{Constants.ResponseIndicator}{item.mbr}");

            var expectedResult = sb.ToString();
            _mockDictionary
                .Setup(x => x.AllMembers)
                .Returns(fakeMembers);

            var sut = new AllMembers();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(expectedResult);

            _mockDictionary.VerifyAll();
        }
    }
}