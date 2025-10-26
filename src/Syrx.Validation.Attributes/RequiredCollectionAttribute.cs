// ============================================================================================================================= 
// author           : david sexton (@sextondjc | sextondjc.com)
// date             : 2015.12.23
// licence          : licensed under the terms of the MIT license. See LICENSE.txt
// =============================================================================================================================
#nullable enable

namespace Syrx.Validation.Attributes
{
    public sealed class RequiredCollectionAttribute : ValidationAttribute
    {
        public int MinimumCount { get; }
        public int MaximumCount { get; }

        public RequiredCollectionAttribute(int minimumCount)
        {
            MinimumCount = minimumCount;
        }

        public RequiredCollectionAttribute(int minimumCount, int maximumCount):this(minimumCount)
        {
            MaximumCount = maximumCount;
        }
        
        public override bool IsValid(object? value)
        {            
            Throw<ArgumentNullException>(value != null, "The object passed to the attribute was null.");            

            // cast to IEnumerable & get length
            var length = ((IEnumerable) value).Cast<object>().Count();

            if (length < MinimumCount)
            {
                // doesn't meet minimum length
                ErrorMessage = string.Format(ErrorMessages.CollectionTooSmall, length, MinimumCount);
                return false;
            }

            // max length is allowed to be zero. 
            if (MaximumCount == ValidationConstants.NoMaximumLimit) return true;
            if (length <= MaximumCount) return true;
            ErrorMessage = string.Format(ErrorMessages.CollectionTooLarge, length, MaximumCount);
            return false;
        }
    }
}
