using AutoFixture;
using FluentAssertions;
using Moq;
using MultiValueDictionary.Command;
using Xunit;

namespace MultiValueDictionary.Tests.Unit.Command
{
    public class AddTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMultiValueDictionary<string, string>> _mockDictionary;

        public AddTests()
        {
            _fixture = new Fixture();
            _mockDictionary = new Mock<IMultiValueDictionary<string, string>>();
        }

        [Fact]
        public void Execute_WhenMemberExists_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var fakeMessage = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey, fakeMember};
            _mockDictionary
                .Setup(x => x.Add(fakeKey, fakeMember))
                .Throws(new MemberExistsException(fakeMessage));

            var sut = new Add();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(IStringCommand.FormatResult(fakeMessage));
            _mockDictionary.VerifyAll();
        }

        [Fact]
        public void Execute_WhenMemberValid_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey, fakeMember};

            var sut = new Add();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(IStringCommand.FormatResult(Constants.Messages.Added));
        }
    }
}