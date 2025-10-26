// ============================================================================================================================= 
// Tests for RequiredRangeAttribute
// =============================================================================================================================

namespace Syrx.Validation.Attributes.Tests.Unit.RequiredRangeAttributeTests
{
    public class IsValid
    {
        [Fact]
        public void NullValueReturnsFalse()
        {
            var attribute = new RequiredRangeAttribute(0, 100);
            var result = attribute.IsValid(null);
            False(result);
            Equal("The value cannot be null", attribute.ErrorMessage);
        }

        [Fact]
        public void ValidIntegerWithinRangeReturnsTrue()
        {
            var attribute = new RequiredRangeAttribute(0, 100);
            var result = attribute.IsValid(50);
            True(result);
        }

        [Fact]
        public void ValidDoubleWithinRangeReturnsTrue()
        {
            var attribute = new RequiredRangeAttribute(0.0, 100.0);
            var result = attribute.IsValid(50.5);
            True(result);
        }

        [Fact]
        public void ValueBelowMinimumReturnsFalse()
        {
            var attribute = new RequiredRangeAttribute(10, 100);
            var result = attribute.IsValid(5);
            False(result);
            Equal("The value 5 must be greater than or equal to 10", attribute.ErrorMessage);
        }

        [Fact]
        public void ValueAboveMaximumReturnsFalse()
        {
            var attribute = new RequiredRangeAttribute(10, 100);
            var result = attribute.IsValid(150);
            False(result);
            Equal("The value 150 must be less than or equal to 100", attribute.ErrorMessage);
        }

        [Fact]
        public void ExclusiveMinimumBoundaryReturnsFalse()
        {
            var attribute = new RequiredRangeAttribute(10, 100, minimumIsExclusive: true, maximumIsExclusive: false);
            var result = attribute.IsValid(10);
            False(result);
            Equal("The value 10 must be greater than 10", attribute.ErrorMessage);
        }

        [Fact]
        public void ExclusiveMaximumBoundaryReturnsFalse()
        {
            var attribute = new RequiredRangeAttribute(10, 100, minimumIsExclusive: false, maximumIsExclusive: true);
            var result = attribute.IsValid(100);
            False(result);
            Equal("The value 100 must be less than 100", attribute.ErrorMessage);
        }

        [Fact]
        public void NonNumericValueReturnsFalse()
        {
            var attribute = new RequiredRangeAttribute(0, 100);
            var result = attribute.IsValid("not a number");
            False(result);
            Equal("The value 'not a number' is not a valid numeric type", attribute.ErrorMessage);
        }

        [Fact]
        public void DecimalValueWithinRangeReturnsTrue()
        {
            var attribute = new RequiredRangeAttribute(0, 1000);
            var result = attribute.IsValid(500.50m);
            True(result);
        }

        [Fact]
        public void FloatValueWithinRangeReturnsTrue()
        {
            var attribute = new RequiredRangeAttribute(0, 1000);
            var result = attribute.IsValid(500.5f);
            True(result);
        }

        [Fact]
        public void ValueEqualToInclusiveMinimum_ReturnsTrue()
        {
            var attribute = new RequiredRangeAttribute(5.0, 10.0, false, false); // Both inclusive
            var result = attribute.IsValid(5.0);
            True(result);
        }

        [Fact]
        public void ValueEqualToInclusiveMaximum_ReturnsTrue()
        {
            var attribute = new RequiredRangeAttribute(5.0, 10.0, false, false); // Both inclusive
            var result = attribute.IsValid(10.0);
            True(result);
        }

        [Fact]
        public void ValueEqualToExclusiveMinimum_ReturnsFalse()
        {
            var attribute = new RequiredRangeAttribute(5.0, 10.0, true, false); // Min exclusive, Max inclusive
            var result = attribute.IsValid(5.0);
            False(result);
        }

        [Fact]
        public void ValueEqualToExclusiveMaximum_ReturnsFalse()
        {
            var attribute = new RequiredRangeAttribute(5.0, 10.0, false, true); // Min inclusive, Max exclusive
            var result = attribute.IsValid(10.0);
            False(result);
        }

        [Fact]
        public void ValueBelowInclusiveMinimum_ReturnsFalse()
        {
            var attribute = new RequiredRangeAttribute(5.0, 10.0); // Default constructor = both inclusive
            var result = attribute.IsValid(4.9);
            False(result);
            Contains("must be greater than or equal to", attribute.ErrorMessage);
        }

        [Fact]
        public void ValueAboveInclusiveMaximum_ReturnsFalse()
        {
            var attribute = new RequiredRangeAttribute(5.0, 10.0); // Default constructor = both inclusive
            var result = attribute.IsValid(10.1);
            False(result);
            Contains("must be less than or equal to", attribute.ErrorMessage);
        }

        [Fact]
        public void InclusiveMinimumBoundaryViolationSetsCorrectErrorMessage()
        {
            var attribute = new RequiredRangeAttribute(10.0, 20.0, false, false);
            var result = attribute.IsValid(9.0);
            False(result);
            Equal("The value 9 must be greater than or equal to 10", attribute.ErrorMessage);
        }

        [Fact]
        public void InclusiveMaximumBoundaryViolationSetsCorrectErrorMessage()
        {
            var attribute = new RequiredRangeAttribute(10.0, 20.0, false, false);
            var result = attribute.IsValid(21.0);
            False(result);
            Equal("The value 21 must be less than or equal to 20", attribute.ErrorMessage);
        }

        [Fact]
        public void BelowInclusiveMinimumReturnsFalse()
        {
            var attribute = new RequiredRangeAttribute(10, 20);
            var result = attribute.IsValid(5);
            False(result);
            Contains("must be greater than or equal to", attribute.ErrorMessage);
        }

        [Fact] 
        public void AboveInclusiveMaximumReturnsFalse()
        {
            var attribute = new RequiredRangeAttribute(10, 20);
            var result = attribute.IsValid(25);
            False(result);
            Contains("must be less than or equal to", attribute.ErrorMessage);
        }
    }
}