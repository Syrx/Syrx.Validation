// ============================================================================================================================= 
// author           : david sexton (@sextondjc | sextondjc.com)
// date             : 2015.12.23
// licence          : licensed under the terms of the MIT license. See LICENSE.txt
// =============================================================================================================================

using System;
using Xunit;
using static Xunit.Assert;

namespace Syrx.Validation.Attributes.Tests.Unit.ValidatorTests
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
            result.ArgumentNull(nameof(name));
        }

        [Fact]
        public void NameTooLongThrowsArgumentOutOfRangeException()
        {
            var name = new string('a', 51);
            var result = Throws<ArgumentOutOfRangeException>(() => new Person(name, DateOfBirth));
            result.ArgumentOutOfRange(nameof(name));
        }

        [Fact]
        public void MinDateOfBirthThrowsArgumentOutOfRangeException()
        {
            var result = Throws<ArgumentOutOfRangeException>(() => new Person("Test", DateTime.MinValue));
            result.ArgumentOutOfRange("dateOfBirth");
        }

        [Fact]
        public void MaxDateOfBirthThrowsArgumentOutOfRangeException()
        {
            var result = Throws<ArgumentOutOfRangeException>(() => new Person("Test", DateTime.MaxValue));
            result.ArgumentOutOfRange("dateOfBirth");
        }


        [Fact]
        public void FutureDateOfBirthThrowsArgumentOutOfRangeException()
        {
            var dateOfBirth = DateTime.Now.AddDays(1);
            var result = Throws<ArgumentOutOfRangeException>(() => new Person("Test", dateOfBirth));
            result.ArgumentOutOfRange("dateOfBirth");
        }


        [Fact]
        public void ReturnsValidInstance()
        {
            const string name = "Test";
            var dateOfBirth = DateTime.Now.AddYears(-25);
            var result = new Person(name, dateOfBirth);
            Equal(name, result.Name);
            Equal(dateOfBirth, result.DateOfBirth);
        }
    }
}
