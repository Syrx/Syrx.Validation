// ============================================================================================================================= 
// author           : david sexton (@sextondjc | sextondjc.com)
// date             : 2015.12.23
// licence          : licensed under the terms of the MIT license. See LICENSE.txt
// =============================================================================================================================

// not to be confused with System.ComponentModel.DataAnnotations Validator. 
namespace Syrx.Validation.Attributes.Tests.Unit.ValidatorTests
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
            exception.HasMessage("The Name field is required.\r\n");
        }

        [Fact]        
        public void NullObjectThrowsArgumentNullException()
        {            
            var exception = Throws<ArgumentNullException>(() => Validate<object>(null));
            exception.ArgumentNull("The item passed for validation was null.");
        }

        [Fact]
        public void ValidatesCollection()
        {
            var collection = new List<ValidatorWidget>
            {
                new() { Name = "Test 1" },
                new() { Name = "Test 2" }
            };

            ValidateCollection(collection);
        }

        [Fact]        
        public void InvalidItemInCollectionThrowsValidationException()
        {
            var collection = new List<ValidatorWidget>
            {
                new() { Name = "Test 1" },
                new() { Name = "" }
            };
            
            var exception = Throws<ValidationException>(() => ValidateCollection(collection));
            exception.HasMessage("The Name field is required.\r\n");
        }

        [Fact]        
        public void NullCollectionThrowsArgumentNullException()
        {
            var collection = new List<ValidatorWidget>();
            collection = null;
            var exception = Throws<ArgumentNullException>(() => ValidateCollection(collection));
            exception.ArgumentNull("The collection passed for validation was null.");
        }
    }
}
