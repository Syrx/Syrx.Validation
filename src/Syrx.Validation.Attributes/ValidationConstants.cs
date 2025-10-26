// ============================================================================================================================= 
// author           : david sexton (@sextondjc | sextondjc.com)
// date             : 2025.10.25
// licence          : licensed under the terms of the MIT license. See LICENSE.txt
// =============================================================================================================================
#nullable enable

namespace Syrx.Validation.Attributes
{
    /// <summary>
    /// Contains constants used throughout the validation library.
    /// </summary>
    internal static class ValidationConstants
    {
        /// <summary>
        /// Default minimum collection count when not specified.
        /// </summary>
        public const int DefaultMinimumCollectionCount = 1;

        /// <summary>
        /// Indicates no maximum limit for collections.
        /// </summary>
        public const int NoMaximumLimit = 0;

        /// <summary>
        /// Maximum string length when not specified.
        /// </summary>
        public const int MaximumStringLength = int.MaxValue;

        /// <summary>
        /// Minimum string length when not specified.
        /// </summary>
        public const int MinimumStringLength = 0;
    }

    /// <summary>
    /// Contains standardized error message templates for consistent validation messaging.
    /// </summary>
    internal static class ErrorMessages
    {
        // Null/Empty validation messages
        public const string ValueCannotBeNull = "The value cannot be null";
        public const string StringCannotBeEmpty = "The string cannot be empty";
        public const string CollectionCannotBeNull = "The collection cannot be null";

        // Type validation messages
        public const string ValueMustBeOfType = "The value must be of type {0}, but was {1}";
        public const string ValueMustBeNumeric = "The value '{0}' is not a valid numeric type";

        // GUID validation messages
        public const string GuidCannotBeEmpty = "The GUID cannot be empty";
        public const string ValueMustBeGuid = "The value must be of type GUID";
        
        // Collection validation messages
        public const string CollectionTooSmall = "The collection length of {0} is less than the minimum required of {1}";
        public const string CollectionTooLarge = "The collection length of {0} is greater than the maximum allowed of {1}";

        // String validation messages
        public const string StringTooShort = "The string length of {0} is less than the minimum required of {1}";
        public const string StringTooLong = "The string length of {0} is greater than the maximum allowed of {1}";
        public const string StringDoesNotMatchPattern = "The string does not match the required pattern: {0}";

        // Range validation messages
        public const string ValueTooSmall = "The value {0} must be greater than {1}";
        public const string ValueTooSmallInclusive = "The value {0} must be greater than or equal to {1}";
        public const string ValueTooLarge = "The value {0} must be less than {1}";
        public const string ValueTooLargeInclusive = "The value {0} must be less than or equal to {1}";

        // Date validation messages
        public const string DateMustBeUtc = "The DateTime must be in UTC";
        public const string DateMustNotBeUtc = "The DateTime should not be in UTC";
        public const string DateMustBeInFuture = "The DateTime must be in the future";
        public const string DateMustBeInPast = "The DateTime must be in the past";
        public const string DateMustBeUtcAndInFuture = "The DateTime must be in UTC and in the future";
        public const string DateMustBeUtcAndInPast = "The DateTime must be in UTC and in the past";
        public const string DateMustBeNonUtcAndInFuture = "The DateTime should not be in UTC and must be in the future";
        public const string DateMustBeNonUtcAndInPast = "The DateTime should not be in UTC and must be in the past";
        public const string DateValidationFailed = "The DateTime did not pass validation. Please check that it meets supported validation rules";
    }
}