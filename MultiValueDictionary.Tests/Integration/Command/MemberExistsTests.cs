using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using MultiValueDictionary.Command;
using Xunit;

namespace MultiValueDictionary.Tests.Integration.Command
{
    public class MemberExistsTests
    {
        private readonly IFixture _fixture;
        private readonly IMultiValueDictionary<string, string> _multiValueDictionary;
        private readonly IDictionary<string, ICollection<string>> _underlyingDictionary;

        public MemberExistsTests()
        {
            _underlyingDictionary = new Dictionary<string, ICollection<string>>();
            _multiValueDictionary = new MultiValueDictionary<string, string>(_underlyingDictionary);
            _fixture = new Fixture();
        }

        [Fact]
        public void MemberExists_WhenDoesNotExist_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey, fakeMember};

            var sut = new MemberExists();

            var result = sut.Execute(_multiValueDictionary, fakeArgs);

            result.Should().Be(IStringCommand.FormatResult(false.ToString()));
        }

        [Fact]
        public void MemberExists_WhenExists_ThenReturnsExpectedResult()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var fakeArgs = new[] {"_ignored", fakeKey, fakeMember};
            _underlyingDictionary.Add(fakeKey, new HashSet<string> {fakeMember});

            var sut = new MemberExists();

            var result = sut.Execute(_multiValueDictionary, fakeArgs);

            result.Should().Be(IStringCommand.FormatResult(true.ToString()));
        }
    }
}