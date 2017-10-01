// ============================================================================================================================= 
// author           : david sexton (@sextondjc | sextondjc.com)
// date             : 2015.12.23
// licence          : licensed under the terms of the MIT license. See LICENSE.txt
// =============================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

// not to be confused with System.ComponentModel.DataAnnotations Validator. 
using static Syrx.Validation.Validator;
using static Xunit.Assert;

namespace Syrx.Validation.Tests.ValidatorTests
{    
    public class Validate
    {
        [Fact]
        public void ValidatesObject()
        {
            var widget = new ValidatorWidget
            {
                Name = "Pass"
            };            
            Validate(widget);
        }

        [Fact]        
        public void InvalidObjectThrowsValidationException()
        {
            var widget = new ValidatorWidget();
            var exception = Throws<ValidationException>(() => Validate(widget));
            Equal("The Name field is required.\r\n", exception.Message);
        }

        [Fact]        
        public void NullObjectThrowsArgumentNullException()
        {            
            var exception = Throws<ArgumentNullException>(() => Validate<object>(null));
            Equal("Value cannot be null.\r\nParameter name: The item passed for validation was null.", exception.Message);
        }

        [Fact]
        public void ValidatesCollection()
        {
            var collection = new List<ValidatorWidget>
            {
                new ValidatorWidget { Name = "Test 1" },
                new ValidatorWidget { Name = "Test 2" }
            };

            ValidateCollection(collection);
        }

        [Fact]        
        public void InvalidItemInCollectionThrowsValidationException()
        {
            var collection = new List<ValidatorWidget>
            {
                new ValidatorWidget { Name = "Test 1" },
                new ValidatorWidget { Name = "" }
            };
            
            var exception = Throws<ValidationException>(() => ValidateCollection(collection));
            Equal("The Name field is required.\r\n", exception.Message);
        }

        [Fact]        
        public void NullCollectionThrowsArgumentNullException()
        {
            var collection = new List<ValidatorWidget>();
            collection = null;
            var exception = Throws<ArgumentNullException>(() => ValidateCollection(collection));
            Equal("Value cannot be null.\r\nParameter name: The collection passed for validation was null.", exception.Message);
        }
    }
}
