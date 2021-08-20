using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Xunit.Assert;

namespace Syrx.Validation.Attributes.Tests.Unit
{
    public static class ExceptionExtensions
    {
        public static void ArgumentNull(this Exception exception, string parameterName)
        {
            exception.HasMessage($"Value cannot be null. (Parameter '{parameterName}')");
        }
        
        public static void ArgumentOutOfRange(this Exception exception, string parameterName)
        {
            exception.HasMessage($"Specified argument was out of the range of valid values. (Parameter '{parameterName}')");
        }

        public static void HasMessage(this Exception exception, string message)
        {
            Equal(message, exception.Message);
        }
    }
}
