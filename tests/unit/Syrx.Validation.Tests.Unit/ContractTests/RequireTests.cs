// ============================================================================================================================= 
// Additional tests for Contract class to improve coverage
// =============================================================================================================================

namespace Syrx.Validation.Tests.ContractTests
{
    public class RequireMethodTests
    {
        [Fact]
        public void RequireWithMessageSupportsValidCondition()
        {
            const bool condition = true;
            Require<ArgumentException>(condition, "This should not throw");
        }

        [Fact]
        public void RequireWithMessageThrowsOnInvalidCondition()
        {
            var exception = Throws<ArgumentException>(() => Require<ArgumentException>(false, "Require test"));
            Equal("Require test", exception.Message);
        }

        [Fact]
        public void RequireWithFactorySupportsValidCondition()
        {
            const bool condition = true;
            Require<ArgumentException>(condition, () => new ArgumentException("Should not be called"));
        }

        [Fact]
        public void RequireWithFactoryThrowsOnInvalidCondition()
        {
            var exception = Throws<ArgumentException>(() => Require<ArgumentException>(false, () => new ArgumentException("Factory test")));
            Equal("Factory test", exception.Message);
        }
    }

    public class ThrowWithInnerExceptionTests
    {
        [Fact]
        public void ThrowWithInnerExceptionSupportsValidCondition()
        {
            const bool condition = true;
            var innerException = new InvalidOperationException("Inner");
            Throw<ArgumentException>(condition, "Test message", innerException);
        }

        [Fact]
        public void ThrowWithInnerExceptionThrowsOnInvalidCondition()
        {
            var innerException = new InvalidOperationException("Inner exception");
            var exception = Throws<ArgumentException>(() => Throw<ArgumentException>(false, "Test message", innerException));
            Equal("Test message", exception.Message);
            NotNull(exception.InnerException);
            Equal("Inner exception", exception.InnerException.Message);
        }

        [Fact]
        public void ThrowWithInnerExceptionAndArgsThrowsOnInvalidCondition()
        {
            var innerException = new InvalidOperationException("Inner exception");
            var exception = Throws<ArgumentException>(() => Throw<ArgumentException>(false, "Test {0} {1}", innerException, "arg1", "arg2"));
            Equal("Test arg1 arg2", exception.Message);
            NotNull(exception.InnerException);
            Equal("Inner exception", exception.InnerException.Message);
        }
    }

    public class MissingCoverageTests
    {
        [Fact]
        public void ThrowWithInnerExceptionOverloadSupportsValidCondition()
        {
            var innerException = new InvalidOperationException("Inner");
            Throw<ArgumentException>(true, "Message {0}", innerException, "arg1");
        }

        [Fact]
        public void ThrowWithInnerExceptionOverloadThrowsOnInvalidCondition()
        {
            var innerException = new InvalidOperationException("Inner exception");
            var exception = Throws<ArgumentException>(() => 
                Throw<ArgumentException>(false, "Message {0}", innerException, "arg1"));
            Equal("Message arg1", exception.Message);
            NotNull(exception.InnerException);
            Equal("Inner exception", exception.InnerException.Message);
        }

        [Fact]
        public void RequireWithMessageAlias()
        {
            var exception = Throws<ArgumentException>(() => 
                Require<ArgumentException>(false, "Required condition failed"));
            Equal("Required condition failed", exception.Message);
        }

        [Fact]
        public void RequireWithFactoryAlias()
        {
            var exception = Throws<ArgumentException>(() => 
                Require<ArgumentException>(false, () => new ArgumentException("Factory message")));
            Equal("Factory message", exception.Message);
        }

        [Fact]
        public void ThrowWithInnerExceptionOverloadWithFormatting()
        {
            var innerException = new InvalidOperationException("Inner exception");
            var exception = Throws<ArgumentException>(() => 
                Throw<ArgumentException>(false, "Message {0} with {1}", innerException, "arg1", "arg2"));
            Equal("Message arg1 with arg2", exception.Message);
            NotNull(exception.InnerException);
            Equal("Inner exception", exception.InnerException.Message);
        }

        [Fact]
        public void ThrowWithInnerExceptionAndNoArgs()
        {
            var innerException = new InvalidOperationException("Inner exception");
            var exception = Throws<ArgumentException>(() => 
                Throw<ArgumentException>(false, "Simple message", innerException));
            Equal("Simple message", exception.Message);
            NotNull(exception.InnerException);
            Equal("Inner exception", exception.InnerException.Message);
        }

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
        public void RequireStringWithFormatting()
        {
            var exception = Throws<ArgumentException>(() => 
                Require<ArgumentException>(false, "Value {0} is invalid", "test"));
            Equal("Value test is invalid", exception.Message);
        }

        [Fact]
        public void RequireWithFactoryReturnsCorrectException()
        {
            var customException = new InvalidOperationException("Custom message");
            var exception = Throws<InvalidOperationException>(() => 
                Require<InvalidOperationException>(false, () => customException));
            Same(customException, exception);
        }

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
    }
}