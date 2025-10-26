// ============================================================================================================================= 
// Tests for RequiredStringAttribute
// =============================================================================================================================
namespace Syrx.Validation.Attributes.Tests.Unit.RequiredStringAttributeTests
{
    public class IsValid
    {
        [Fact]
        public void NullValueReturnsFalse()
        {
            var attribute = new RequiredStringAttribute(1, 10);
            var result = attribute.IsValid(null);
            False(result);
            Equal("The value cannot be null", attribute.ErrorMessage);
        }

        [Fact]
        public void NonStringValueReturnsFalse()
        {
            var attribute = new RequiredStringAttribute(1, 10);
            var result = attribute.IsValid(123);
            False(result);
            Equal("The value must be of type string, but was Int32", attribute.ErrorMessage);
        }

        [Fact]
        public void EmptyStringWhenNotAllowedReturnsFalse()
        {
            var attribute = new RequiredStringAttribute(1, 10, allowEmpty: false);
            var result = attribute.IsValid("");
            False(result);
            Equal("The string cannot be empty", attribute.ErrorMessage);
        }

        [Fact]
        public void EmptyStringWhenAllowedReturnsTrue()
        {
            var attribute = new RequiredStringAttribute(0, 10, allowEmpty: true);
            var result = attribute.IsValid("");
            True(result);
        }

        [Fact]
        public void StringTooShortReturnsFalse()
        {
            var attribute = new RequiredStringAttribute(5, 10);
            var result = attribute.IsValid("abc");
            False(result);
            Equal("The string length of 3 is less than the minimum required of 5", attribute.ErrorMessage);
        }

        [Fact]
        public void StringTooLongReturnsFalse()
        {
            var attribute = new RequiredStringAttribute(1, 5);
            var result = attribute.IsValid("toolongstring");
            False(result);
            Equal("The string length of 13 is greater than the maximum allowed of 5", attribute.ErrorMessage);
        }

        [Fact]
        public void ValidLengthStringReturnsTrue()
        {
            var attribute = new RequiredStringAttribute(3, 10);
            var result = attribute.IsValid("hello");
            True(result);
        }

        [Fact]
        public void PatternMatchingValidStringReturnsTrue()
        {
            var attribute = new RequiredStringAttribute(@"^[a-zA-Z]+$"); // Only letters
            var result = attribute.IsValid("HelloWorld");
            True(result);
        }

        [Fact]
        public void PatternMatchingInvalidStringReturnsFalse()
        {
            var attribute = new RequiredStringAttribute(@"^[a-zA-Z]+$"); // Only letters
            var result = attribute.IsValid("Hello123");
            False(result);
            Equal("The string does not match the required pattern: ^[a-zA-Z]+$", attribute.ErrorMessage);
        }

        [Fact]
        public void EmailPatternValidEmailReturnsTrue()
        {
            var attribute = new RequiredStringAttribute(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            var result = attribute.IsValid("test@example.com");
            True(result);
        }

        [Fact]
        public void EmailPatternInvalidEmailReturnsFalse()
        {
            var attribute = new RequiredStringAttribute(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            var result = attribute.IsValid("invalid-email");
            False(result);
            Contains("does not match the required pattern", attribute.ErrorMessage);
        }

        [Fact]
        public void CombinedLengthAndPatternValidStringReturnsTrue()
        {
            var attribute = new RequiredStringAttribute(5, 20, @"^[a-zA-Z]+$");
            var result = attribute.IsValid("HelloWorld");
            True(result);
        }

        [Fact]
        public void CombinedLengthAndPatternTooShortReturnsFalse()
        {
            var attribute = new RequiredStringAttribute(10, 20, @"^[a-zA-Z]+$");
            var result = attribute.IsValid("Hi");
            False(result);
            Equal("The string length of 2 is less than the minimum required of 10", attribute.ErrorMessage);
        }

        [Fact]
        public void CombinedLengthAndPatternInvalidPatternReturnsFalse()
        {
            var attribute = new RequiredStringAttribute(3, 20, @"^[a-zA-Z]+$");
            var result = attribute.IsValid("Hello123");
            False(result);
            Equal("The string does not match the required pattern: ^[a-zA-Z]+$", attribute.ErrorMessage);
        }

        [Fact]
        public void Constructor_WithMinimumLengthOnly_SetsPropertiesCorrectly()
        {
            var attribute = new RequiredStringAttribute(5);
            Equal(5, attribute.MinimumLength);
            Equal(int.MaxValue, attribute.MaximumLength);
        }

        [Fact]
        public void Constructor_WithInvalidPattern_ThrowsArgumentException()
        {
            var exception = Throws<ArgumentException>(() => 
                new RequiredStringAttribute("[invalid regex"));
            Contains("Invalid regular expression pattern", exception.Message);
        }

        [Fact]
        public void Constructor_WithInvalidPatternInCombined_ThrowsArgumentException()
        {
            var exception = Throws<ArgumentException>(() => 
                new RequiredStringAttribute(1, 10, "[invalid regex"));
            Contains("Invalid regular expression pattern", exception.Message);
        }
    }
}
