using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using MultiValueDictionary.Command;
using Xunit;

namespace MultiValueDictionary.Tests.Integration.Command
{
    public class AddTests
    {
        private readonly IFixture _fixture;
        private readonly IMultiValueDictionary<string, string> _multiValueDictionary;
        private readonly IDictionary<string, ICollection<string>> _underlyingDictionary;

        public AddTests()
        {
            _underlyingDictionary = new Dictionary<string, ICollection<string>>();
            _multiValueDictionary = new MultiValueDictionary<string, string>(_underlyingDictionary);
            _fixture = new Fixture();
        }

        [Fact]
        public void Add_WhenNewKey_ThenAddsNewKeyAndMember()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey, fakeMember};

            var sut = new Add();

            var result = sut.Execute(_multiValueDictionary, fakeArgs);

            result.Should().Be(IStringCommand.FormatResult(Constants.Messages.Added));
            _underlyingDictionary.Keys.Count.Should().Be(1);
            _underlyingDictionary[fakeKey].Count.Should().Be(1);
            _underlyingDictionary[fakeKey].Should().Contain(fakeMember);
        }

        [Fact]
        public void Add_WhenExistingKey_ThenAddsNewMemberToKey()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey, fakeMember};
            _underlyingDictionary.Add(fakeKey, new HashSet<string> {"EXISTING_MEMBER"});

            var sut = new Add();

            var result = sut.Execute(_multiValueDictionary, fakeArgs);

            result.Should().Be(IStringCommand.FormatResult(Constants.Messages.Added));
            _underlyingDictionary.Keys.Count.Should().Be(1);
            _underlyingDictionary[fakeKey].Count.Should().Be(2);
            _underlyingDictionary[fakeKey].Should().Contain(fakeMember);
        }
    }
}