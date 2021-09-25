using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using MultiValueDictionary.Command;
using Xunit;

namespace MultiValueDictionary.Tests.Integration.Command
{
    public class AllMembersTests
    {
        private readonly IFixture _fixture;
        private readonly IMultiValueDictionary<string, string> _multiValueDictionary;
        private readonly IDictionary<string, ICollection<string>> _underlyingDictionary;

        public AllMembersTests()
        {
            _underlyingDictionary = new Dictionary<string, ICollection<string>>();
            _multiValueDictionary = new MultiValueDictionary<string, string>(_underlyingDictionary);
            _fixture = new Fixture();
        }

        [Fact]
        public void AllMembers_WhenNoMembers_ThenReturnsEmptySetMessage()
        {
            var fakeArgs = new[] {"_ignored"};

            var sut = new AllMembers();

            var result = sut.Execute(_multiValueDictionary, fakeArgs);

            result.Should().Be(IStringCommand.FormatResult(Constants.Messages.EmptySet));
        }

        [Fact]
        public void AllMembers_WhenMembers_ThenReturnsExpectedResult()
        {
            var fakeArgs = new[] {"_ignored"};
            var fakeKeyValuePairs = _fixture.CreateMany<KeyValuePair<string, ICollection<string>>>().ToList();

            foreach (var fakeKeyValuePair in fakeKeyValuePairs) _underlyingDictionary.Add(fakeKeyValuePair);

            var sut = new AllMembers();

            var result = sut.Execute(_multiValueDictionary, fakeArgs);

            foreach (var member in fakeKeyValuePairs.SelectMany(kvp => kvp.Value)) result.Should().Contain(member);
        }
    }
}