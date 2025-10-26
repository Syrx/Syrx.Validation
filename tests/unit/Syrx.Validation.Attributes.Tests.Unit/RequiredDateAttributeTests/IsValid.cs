// ============================================================================================================================= 
// author           : david sexton (@sextondjc | sextondjc.com)
// date             : 2015.12.23
// modified         : 2017-03-11 (16:31)
// licence          : licensed under the terms of the MIT license. See LICENSE.txt
// =============================================================================================================================

namespace Syrx.Validation.Attributes.Tests.Unit.RequiredDateAttributeTests
{    
    public class IsValid
    {
        [Fact]        
        public void NullObjectNotSupported()
        {
            var attribute = new RequiredDateAttribute();
            var exception = Throws<ArgumentNullException>(() => attribute.IsValid(null)); 
            exception.HasMessage("Value cannot be null. (Parameter 'A value must be supplied to the RequiredDateAttribute.')");
        }

        [Fact]        
        public void OnlySupportsDateTime()
        {
            var value = "test";
            var attribute = new RequiredDateAttribute();
            var exception = Throws<ArgumentException>(() => attribute.IsValid(value));
            exception.HasMessage("The value supplied to the RequiredDateAttribute (test) was not a DateTime type.");
        }

        [Fact]
        public void DefaultsToUtc()
        {
            var value = DateTime.Now;
            var attribute = new RequiredDateAttribute();
            var result = attribute.IsValid(value);
            False(result);
        }

        [Fact]
        public void NoneOptionReturnsTrue()
        {
            var value = DateTime.Now;
            var options = RequiredDateOptions.None;
            var attribute = new RequiredDateAttribute(options);
            var result = attribute.IsValid(value);
            True(result);
        }

        [Fact]
        public void SupportsNonUtcDates()
        {
            var value = DateTime.Now;
            var options = RequiredDateOptions.NotUtc;
            var attribute = new RequiredDateAttribute(options);
            var result = attribute.IsValid(value);
            True(result);
        }

        [Fact]
        public void SupportsFutureOnlyDate()
        {
            var value = DateTime.UtcNow.AddDays(1);
            var options = RequiredDateOptions.FutureOnly;
            var attribute = new RequiredDateAttribute(options);
            var result = attribute.IsValid(value);
            True(result);
        }

        [Fact]
        public void SupportsFutureOnlyUtcDate()
        {
            var value = DateTime.UtcNow.AddDays(1);
            var options = RequiredDateOptions.FutureOnly | RequiredDateOptions.UtcOnly;
            var attribute = new RequiredDateAttribute(options);
            var result = attribute.IsValid(value);
            True(result);
        }

        [Fact]
        public void SupportsFutureOnlyNotUtcDate()
        {
            var value = DateTime.Now.AddDays(1);
            var options = RequiredDateOptions.FutureOnly | RequiredDateOptions.NotUtc;
            var attribute = new RequiredDateAttribute(options);
            var result = attribute.IsValid(value);
            True(result);
        }

        [Fact]
        public void SupportsPastOnlyDate()
        {
            var value = DateTime.UtcNow.AddDays(-1);
            var options = RequiredDateOptions.PastOnly;
            var attribute = new RequiredDateAttribute(options);
            var result = attribute.IsValid(value);
            True(result);
        }

        [Fact]
        public void SupportsPastOnlyUtcDate()
        {
            var value = DateTime.UtcNow.AddDays(-1);
            var options = RequiredDateOptions.PastOnly | RequiredDateOptions.UtcOnly;
            var attribute = new RequiredDateAttribute(options);
            var result = attribute.IsValid(value);
            True(result);
        }

        [Fact]
        public void SupportsPastOnlyNotUtcDate()
        {
            var value = DateTime.Now.AddDays(-1);
            var options = RequiredDateOptions.PastOnly | RequiredDateOptions.NotUtc;
            var attribute = new RequiredDateAttribute(options);
            var result = attribute.IsValid(value);
            True(result);
        }

        [Fact]
        public void PastOnlyAndFutureOnlyIsNotSupported()
        {
            var value = DateTime.Now.AddDays(-1);
            var options = RequiredDateOptions.PastOnly | RequiredDateOptions.FutureOnly;
            var attribute = new RequiredDateAttribute(options);
            var result = attribute.IsValid(value);
            False(result);
        }

        [Fact]
        public void NoneAndFutureOnlyIsSupported()
        {
            var value = DateTime.Now.AddDays(1);
            var options = RequiredDateOptions.None | RequiredDateOptions.FutureOnly;
            var attribute = new RequiredDateAttribute(options);
            var result = attribute.IsValid(value);
            True(result);
        }

        [Fact]
        public void IsValid_DateTime_PastOnly_PassesValidation()
        {
            var value = DateTime.Now.AddDays(-1);
            var options = RequiredDateOptions.None | RequiredDateOptions.PastOnly;
            var attribute = new RequiredDateAttribute(options);
            var result = attribute.IsValid(value);
            True(result);
        }

        [Fact]
        public void IsValid_PastOnly_WithFutureDate_ReturnsFalse()
        {
            var attribute = new RequiredDateAttribute(RequiredDateOptions.PastOnly);
            var futureDate = DateTime.Now.AddDays(1);
            var result = attribute.IsValid(futureDate);
            False(result);
            Contains("must be in UTC and in the past", attribute.ErrorMessage);
        }

        [Fact]
        public void IsValid_FutureOnly_WithPastDate_ReturnsFalse()
        {
            var attribute = new RequiredDateAttribute(RequiredDateOptions.FutureOnly);
            var pastDate = DateTime.Now.AddDays(-1);
            var result = attribute.IsValid(pastDate);
            False(result);
            Contains("must be in UTC and in the future", attribute.ErrorMessage);
        }

        [Fact]
        public void IsValid_UtcOnly_WithLocalTime_ReturnsFalse()
        {
            var attribute = new RequiredDateAttribute(RequiredDateOptions.UtcOnly);
            var localTime = DateTime.Now; // Local time
            var result = attribute.IsValid(localTime);
            False(result);
        }

        [Fact]
        public void IsValid_NotUtc_WithUtcTime_ReturnsFalse()
        {
            var attribute = new RequiredDateAttribute(RequiredDateOptions.NotUtc);
            var utcTime = DateTime.UtcNow;
            var result = attribute.IsValid(utcTime);
            False(result);
        }

    }
}
