using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using MultiValueDictionary.Command;
using Xunit;

namespace MultiValueDictionary.Tests.Integration.Command
{
    public class RemoveTests
    {
        private readonly IFixture _fixture;
        private readonly IMultiValueDictionary<string, string> _multiValueDictionary;
        private readonly IDictionary<string, ICollection<string>> _underlyingDictionary;

        public RemoveTests()
        {
            _underlyingDictionary = new Dictionary<string, ICollection<string>>();
            _multiValueDictionary = new MultiValueDictionary<string, string>(_underlyingDictionary);
            _fixture = new Fixture();
        }

        [Fact]
        public void Remove_WhenKeyDoesNotExist_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey, fakeMember};

            var sut = new Remove();

            var result = sut.Execute(_multiValueDictionary, fakeArgs);

            result.Should().Be(IStringCommand.FormatResult(Constants.Messages.KeyNotFound));
        }

        [Fact]
        public void Remove_WhenMemberDoesNotExist_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey, fakeMember};
            _underlyingDictionary.Add(fakeKey, _fixture.CreateMany<string>().ToHashSet());

            var sut = new Remove();

            var result = sut.Execute(_multiValueDictionary, fakeArgs);

            result.Should().Be(IStringCommand.FormatResult(Constants.Messages.MemberDoesNotExist));
        }

        [Fact]
        public void Remove_WhenMemberExistsAndIsOnlyOneForKey_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey, fakeMember};
            _underlyingDictionary.Add(fakeKey, new HashSet<string> {fakeMember});

            var sut = new Remove();

            var result = sut.Execute(_multiValueDictionary, fakeArgs);

            result.Should().Be(IStringCommand.FormatResult(Constants.Messages.Removed));
            _underlyingDictionary.Keys.Should().NotContain(fakeKey);
        }

        [Fact]
        public void Remove_WhenMemberExistsAndHasCompanions_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey, fakeMember};
            _underlyingDictionary.Add(fakeKey, new HashSet<string> {fakeMember, "EXISTING_MEMBER"});

            var sut = new Remove();

            var result = sut.Execute(_multiValueDictionary, fakeArgs);

            result.Should().Be(IStringCommand.FormatResult(Constants.Messages.Removed));
            _underlyingDictionary.Keys.Should().Contain(fakeKey);
            _underlyingDictionary[fakeKey].Should().NotContain(fakeMember);
        }
    }
}