// ============================================================================================================================= 
// author           : david sexton (@sextondjc | sextondjc.com)
// date             : 2015.12.23
// modified         : 2017-03-11 (16:31)
// licence          : licensed under the terms of the MIT license. See LICENSE.txt
// =============================================================================================================================
using Syrx.Validation.Attributes;
using System;
using Xunit;
using static Xunit.Assert;

namespace Syrx.Validation.Tests.Attributes.RequiredDateAttributeTests
{    
    public class IsValid
    {
        [Fact]        
        public void NullObjectNotSupported()
        {
            var attribute = new RequiredDateAttribute();
            var exception = Throws<ArgumentNullException>(() => attribute.IsValid(null));
            Equal("Value cannot be null.\r\nParameter name: A value must be supplied to the RequiredDateAttribute.", exception.Message);
        }

        [Fact]        
        public void OnlySupportsDateTime()
        {
            var value = "test";
            var attribute = new RequiredDateAttribute();
            var exception = Throws<ArgumentException>(() => attribute.IsValid(value));
            Equal("The value supplied to the RequiredDateAttribute (test) was not a DateTime type.", exception.Message);
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
        public void NoneAndPastOnlyIsSupported()
        {
            var value = DateTime.Now.AddDays(-1);
            var options = RequiredDateOptions.None | RequiredDateOptions.PastOnly;
            var attribute = new RequiredDateAttribute(options);
            var result = attribute.IsValid(value);
            True(result);
        }

    }
}
