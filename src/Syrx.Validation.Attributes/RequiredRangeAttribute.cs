// ============================================================================================================================= 
// author           : david sexton (@sextondjc | sextondjc.com)
// date             : 2025.10.25
// licence          : licensed under the terms of the MIT license. See LICENSE.txt
// =============================================================================================================================
#nullable enable

namespace Syrx.Validation.Attributes
{
    /// <summary>
    /// Specifies that a numeric property must be within a specified range.
    /// Supports all numeric types including int, long, double, decimal, etc.
    /// </summary>
    public sealed class RequiredRangeAttribute : ValidationAttribute
    {
        public double Minimum { get; }
        public double Maximum { get; }
        public bool MinimumIsExclusive { get; }
        public bool MaximumIsExclusive { get; }

        /// <summary>
        /// Initializes a new instance with inclusive range bounds.
        /// </summary>
        /// <param name="minimum">The minimum allowed value (inclusive).</param>
        /// <param name="maximum">The maximum allowed value (inclusive).</param>
        public RequiredRangeAttribute(double minimum, double maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
            MinimumIsExclusive = false;
            MaximumIsExclusive = false;
        }

        /// <summary>
        /// Initializes a new instance with configurable range bounds.
        /// </summary>
        /// <param name="minimum">The minimum allowed value.</param>
        /// <param name="maximum">The maximum allowed value.</param>
        /// <param name="minimumIsExclusive">Whether the minimum bound is exclusive.</param>
        /// <param name="maximumIsExclusive">Whether the maximum bound is exclusive.</param>
        public RequiredRangeAttribute(double minimum, double maximum, bool minimumIsExclusive, bool maximumIsExclusive)
        {
            Minimum = minimum;
            Maximum = maximum;
            MinimumIsExclusive = minimumIsExclusive;
            MaximumIsExclusive = maximumIsExclusive;
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                ErrorMessage = ErrorMessages.ValueCannotBeNull;
                return false;
            }

            // Try to convert to double for comparison
            double numericValue;
            try
            {
                numericValue = Convert.ToDouble(value);
            }
            catch (Exception)
            {
                ErrorMessage = string.Format(ErrorMessages.ValueMustBeNumeric, value);
                return false;
            }

            // Check minimum bound
            if (MinimumIsExclusive)
            {
                if (numericValue <= Minimum)
                {
                    ErrorMessage = string.Format(ErrorMessages.ValueTooSmall, numericValue, Minimum);
                    return false;
                }
            }
            else
            {
                if (numericValue < Minimum)
                {
                    ErrorMessage = string.Format(ErrorMessages.ValueTooSmallInclusive, numericValue, Minimum);
                    return false;
                }
            }

            // Check maximum bound
            if (MaximumIsExclusive)
            {
                if (numericValue >= Maximum)
                {
                    ErrorMessage = string.Format(ErrorMessages.ValueTooLarge, numericValue, Maximum);
                    return false;
                }
            }
            else
            {
                if (numericValue > Maximum)
                {
                    ErrorMessage = string.Format(ErrorMessages.ValueTooLargeInclusive, numericValue, Maximum);
                    return false;
                }
            }

            return true;
        }
    }
}