// ============================================================================================================================= 
// author           : david sexton (@sextondjc | sextondjc.com)
// date             : 2015.12.23
// modified         : 2017-03-11 (16:25)
// licence          : licensed under the terms of the MIT license. See LICENSE.txt
// =============================================================================================================================

namespace Syrx.Validation.Attributes.Tests.Unit.RequiredCollectionAttributeTests
{    
    public class IsValid
    {
        [Fact]        
        public void NullObjectThrowsArgumentNullException()
        {
            var attribute = new RequiredCollectionAttribute(0, 0);            
            var exception = Throws<ArgumentNullException>(() => attribute.IsValid(null));
            exception.ArgumentNull("The object passed to the attribute was null.");
        }

        [Fact]        
        public void WrongTypeThrowsInvalidCastException()
        {
            var attribute = new RequiredCollectionAttribute(0, 0);
            var exception = Throws<InvalidCastException>(() => attribute.IsValid(attribute));
            exception.HasMessage("Unable to cast object of type 'Syrx.Validation.Attributes.RequiredCollectionAttribute' to type 'System.Collections.IEnumerable'.");
        }
        
        [Theory]
        [MemberData(nameof(MinLengthData))]
        public void MinimumLengthCheck(List<int> collection, int min, bool expect)
        {
            var attribute = new RequiredCollectionAttribute(min);
            var result = attribute.IsValid(collection);
            Equal(expect, result);
        }
        
        [Theory]
        [MemberData(nameof(MaxLengthData))]
        public void MaximumLengthCheck(List<int> collection, int min, int max, bool expect)
        {
            var attribute = new RequiredCollectionAttribute(min, max);
            var result = attribute.IsValid(collection);
            Equal(expect, result);
        }
        
        public static IEnumerable<object[]> MinLengthData => new[]
        {
            new object[] { new List<int>(), 1, false },
            new object[] { new List<int> { 1 }, 1, true },
            new object[] { new List<int> { 1,2,3 }, 2, true }
        };

        public static IEnumerable<object[]> MaxLengthData => new[]
        {
            new object[] { new List<int> { 1, 2, 3 }, 0, 2, false },
            new object[] { new List<int> { 1, 2, 3 }, 0, 5, true },
            new object[] { new List<int> { 1, 2, 3 }, 1, 5, true }
        };
    }
}
