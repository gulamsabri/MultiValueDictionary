using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using MultiValueDictionary.Command;
using Xunit;

namespace MultiValueDictionary.Tests.Integration.Command
{
    public class MembersTests
    {
        private readonly IFixture _fixture;
        private readonly IMultiValueDictionary<string, string> _multiValueDictionary;
        private readonly IDictionary<string, ICollection<string>> _underlyingDictionary;

        public MembersTests()
        {
            _underlyingDictionary = new Dictionary<string, ICollection<string>>();
            _multiValueDictionary = new MultiValueDictionary<string, string>(_underlyingDictionary);
            _fixture = new Fixture();
        }

        [Fact]
        public void Items_WhenNoMatchingKey_ThenReturnsExpectedMessage()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey};

            var sut = new Members();

            var result = sut.Execute(_multiValueDictionary, fakeArgs);

            result.Should().Be(IStringCommand.FormatResult(Constants.Messages.KeyNotFound));
        }

        [Fact]
        public void Members_WhenMembers_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey};
            var expectedMembers = _fixture.CreateMany<string>().ToHashSet();
            var expectedKeyValuePair = new KeyValuePair<string, ICollection<string>>(fakeKey, expectedMembers);

            _underlyingDictionary.Add(expectedKeyValuePair);

            var sut = new Members();

            var result = sut.Execute(_multiValueDictionary, fakeArgs);

            foreach (var member in expectedMembers) result.Should().Contain(member);
        }
    }
}