// ============================================================================================================================= 
// author           : david sexton (@sextondjc | sextondjc.com)
// date             : 2015.12.23
// licence          : licensed under the terms of the MIT license. See LICENSE.txt
// =============================================================================================================================

namespace Syrx.Validation.Attributes.Tests.Unit.RequiredGuidAttributeTests
{    
    public class IsValid
    {
        [Fact]
        public void ValidGuidReturnsTrue()
        {
            var value = Guid.NewGuid();

            var attribute = new RequiredGuidAttribute();
            var result = attribute.IsValid(value);

            True(result);
        }

        [Fact]
        public void NullValueReturnsFalse()
        {
            var attribute = new RequiredGuidAttribute();
            var result = attribute.IsValid(null);

            False(result);
        }

        [Fact]
        public void NonGuidReturnsFalse()
        {
            var attribute = new RequiredGuidAttribute();
            var result = attribute.IsValid("Test");

            False(result);
        }

        [Fact]
        public void EmptyGuidReturnsFalse()
        {
            var attribute = new RequiredGuidAttribute();
            var result = attribute.IsValid(Guid.Empty);

            False(result);
        }        
    }
}
