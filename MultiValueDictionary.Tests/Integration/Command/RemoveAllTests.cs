using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using MultiValueDictionary.Command;
using Xunit;

namespace MultiValueDictionary.Tests.Integration.Command
{
    public class RemoveAllTests
    {
        private readonly IFixture _fixture;
        private readonly IMultiValueDictionary<string, string> _multiValueDictionary;
        private readonly IDictionary<string, ICollection<string>> _underlyingDictionary;

        public RemoveAllTests()
        {
            _underlyingDictionary = new Dictionary<string, ICollection<string>>();
            _multiValueDictionary = new MultiValueDictionary<string, string>(_underlyingDictionary);
            _fixture = new Fixture();
        }

        [Fact]
        public void RemoveAll_WhenKeyDoesNotExist_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey};

            var sut = new RemoveAll();

            var result = sut.Execute(_multiValueDictionary, fakeArgs);

            result.Should().Be(IStringCommand.FormatResult(Constants.Messages.KeyNotFound));
        }

        [Fact]
        public void RemoveAll_WhenKeyExists_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey};
            _underlyingDictionary.Add(fakeKey, _fixture.CreateMany<string>().ToHashSet());

            var sut = new RemoveAll();

            var result = sut.Execute(_multiValueDictionary, fakeArgs);

            result.Should().Be(IStringCommand.FormatResult(Constants.Messages.Removed));
            _underlyingDictionary.Keys.Should().NotContain(fakeKey);
        }
    }
}