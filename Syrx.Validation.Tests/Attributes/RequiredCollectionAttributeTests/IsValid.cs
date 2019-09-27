// ============================================================================================================================= 
// author           : david sexton (@sextondjc | sextondjc.com)
// date             : 2015.12.23
// modified         : 2017-03-11 (16:25)
// licence          : licensed under the terms of the MIT license. See LICENSE.txt
// =============================================================================================================================
using Syrx.Validation.Attributes;
using System;
using System.Collections.Generic;
using Xunit;
using static Xunit.Assert;

namespace Syrx.Validation.Tests.Attributes.RequiredCollectionAttributeTests
{    
    public class IsValid
    {
        [Fact]        
        public void NullObjectThrowsArgumentNullException()
        {
            var attribute = new RequiredCollectionAttribute(0, 0);            
            var exception = Throws<ArgumentNullException>(() => attribute.IsValid(null));
            Equal("Value cannot be null. (Parameter 'The object passed to the attribute was null.')", exception.Message);
        }

        [Fact]        
        public void WrongTypeThrowsInvalidCastException()
        {
            var attribute = new RequiredCollectionAttribute(0, 0);
            var exception = Throws<InvalidCastException>(() => attribute.IsValid(attribute));
            Equal("Unable to cast object of type 'Syrx.Validation.Attributes.RequiredCollectionAttribute' to type 'System.Collections.IEnumerable'.", exception.Message);
        }

        [Fact]
        public void LengthBelowMinimumReturnsFalse()
        {
            var collection = new List<object>();            
            var attribute = new RequiredCollectionAttribute(1);
            var result = attribute.IsValid(collection);
            False(result);
        }

        [Fact]
        public void LengthAboveMaximumReturnsFalse()
        {
            var collection = new List<int> { 1, 2, 3 };
            var attribute = new RequiredCollectionAttribute(0, 2);
            var result = attribute.IsValid(collection);
            False(result);
        }

        [Fact]
        public void LengthMatchesMinimumReturnsTrue()
        {
            var collection = new List<int> { 1 };
            var attribute = new RequiredCollectionAttribute(1);
            var result = attribute.IsValid(collection);
            True(result);
        }

        [Fact]
        public void LengthAboveMinimumReturnsTrue()
        {
            var collection = new List<int> { 1, 2, 3 };
            var attribute = new RequiredCollectionAttribute(2);
            var result = attribute.IsValid(collection);
            True(result);
        }

        [Fact]
        public void LengthBelowMaximumReturnsTrue()
        {
            var collection = new List<int> { 1, 2, 3 };
            var attribute = new RequiredCollectionAttribute(0,5);
            var result = attribute.IsValid(collection);
            True(result);
        }

        [Fact]
        public void LengthBetweenMinimumAndMaximumReturnsTrue()
        {
            var collection = new List<int> { 1, 2, 3 };
            var attribute = new RequiredCollectionAttribute(1, 5);
            var result = attribute.IsValid(collection);
            True(result);
        }        
    }
}
