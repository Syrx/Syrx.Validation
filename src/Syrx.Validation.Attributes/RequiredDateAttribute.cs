// ============================================================================================================================= 
// author           : david sexton (@sextondjc | sextondjc.com)
// date             : 2015.12.23
// licence          : licensed under the terms of the MIT license. See LICENSE.txt
// =============================================================================================================================
#nullable enable

namespace Syrx.Validation.Attributes
{
    /// <summary>
    /// Specifies that a DateTime property must be populated and,
    /// optionally, must be UTC time and/or in the future.  
    /// The default behaviour is to assume that the value is 
    /// UTC. 
    /// </summary>
    public class RequiredDateAttribute : ValidationAttribute
    {
        public RequiredDateOptions Options { get; }

        public RequiredDateAttribute()
        {
            // default to UTC. 
            Options = UtcOnly;
        }

        public RequiredDateAttribute(RequiredDateOptions options)
        {
            Options = options;
        }
        
        public override bool IsValid(object? value)
        {
            // preconditions. 
            Throw<ArgumentNullException>(value != null, "A value must be supplied to the RequiredDateAttribute.");
            Throw<ArgumentException>(value.GetType() == typeof(DateTime), "The value supplied to the RequiredDateAttribute ({0}) was not a DateTime type.", value);

            var result = false;                        
            var toValidate = Convert.ToDateTime(value);
            
            // switch on our supported options. 
            switch (Options)
            {
                case None:
                    result = true;
                    break;
                case NotUtc:
                    result = (toValidate.Kind != DateTimeKind.Utc);
                    ErrorMessage = ErrorMessages.DateMustNotBeUtc;
                    break;
                case UtcOnly:
                    result = (toValidate.Kind == DateTimeKind.Utc);
                    ErrorMessage = ErrorMessages.DateMustBeUtc;
                    break;
                case FutureOnly:
                case UtcOnly | FutureOnly:
                    result = (toValidate > DateTime.UtcNow);
                    ErrorMessage = ErrorMessages.DateMustBeUtcAndInFuture;
                    break;
                case PastOnly:
                case UtcOnly | PastOnly:
                    result = (toValidate < DateTime.UtcNow);
                    ErrorMessage = ErrorMessages.DateMustBeUtcAndInPast;
                    break;                
                case NotUtc | FutureOnly:
                    result = ((toValidate.Kind != DateTimeKind.Utc) && (toValidate > DateTime.Now));
                    ErrorMessage = ErrorMessages.DateMustBeNonUtcAndInFuture;
                    break;
                case NotUtc | PastOnly:
                    result = ((toValidate.Kind != DateTimeKind.Utc) && (toValidate < DateTime.Now));
                    ErrorMessage = ErrorMessages.DateMustBeNonUtcAndInPast;
                    break;
                default:
                    // some crazy case which isn't supported.
                    ErrorMessage = ErrorMessages.DateValidationFailed;                    
                    break;
                
            }

            return result;
        }
    }
}
