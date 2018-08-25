// ============================================================================================================================= 
// author           : david sexton (@sextondjc | sextondjc.com)
// date             : 2015.12.23
// licence          : licensed under the terms of the MIT license. See LICENSE.txt
// =============================================================================================================================

using System;
using Xunit;
using static Xunit.Assert;

namespace Syrx.Validation.Tests.ValidatorTests
{
    /// <summary>
    /// Used soley for example.
    /// </summary>
    public class PersonTest
    {
        private DateTime DateOfBirth => DateTime.Now.Subtract(TimeSpan.FromDays(1));

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void NameIsNullEmptyWhitespaceThrowsArgumentNullException(string name)
        {
            var result = Throws<ArgumentNullException>(() => new Person(name, this.DateOfBirth));
            Equal("Value cannot be null.\r\nParameter name: name", result.Message);
        }

        [Fact]
        public void NameTooLongThrowsArgumentOutOfRangeException()
        {
            var name = new string('a', 51);
            var result = Throws<ArgumentOutOfRangeException>(() => new Person(name, DateOfBirth));
            Equal("Specified argument was out of the range of valid values.\r\nParameter name: name", result.Message);
        }

        [Fact]
        public void MinDateOfBirthThrowsArgumentOutOfRangeException()
        {
            var result = Throws<ArgumentOutOfRangeException>(() => new Person("Test", DateTime.MinValue));
            Equal("Specified argument was out of the range of valid values.\r\nParameter name: dateOfBirth", result.Message);

        }

        [Fact]
        public void MaxDateOfBirthThrowsArgumentOutOfRangeException()
        {
            var result = Throws<ArgumentOutOfRangeException>(() => new Person("Test", DateTime.MaxValue));
            Equal("Specified argument was out of the range of valid values.\r\nParameter name: dateOfBirth", result.Message);
        }


        [Fact]
        public void FutureDateOfBirthThrowsArgumentOutOfRangeException()
        {
            var dateOfBirth = DateTime.Now.AddDays(1);
            var result = Throws<ArgumentOutOfRangeException>(() => new Person("Test", dateOfBirth));
            Equal("Specified argument was out of the range of valid values.\r\nParameter name: dateOfBirth", result.Message);
        }
    }
}
