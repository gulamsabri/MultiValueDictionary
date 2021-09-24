using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Moq;
using MultiValueDictionary.Exceptions;
using Xunit;

namespace MultiValueDictionary.Tests.Unit
{
    public class MultiValueDictionaryTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IDictionary<string, ICollection<string>>> _mockDictionary;

        public MultiValueDictionaryTests()
        {
            _fixture = new Fixture();
            _mockDictionary = new Mock<IDictionary<string, ICollection<string>>>();
        }
        
        [Fact]
        public void AllMembers_FlattensValueCollections()
        {
            var fakeHashSet1 = _fixture.Create<HashSet<string>>();
            var fakeHashSet2 = _fixture.Create<HashSet<string>>();
            var fakeValues = new HashSet<ICollection<string>> {fakeHashSet1, fakeHashSet2};
            var expectedResult = fakeValues.SelectMany(v => v);
            _mockDictionary
                .Setup(x => x.Values)
                .Returns(fakeValues);
            
            var sut = new MultiValueDictionary<string,string>(_mockDictionary.Object);

            sut.AllMembers
                .Should()
                .BeEquivalentTo(expectedResult);
            
            _mockDictionary.VerifyAll();
        }
        
        [Fact]
        public void Add_WhenNewKey_ThenAddsKeyWithNewHashSetMember()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            _mockDictionary
                .Setup(x => x.ContainsKey(fakeKey))
                .Returns(false);
            
            var sut = new MultiValueDictionary<string,string>(_mockDictionary.Object);
            
            sut.Add(fakeKey, fakeMember);
            
            _mockDictionary.VerifyAll();
            _mockDictionary
                .Verify(x => 
                    x.Add(fakeKey, It.Is<HashSet<string>>(v => v.Contains(fakeMember))), 
                    Times.Once());
        }
        
        [Fact]
        public void Add_WhenExistingKey_ThenUpdatesExistingHashsetWithMember()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var mockMemberCollection = new Mock<ICollection<string>>();
            mockMemberCollection.Setup(x => x.Contains(fakeMember)).Returns(false);
            _mockDictionary.Setup(x => x.ContainsKey(fakeKey)).Returns(true);
            _mockDictionary
                .Setup(x => x[fakeKey])
                .Returns(mockMemberCollection.Object);
            
            var sut = new MultiValueDictionary<string,string>(_mockDictionary.Object);

            sut.Add(fakeKey, fakeMember);
            
            _mockDictionary.VerifyAll();
            mockMemberCollection.Verify(x => x.Add(fakeMember), Times.Once());
        }
        
        [Fact]
        public void Add_WhenExistingKeyAndMember_ThenThrowsMemberExistsException()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var mockMemberCollection = new Mock<ICollection<string>>();
            mockMemberCollection.Setup(x => x.Contains(fakeMember)).Returns(true);
            _mockDictionary.Setup(x => x.ContainsKey(fakeKey)).Returns(true);
            _mockDictionary
                .Setup(x => x[fakeKey])
                .Returns(mockMemberCollection.Object);
            
            var sut = new MultiValueDictionary<string,string>(_mockDictionary.Object);

            sut.Invoking(x => x.Add(fakeKey, fakeMember))
                .Should()
                .ThrowExactly<MemberExistsException>()
                .WithMessage(Constants.Messages.MemberExists);
            
            _mockDictionary.VerifyAll();
            mockMemberCollection.Verify(x => x.Add(fakeMember), Times.Never);
        }
        
        [Fact]
        public void Clear_CallsClearOnUnderlyingDictionary()
        {
            var sut = new MultiValueDictionary<string,string>(_mockDictionary.Object);

            sut.Clear();
            
            _mockDictionary.Verify(x => x.Clear(), Times.Once);
        }
        
        [Fact]
        public void Items_ReturnsUnderlyingDictionaryAsReadOnly()
        {
            var fakeDictionary = _fixture.Create<Dictionary<string, ICollection<string>>>();
            
            var sut = new MultiValueDictionary<string,string>(fakeDictionary);

            sut.Items
                .Should()
                .BeEquivalentTo((IReadOnlyDictionary<string, ICollection<string>>) fakeDictionary);
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void KeyExists_CallsUnderlyingDictionaryContainsKey(bool underlyingResult)
        {
            var fakeKey = _fixture.Create<string>();
            _mockDictionary
                .Setup(x => x.ContainsKey(fakeKey))
                .Returns(underlyingResult);
            
            var sut = new MultiValueDictionary<string,string>(_mockDictionary.Object);

            sut.KeyExists(fakeKey)
                .Should().Be(underlyingResult);

            _mockDictionary.VerifyAll();
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Remove_WithKey_ReturnsUnderlyingCollectionRemoveResult(bool underlyingResult)
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var mockMemberCollection = new Mock<ICollection<string>>();
            _mockDictionary
                .Setup(x => x[fakeKey])
                .Returns(mockMemberCollection.Object);
            mockMemberCollection
                .Setup(x => x.Remove(fakeMember))
                .Returns(underlyingResult);
            
            var sut = new MultiValueDictionary<string,string>(_mockDictionary.Object);

            sut.Remove(fakeKey, fakeMember)
                .Should().Be(underlyingResult);

            _mockDictionary.VerifyAll();
            mockMemberCollection.VerifyAll();
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RemoveAll_WithKey_ReturnsUnderlyingDictionaryRemoveResult(bool underlyingResult)
        {
            var fakeKey = _fixture.Create<string>();
            _mockDictionary
                .Setup(x => x.Remove(fakeKey))
                .Returns(underlyingResult);

            var sut = new MultiValueDictionary<string,string>(_mockDictionary.Object);

            sut.RemoveAll(fakeKey)
                .Should().Be(underlyingResult);

            _mockDictionary.VerifyAll();
        }
        
        [Fact]
        public void Members_WithKey_ReturnsUnderlyingCollectionAsReadOnly()
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMemberCollection = _fixture.Create<HashSet<string>>();
            _mockDictionary
                .Setup(x => x[fakeKey])
                .Returns(fakeMemberCollection);

            var sut = new MultiValueDictionary<string,string>(_mockDictionary.Object);

            sut.Members(fakeKey)
                .Should()
                .BeEquivalentTo(fakeMemberCollection);
            
            _mockDictionary.VerifyAll();
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MemberExists_WithKey_ReturnsUnderlyingCollectionContainsResult(bool underlyingResult)
        {
            var fakeKey = _fixture.Create<string>();
            var fakeMember = _fixture.Create<string>();
            var mockMemberCollection = new Mock<ICollection<string>>();
            _mockDictionary
                .Setup(x => x[fakeKey])
                .Returns(mockMemberCollection.Object);
            mockMemberCollection
                .Setup(x => x.Contains(fakeMember))
                .Returns(underlyingResult);

            var sut = new MultiValueDictionary<string,string>(_mockDictionary.Object);

            sut.MemberExists(fakeKey, fakeMember)
                .Should().Be(underlyingResult);

            _mockDictionary.VerifyAll();
        }
    }
}