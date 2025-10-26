# Architecture Documentation

## Overview

Syrx.Validation is a lightweight, high-performance validation library for .NET applications, designed around two core concepts:

1. **Precondition Checking** - Runtime validation with immediate exception throwing
2. **Attribute-Based Validation** - Declarative model validation using custom attributes

## Solution Structure

```
Syrx.Validation/
├── src/
│   ├── Syrx.Validation/                    # Core validation library
│   │   ├── Contract.cs                     # Precondition checking
│   │   └── Syrx.Validation.csproj
│   └── Syrx.Validation.Attributes/        # Attribute-based validation
│       ├── RequiredCollectionAttribute.cs  # Collection validation
│       ├── RequiredDateAttribute.cs        # DateTime validation
│       ├── RequiredGuidAttribute.cs        # GUID validation
│       ├── RequiredRangeAttribute.cs       # Numeric range validation
│       ├── RequiredStringAttribute.cs      # String validation
│       ├── ValidationConstants.cs          # Error messages & constants
│       ├── Validator.cs                    # Validation orchestrator
│       └── Syrx.Validation.Attributes.csproj
├── tests/
│   └── unit/
│       ├── Syrx.Validation.Tests.Unit/         # Core library tests
│       └── Syrx.Validation.Attributes.Tests.Unit/ # Attribute tests
└── docs/                                   # Documentation
```

## Core Components

### 1. Contract Class (Syrx.Validation)

The `Contract` class provides static methods for precondition checking, enabling clean, readable validation code.

#### Design Principles
- **Fail-Fast**: Validate early and throw immediately on invalid conditions
- **Fluent Syntax**: Single-line validation statements
- **Flexible Exception Handling**: Support for any exception type
- **Performance**: Near-zero overhead when conditions are satisfied

#### Method Signatures
```csharp
public static class Contract
{
    // Basic validation with custom exception type
    public static void Throw<T>(bool condition, string message) where T : Exception, new();
    
    // With formatted message
    public static void Throw<T>(bool condition, string message, params object[] args) where T : Exception, new();
    
    // With inner exception
    public static void Throw<T>(bool condition, string message, Exception innerException) where T : Exception, new();
    public static void Throw<T>(bool condition, string message, Exception innerException, params object[] args) where T : Exception, new();
    
    // With exception factory
    public static void Throw<T>(bool condition, Func<T> exceptionFactory) where T : Exception;
    
    // Require aliases (same signatures as Throw)
    public static void Require<T>(...) where T : Exception, new();
}
```

#### Static Analysis Support
- Enhanced with `[DoesNotReturnIf(false)]` attributes
- Provides better null-state analysis for consuming code
- Improves compiler warnings and unreachable code detection

### 2. Validation Attributes (Syrx.Validation.Attributes)

#### Validator Class
The `Validator` class serves as the central orchestrator for attribute-based validation.

```csharp
public static class Validator
{
    public static ValidationResult Validate(object instance);
}
```

**Key Features:**
- Reflection-based property scanning
- Attribute discovery and execution
- Aggregated result collection
- Thread-safe operation

#### ValidationResult Class
```csharp
public class ValidationResult
{
    public bool IsValid { get; }
    public IReadOnlyList<ValidationError> ValidationErrors { get; }
}

public class ValidationError
{
    public string PropertyName { get; }
    public string ErrorMessage { get; }
}
```

#### Validation Attributes Architecture

All validation attributes inherit from `ValidationAttribute` and implement the `IsValid` method:

```csharp
public abstract class ValidationAttribute : Attribute
{
    public abstract bool IsValid(object? value);
    public virtual string FormatErrorMessage(string name);
}
```

##### RequiredStringAttribute
- **Purpose**: String validation with length and pattern constraints
- **Features**: Min/max length, regex pattern matching, combined validation
- **Performance**: Compiled regex patterns cached for reuse

```csharp
public class RequiredStringAttribute : ValidationAttribute
{
    public int MinLength { get; set; }
    public int MaxLength { get; set; }
    public string? Pattern { get; set; }
    
    // Compiled regex cached for performance
    private Regex? _compiledRegex;
}
```

##### RequiredRangeAttribute
- **Purpose**: Numeric range validation for all numeric types
- **Features**: Inclusive/exclusive boundaries, universal numeric support
- **Type Handling**: Uses `Convert.ToDouble()` for universal numeric comparison

```csharp
public class RequiredRangeAttribute : ValidationAttribute
{
    public double MinValue { get; set; } = double.MinValue;
    public double MaxValue { get; set; } = double.MaxValue;
    public bool IsMinInclusive { get; set; } = true;
    public bool IsMaxInclusive { get; set; } = true;
}
```

##### RequiredDateAttribute
- **Purpose**: DateTime validation with temporal constraints
- **Options**: Past, future, today combinations

```csharp
public enum RequiredDateOptions
{
    None,
    PastOnly,
    FutureOnly,
    TodayOrPast,
    TodayOrFuture
}
```

##### RequiredGuidAttribute
- **Purpose**: GUID validation (not Guid.Empty)
- **Implementation**: Simple, efficient empty GUID check

##### RequiredCollectionAttribute
- **Purpose**: Collection validation with count constraints
- **Support**: Any `IEnumerable` implementation
- **Features**: Min/max count validation

### 3. ValidationConstants Class

Centralized constants for consistent error messaging and default values:

```csharp
public static class ValidationConstants
{
    // Default values
    public const int DefaultMinLength = 1;
    public const int DefaultMaxLength = int.MaxValue;
    public const int DefaultMinCount = 1;
    public const int DefaultMaxCount = int.MaxValue;
    
    // Error message templates
    public const string RequiredStringMessage = "The field {0} is required and must be between {1} and {2} characters.";
    public const string RequiredRangeMessage = "The field {0} must be between {1} and {2}.";
    // ... additional templates
}
```

## Design Patterns

### 1. Static Factory Pattern
The `Contract` class uses static factory methods to create and throw exceptions, providing a clean API without requiring instantiation.

### 2. Attribute Pattern
Validation attributes follow the standard .NET attribute pattern, allowing declarative validation that integrates with reflection-based processing.

### 3. Template Method Pattern
The base `ValidationAttribute` class defines the validation workflow, while concrete implementations provide specific validation logic in the `IsValid` method.

### 4. Caching Pattern
Performance-critical components use caching:
- Compiled regex patterns in `RequiredStringAttribute`
- Property reflection metadata in `Validator`

## Performance Considerations

### 1. Contract Methods
- **Condition True**: Near-zero overhead (simple boolean check)
- **Condition False**: Exception creation overhead (unavoidable)
- **Message Formatting**: Only performed when exceptions are thrown

### 2. Attribute Validation
- **Reflection Caching**: Property information cached to reduce reflection overhead
- **Compiled Regex**: Pattern validation uses compiled regex for better performance
- **Minimal Allocations**: Validation process designed to minimize memory allocations

### 3. Type Conversion
- **RequiredRangeAttribute**: Uses `Convert.ToDouble()` for universal numeric support
- **Trade-off**: Slight performance cost for type flexibility

## Thread Safety

### Contract Class
- **Thread-Safe**: Static methods with no shared mutable state
- **Concurrent Access**: Multiple threads can safely call validation methods

### Validation Attributes
- **Immutable**: Attribute instances are immutable after construction
- **Thread-Safe**: Safe for concurrent validation operations
- **Regex Compilation**: One-time compilation with thread-safe access

### Validator Class
- **Thread-Safe**: Uses thread-safe reflection operations
- **No Shared State**: Each validation operation is independent

## Extensibility

### Custom Validation Attributes
```csharp
public class CustomValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        // Custom validation logic
        return value != null && IsValidCustomRule(value);
    }
    
    public override string FormatErrorMessage(string name)
    {
        return $"The field {name} failed custom validation.";
    }
}
```

### Custom Exception Types
```csharp
public class BusinessRuleException : Exception
{
    public string RuleCode { get; }
    
    public BusinessRuleException(string ruleCode, string message) : base(message)
    {
        RuleCode = ruleCode;
    }
}

// Usage
Contract.Throw<BusinessRuleException>(violatesRule, () => 
    new BusinessRuleException("BR001", "Business rule violated"));
```

## Integration Patterns

### ASP.NET Core Integration
```csharp
public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var parameter in context.ActionArguments.Values)
        {
            var result = Validator.Validate(parameter);
            if (!result.IsValid)
            {
                context.Result = new BadRequestObjectResult(result.ValidationErrors);
                return;
            }
        }
    }
}
```

### Dependency Injection Integration
```csharp
public interface IValidationService
{
    ValidationResult Validate<T>(T instance) where T : class;
}

public class ValidationService : IValidationService
{
    public ValidationResult Validate<T>(T instance) where T : class
    {
        Contract.Throw<ArgumentNullException>(instance == null, nameof(instance));
        return Validator.Validate(instance);
    }
}
```

## Testing Architecture

### Test Structure
- **Unit Tests**: Focused on individual attribute behavior
- **Integration Tests**: End-to-end validation scenarios
- **Coverage Tests**: Ensure all code paths are exercised

### Test Patterns
```csharp
[Theory]
[InlineData(null, false)]  // Null values
[InlineData("", false)]    // Empty values
[InlineData("valid", true)] // Valid values
public void IsValid_ReturnsExpectedResult(string input, bool expected)
{
    var attribute = new RequiredStringAttribute();
    var result = attribute.IsValid(input);
    Assert.Equal(expected, result);
}
```

## Error Handling Strategy

### 1. Consistent Error Messages
- Standardized templates in `ValidationConstants`
- Property name substitution for context
- Localization-friendly message format

### 2. Exception Hierarchy
- Use appropriate built-in exception types
- Support for custom exception types
- Inner exception preservation

### 3. Validation Result Aggregation
- Multiple validation errors collected in single result
- Non-blocking validation (continues after first failure)
- Structured error information

## Deployment Considerations

### NuGet Package Structure
- **Syrx.Validation**: Core precondition checking (minimal dependencies)
- **Syrx.Validation.Attributes**: Extended validation (depends on core package)

### Framework Support
- **.NET 8.0**: Full support with nullable reference types and performance optimizations
- **.NET 9.0**: Latest runtime improvements and modern language features
- **Multi-targeting**: Single codebase for multiple framework versions

### Backward Compatibility
- Semantic versioning for breaking changes
- Obsolete attribute for deprecated features
- Migration guides for major versions

This architecture provides a solid foundation for validation operations while maintaining flexibility, performance, and extensibility for diverse .NET applications.