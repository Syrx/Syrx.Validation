// ============================================================================================================================= 
// author           : david sexton (@sextondjc | sextondjc.com)
// date             : 2015.12.23
// modified         : 2018-08-25 (15:42)
// licence          : licensed under the terms of the MIT license. See LICENSE.txt
// =============================================================================================================================

namespace Syrx.Validation.Tests.ContractTests
{
    public class Throw
    {
        [Fact]
        public void InvalidConditionThrowsArgumentException()
        {
            var exception = Throws<ArgumentException>(() => Throw<ArgumentException>(false, "Validation test"));
            Equal("Validation test", exception.Message);
            Null(exception.InnerException);
        }

        [Fact]
        public void SupportsArbitraryExceptionFactoryNoParams()
        {
            Throw(true, () => new Exception());
        }

        [Fact]
        public void SupportsArbitraryExceptionWithInnerExceptionAndNoParams()
        {
            var innerException = new Exception("Inner exception");
            Throw<Exception>(true, "Test", innerException);
        }

        [Fact]
        public void SupportsArbitraryExceptionWithInnerExceptionAndParams()
        {
            var innerException = new Exception("Inner exception");
            Throw<Exception>(true, "Test {0} {1}", innerException, 1, "2");
        }

        [Fact]
        public void SupportsArbitraryExceptionWithoutInnerExceptionAndParams()
        {
            Throw<Exception>(true, "Test {0} {1}", 1, "2");
        }

        [Fact]
        public void SupportsArbitraryExceptionWithoutInnerExceptionOrParams()
        {
            Throw<Exception>(true, "Test");
        }

        [Fact]
        public void ThrowsArbitraryExceptionFromFactoryWithNoParams()
        {
            var result = Throws<Exception>(() => Throw(false, () => new Exception("unit test")));
            Equal("unit test", result.Message);
        }

        [Fact]
        public void ThrowsArbitraryExceptionWithInnerExceptionAndNoParams()
        {
            var innerException = new Exception("Inner exception");
            var exception = Throws<Exception>(() => Throw<Exception>(false, "Test", innerException));
            Equal("Test", exception.Message);
            NotNull(exception.InnerException);
        }

        [Fact]
        public void ThrowsArbitraryExceptionWithInnerExceptionAndParams()
        {
            var innerException = new Exception("Inner exception");
            var exception = Throws<Exception>(() => Throw<Exception>(false, "Test {0} {1}", innerException, 1, "2"));
            Equal("Test 1 2", exception.Message);
            NotNull(exception.InnerException);
        }

        [Fact]
        public void ThrowsArbitraryExceptionWithoutInnerExceptionAndParams()
        {
            var exception = Throws<Exception>(() => Throw<Exception>(false, "Test {0} {1}", 1, "2"));
            Equal("Test 1 2", exception.Message);
            Null(exception.InnerException);
        }

        [Fact]
        public void ThrowsArbitraryExceptionWithoutInnerExceptionOrParams()
        {
            var exception = Throws<Exception>(() => Throw<Exception>(false, "Test"));
            Equal("Test", exception.Message);
            Null(exception.InnerException);
        }

        [Fact]
        public void ValidConditionWillNotThrowAnException()
        {
            const bool condition = true;
            Throw<Exception>(condition, "Validation test");
        }
    }
}