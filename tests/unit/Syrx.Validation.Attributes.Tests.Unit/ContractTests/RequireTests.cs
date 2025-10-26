// =============================================================================================================================
// Contract Require method tests to achieve full coverage
// =============================================================================================================================
namespace Syrx.Validation.Attributes.Tests.Unit.ContractTests
{
    public class RequireMethodTests
    {
        [Fact]
        public void RequireWithMessageAndArgsThrowsCorrectException()
        {
            var exception = Throws<ArgumentException>(() => 
                Require<ArgumentException>(false, "Message {0} and {1}", "param1", "param2"));
            Equal("Message param1 and param2", exception.Message);
        }

        [Fact]
        public void RequireWithFactoryThrowsCorrectException()
        {
            var exception = Throws<ArgumentException>(() => 
                Require<ArgumentException>(false, () => new ArgumentException("Direct factory call")));
            Equal("Direct factory call", exception.Message);
        }

        [Fact]
        public void RequireWithMessageSupportsValidCondition()
        {
            const bool condition = true;
            Require<ArgumentException>(condition, "This should not throw", "arg1", "arg2");
        }

        [Fact]
        public void RequireWithFactorySupportsValidCondition()
        {
            const bool condition = true;
            Require<ArgumentException>(condition, () => new ArgumentException("Should not be called"));
        }
    }

    public class ThrowWithInnerExceptionTests
    {
        [Fact]
        public void ThrowWithInnerExceptionAndArgsThrowsCorrectException()
        {
            var innerException = new InvalidOperationException("Inner exception");
            var exception = Throws<ArgumentException>(() => 
                Throw<ArgumentException>(false, "Message {0} {1}", innerException, "arg1", "arg2"));
            Equal("Message arg1 arg2", exception.Message);
            NotNull(exception.InnerException);
            Equal("Inner exception", exception.InnerException.Message);
        }

        [Fact]
        public void ThrowWithInnerExceptionSupportsValidCondition()
        {
            const bool condition = true;
            var innerException = new InvalidOperationException("Inner");
            Throw<ArgumentException>(condition, "Test message", innerException, "arg1");
        }

        [Fact]
        public void ThrowWithInnerExceptionAndEmptyArgsThrowsCorrectException()
        {
            var innerException = new InvalidOperationException("Inner exception");
            var exception = Throws<ArgumentException>(() => 
                Throw<ArgumentException>(false, "Simple message", innerException, new object[0]));
            Equal("Simple message", exception.Message);
            NotNull(exception.InnerException);
            Equal("Inner exception", exception.InnerException.Message);
        }
    }
}
