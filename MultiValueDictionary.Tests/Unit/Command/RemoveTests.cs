using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using Moq;
using MultiValueDictionary.Command;
using Xunit;

namespace MultiValueDictionary.Tests.Unit.Command
{
    public class RemoveTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMultiValueDictionary<string, string>> _mockDictionary;

        public RemoveTests()
        {
            _fixture = new Fixture();
            _mockDictionary = new Mock<IMultiValueDictionary<string, string>>();
        }

        [Fact]
        public void Execute_WhenMemberValid_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey, fakeMember};
            _mockDictionary
                .Setup(x => x.Remove(fakeKey, fakeMember))
                .Returns(true);

            var sut = new Remove();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(IStringCommand.FormatResult(Constants.Messages.Removed));

            _mockDictionary.VerifyAll();
        }

        [Fact]
        public void Execute_WhenRemoveIsFalseForSomeReason_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey, fakeMember};
            _mockDictionary
                .Setup(x => x.Remove(fakeKey, fakeMember))
                .Returns(false);

            var sut = new Remove();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(IStringCommand.FormatResult(Constants.Messages.UnknownError));

            _mockDictionary.VerifyAll();
        }

        [Fact]
        public void Execute_WhenMemberDoesNotExistException_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey, fakeMember};
            _mockDictionary
                .Setup(x => x.Remove(fakeKey, fakeMember))
                .Throws(new MemberDoesNotExistException(Constants.Messages.MemberDoesNotExist));

            var sut = new Remove();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(IStringCommand.FormatResult(Constants.Messages.MemberDoesNotExist));

            _mockDictionary.VerifyAll();
        }

        [Fact]
        public void Execute_WhenKeyNotFoundException_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey, fakeMember};
            _mockDictionary
                .Setup(x => x.Remove(fakeKey, fakeMember))
                .Throws<KeyNotFoundException>();

            var sut = new Remove();

            sut.Execute(_mockDictionary.Object, fakeArgs)
                .Should()
                .Be(IStringCommand.FormatResult(Constants.Messages.KeyNotFound));

            _mockDictionary.VerifyAll();
        }
    }
}