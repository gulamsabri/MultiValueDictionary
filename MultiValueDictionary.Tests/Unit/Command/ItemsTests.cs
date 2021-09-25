using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AutoFixture;
using FluentAssertions;
using Moq;
using MultiValueDictionary.Command;
using Xunit;

namespace MultiValueDictionary.Tests.Unit.Command
{
    public class ItemsTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMultiValueDictionary<string, string>> _mockDictionary;

        public ItemsTests()
        {
            _fixture = new Fixture();
            _mockDictionary = new Mock<IMultiValueDictionary<string, string>>();
        }

        [Fact]
        public void Execute_WhenNoItems_ThenReturnsExpectedResult()
        {
            var fakeArgs = new[] {"_ignored"};
            _mockDictionary
                .Setup(x => x.Items)
                .Returns(new ReadOnlyDictionary<string, ICollection<string>>(
                    new Dictionary<string, ICollection<string>>()));

            var sut = new Items();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(IStringCommand.FormatResult(Constants.Messages.EmptySet));
            _mockDictionary.VerifyAll();
        }

        [Fact]
        public void Execute_WhenItems_ThenReturnsExpectedResult()
        {
            var fakeArgs = new[] {"_ignored"};
            var fakeItems = _fixture.Create<ReadOnlyDictionary<string, ICollection<string>>>();
            var expectedResultData = fakeItems.Select((kvp, idx) => new {kvp, idx});
            var sb = new StringBuilder();
            foreach (var item in expectedResultData)
                sb.AppendLine(
                    $"{item.idx + 1}{Constants.ResponseIndicator}{item.kvp.Key}: {string.Join(" ", item.kvp.Value)}");

            var expectedResult = sb.ToString();
            _mockDictionary
                .Setup(x => x.Items)
                .Returns(fakeItems);

            var sut = new Items();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(expectedResult);
            _mockDictionary.VerifyAll();
        }
    }
}