// ============================================================================================================================= 
// author           : david sexton (@sextondjc | sextondjc.com)
// date             : 2015.12.23
// licence          : licensed under the terms of the MIT license. See LICENSE.txt
// =============================================================================================================================
#nullable enable

namespace Syrx.Validation.Attributes
{
    /// <summary>
    /// A lightweight validation helper class which can be used to 
    /// perform model validation in repository classes.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Attempts to validate a model based on its property attributes.
        /// </summary>
        /// <typeparam name="T">The type of item to validate.</typeparam>
        /// <param name="item">The item to validate.</param>
        public static void Validate<T>(T? item)
        {
            // kick out nulls
            Throw<ArgumentNullException>(item != null, "The item passed for validation was null.");
            
            var context = new ValidationContext(
                item,
                serviceProvider: null, 
                items: null);

            var results = new List<ValidationResult>();
            var isValid = TryValidateObject(
                item, 
                context, 
                results,
                validateAllProperties: true
            );

            if (isValid) return;
            // build up the validation error message using string join for better performance
            var errorMessages = results.Select(r => r.ErrorMessage).Where(msg => !string.IsNullOrEmpty(msg));
            var combinedMessage = string.Join("\r\n", errorMessages);
            if (!string.IsNullOrEmpty(combinedMessage))
            {
                combinedMessage += "\r\n"; // Preserve original behavior of adding trailing newline
            }

            throw new ValidationException(combinedMessage);
        }

        public static void ValidateCollection<T>(IEnumerable<T>? items)
        {
            Throw<ArgumentNullException>(items != null, "The collection passed for validation was null.");
            foreach (var item in items)
            {
                Validate(item);
            }
        }
    }
}
