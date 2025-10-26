// ============================================================================================================================= 
// author           : david sexton (@sextondjc | sextondjc.com)
// date             : 2025.10.25
// licence          : licensed under the terms of the MIT license. See LICENSE.txt
// =============================================================================================================================
#nullable enable

namespace Syrx.Validation.Attributes
{
    /// <summary>
    /// Specifies that a string property must meet length and/or pattern requirements.
    /// </summary>
    public sealed class RequiredStringAttribute : ValidationAttribute
    {
        public int MinimumLength { get; }
        public int MaximumLength { get; }
        public string? Pattern { get; }
        public bool AllowEmpty { get; }
        
        private readonly Regex? _regex;

        /// <summary>
        /// Initializes a new instance with minimum length requirement.
        /// </summary>
        /// <param name="minimumLength">The minimum required length.</param>
        public RequiredStringAttribute(int minimumLength) : this(minimumLength, ValidationConstants.MaximumStringLength)
        {
        }

        /// <summary>
        /// Initializes a new instance with length range requirements.
        /// </summary>
        /// <param name="minimumLength">The minimum required length.</param>
        /// <param name="maximumLength">The maximum allowed length.</param>
        /// <param name="allowEmpty">Whether empty strings are allowed (default: false).</param>
        public RequiredStringAttribute(int minimumLength, int maximumLength, bool allowEmpty = false)
        {
            Throw<ArgumentOutOfRangeException>(minimumLength >= 0, $"Minimum length must be non-negative, got {minimumLength}");
            Throw<ArgumentOutOfRangeException>(maximumLength >= minimumLength, $"Maximum length ({maximumLength}) must be >= minimum length ({minimumLength})");
            
            MinimumLength = minimumLength;
            MaximumLength = maximumLength;
            AllowEmpty = allowEmpty;
        }

        /// <summary>
        /// Initializes a new instance with pattern matching requirement.
        /// </summary>
        /// <param name="pattern">The regular expression pattern that the string must match.</param>
        /// <param name="allowEmpty">Whether empty strings are allowed (default: false).</param>
        public RequiredStringAttribute(string pattern, bool allowEmpty = false)
        {
            Throw<ArgumentNullException>(!string.IsNullOrWhiteSpace(pattern), nameof(pattern));
            
            Pattern = pattern;
            AllowEmpty = allowEmpty;
            MinimumLength = ValidationConstants.MinimumStringLength;
            MaximumLength = ValidationConstants.MaximumStringLength;
            
            try
            {
                _regex = new Regex(pattern, RegexOptions.Compiled);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Invalid regular expression pattern: {pattern}", nameof(pattern), ex);
            }
        }

        /// <summary>
        /// Initializes a new instance with both length and pattern requirements.
        /// </summary>
        /// <param name="minimumLength">The minimum required length.</param>
        /// <param name="maximumLength">The maximum allowed length.</param>
        /// <param name="pattern">The regular expression pattern that the string must match.</param>
        /// <param name="allowEmpty">Whether empty strings are allowed (default: false).</param>
        public RequiredStringAttribute(int minimumLength, int maximumLength, string pattern, bool allowEmpty = false)
            : this(minimumLength, maximumLength, allowEmpty)
        {
            Throw<ArgumentNullException>(!string.IsNullOrWhiteSpace(pattern), nameof(pattern));
            
            Pattern = pattern;
            
            try
            {
                _regex = new Regex(pattern, RegexOptions.Compiled);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Invalid regular expression pattern: {pattern}", nameof(pattern), ex);
            }
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                ErrorMessage = ErrorMessages.ValueCannotBeNull;
                return false;
            }

            if (value is not string stringValue)
            {
                ErrorMessage = string.Format(ErrorMessages.ValueMustBeOfType, "string", value.GetType().Name);
                return false;
            }

            // Check if empty strings are allowed
            if (string.IsNullOrEmpty(stringValue))
            {
                if (!AllowEmpty)
                {
                    ErrorMessage = ErrorMessages.StringCannotBeEmpty;
                    return false;
                }
                return true; // Empty is allowed and valid
            }

            // Check length constraints
            if (stringValue.Length < MinimumLength)
            {
                ErrorMessage = string.Format(ErrorMessages.StringTooShort, stringValue.Length, MinimumLength);
                return false;
            }

            if (stringValue.Length > MaximumLength)
            {
                ErrorMessage = string.Format(ErrorMessages.StringTooLong, stringValue.Length, MaximumLength);
                return false;
            }

            // Check pattern constraint
            if (_regex != null && !_regex.IsMatch(stringValue))
            {
                ErrorMessage = string.Format(ErrorMessages.StringDoesNotMatchPattern, Pattern);
                return false;
            }

            return true;
        }
    }
}