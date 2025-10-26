// ============================================================================================================================= 
// author           : david sexton (@sextondjc | sextondjc.com)
// date             : 2015.12.23
// licence          : licensed under the terms of the MIT license. See LICENSE.txt
// =============================================================================================================================
#nullable enable

namespace Syrx.Validation.Attributes
{
    /// <summary>
    /// Specifies that a required GUID property must be 
    /// populated with a non-empty GUID.
    /// </summary>
    public class RequiredGuidAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            // checks:
            // 1. not null
            // 2. is typeof(Guid)
            // 3. !Guid.Empty

            var result = false;

            if(value != null)
            {
                if(value is Guid guid)
                {
                    if(guid != Guid.Empty)
                    {
                        result = true;
                    }
                    else
                    {
                        ErrorMessage = ErrorMessages.GuidCannotBeEmpty;
                    }
                }
                else
                {
                    ErrorMessage = ErrorMessages.ValueMustBeGuid;
                }
            }
            else
            {
                ErrorMessage = ErrorMessages.ValueCannotBeNull;
            }
            
            return result;
        }
    }
}
