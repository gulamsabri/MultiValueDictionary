using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using MultiValueDictionary.Command;
using Xunit;

namespace MultiValueDictionary.Tests.Integration.Command
{
    public class ClearTests
    {
        private readonly IFixture _fixture;
        private readonly IMultiValueDictionary<string, string> _multiValueDictionary;
        private readonly IDictionary<string, ICollection<string>> _underlyingDictionary;

        public ClearTests()
        {
            _underlyingDictionary = new Dictionary<string, ICollection<string>>();
            _multiValueDictionary = new MultiValueDictionary<string, string>(_underlyingDictionary);
            _fixture = new Fixture();
        }

        [Fact]
        public void Clear_RemovesEverythingAndReturnsExpectedMessage()
        {
            var fakeArgs = new[] {"_ignored"};
            var fakeKeyValuePairs = _fixture.CreateMany<KeyValuePair<string, ICollection<string>>>().ToList();

            foreach (var fakeKeyValuePair in fakeKeyValuePairs) _underlyingDictionary.Add(fakeKeyValuePair);

            var sut = new Clear();

            var result = sut.Execute(_multiValueDictionary, fakeArgs);

            result.Should().Be(IStringCommand.FormatResult(Constants.Messages.Cleared));
            _underlyingDictionary.Should().BeEmpty();
        }
    }
}