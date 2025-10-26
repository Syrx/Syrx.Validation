# Syrx.Validation

> Core validation library with precondition checking for .NET applications

[![NuGet Version](https://img.shields.io/nuget/v/Syrx.Validation.svg)](https://www.nuget.org/packages/Syrx.Validation/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Syrx.Validation.svg)](https://www.nuget.org/packages/Syrx.Validation/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Overview

**Syrx.Validation** is a lightweight, high-performance validation library that provides elegant precondition checking for .NET applications. It transforms verbose multi-line validation statements into clean, readable single-line expressions while giving you complete control over exception handling.

## Features

- ✅ **Fluent Precondition Checking**: Convert multi-line `if-throw` statements into single-line expressions
- ✅ **Flexible Exception Handling**: Support for custom exception types and factories
- ✅ **High Performance**: Near-zero overhead when conditions are met
- ✅ **Modern .NET**: Full support for .NET 8.0+ with nullable reference types
- ✅ **Static Analysis Support**: Enhanced with `DoesNotReturnIf` attributes for better compiler analysis
- ✅ **Zero Dependencies**: No external dependencies required

## Installation

Install via NuGet Package Manager:

```powershell
Install-Package Syrx.Validation
```

Or via .NET CLI:

```bash
dotnet add package Syrx.Validation
```

## Quick Start

```csharp
using static Syrx.Validation.Contract;

public class UserService
{
    public void CreateUser(string email, int age)
    {
        // Validate parameters with clean, readable syntax
        Throw<ArgumentNullException>(string.IsNullOrWhiteSpace(email), "Email is required");
        Throw<ArgumentOutOfRangeException>(age < 0 || age > 150, "Age must be between 0 and 150");
        
        // Process validated data
        ProcessUser(email, age);
    }
}
```

## API Reference

### Contract Class

The `Contract` class provides static methods for precondition checking:

#### Basic Validation

```csharp
// Throw exception if condition is false
Throw<TException>(bool condition, string message);

// Example
Throw<ArgumentNullException>(value == null, "Value cannot be null");
```

#### Formatted Messages

```csharp
// Support for string formatting
Throw<TException>(bool condition, string message, params object[] args);

// Example
Throw<ArgumentException>(value < 0, "Value {0} must be non-negative", value);
```

#### Inner Exception Support

```csharp
// Include inner exceptions
Throw<TException>(bool condition, string message, Exception innerException);
Throw<TException>(bool condition, string message, Exception innerException, params object[] args);

// Example
Throw<InvalidOperationException>(failed, "Operation failed", innerException);
```

#### Exception Factory Pattern

```csharp
// Use factory delegates for complex exception creation
Throw<TException>(bool condition, Func<TException> exceptionFactory);

// Example
Throw<CustomException>(invalid, () => new CustomException("Custom message", additionalData));
```

#### Require Aliases

```csharp
// All Throw methods have Require aliases for readability
Require<ArgumentNullException>(value != null, "Value is required");
```

## Advanced Usage Examples

### Method Parameter Validation

```csharp
using static Syrx.Validation.Contract;

public class OrderService
{
    public decimal CalculateDiscount(decimal orderAmount, string couponCode, DateTime validUntil)
    {
        // Validate all parameters upfront
        Throw<ArgumentOutOfRangeException>(orderAmount <= 0, 
            "Order amount must be positive, got {0}", orderAmount);
        
        Throw<ArgumentException>(string.IsNullOrWhiteSpace(couponCode), 
            "Coupon code cannot be empty");
        
        Throw<ArgumentException>(validUntil < DateTime.Now, 
            "Coupon expired on {0}", validUntil);
        
        return CalculateDiscountInternal(orderAmount, couponCode);
    }
}
```

### Business Rule Validation

```csharp
public class BankAccount
{
    private decimal _balance;
    
    public void Withdraw(decimal amount)
    {
        Throw<ArgumentOutOfRangeException>(amount <= 0, 
            "Withdrawal amount must be positive");
        
        Throw<InvalidOperationException>(_balance < amount, 
            "Insufficient funds. Balance: {0}, Requested: {1}", _balance, amount);
        
        _balance -= amount;
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
Throw<BusinessRuleException>(violatesRule, 
    () => new BusinessRuleException("BR001", "Business rule violation detected"));
```

### Factory Pattern for Complex Scenarios

```csharp
public void ProcessPayment(PaymentRequest request)
{
    Throw<PaymentException>(request.Amount <= 0, () => 
        new PaymentException(
            "INVALID_AMOUNT", 
            $"Payment amount {request.Amount} is invalid",
            request.TransactionId
        ));
    
    Throw<PaymentException>(string.IsNullOrEmpty(request.MerchantId), () =>
        new PaymentException(
            "MISSING_MERCHANT",
            "Merchant ID is required for payment processing",
            request.TransactionId
        ));
}
```

## Performance Characteristics

- **Zero Overhead**: When conditions are `true`, the method calls have minimal performance impact
- **Efficient Exception Creation**: Exception objects are only created when conditions fail
- **Memory Efficient**: No allocations for successful validations
- **Compiler Optimizations**: Benefits from JIT compiler optimizations in hot paths

## Static Analysis Support

The library includes `DoesNotReturnIf` attributes that provide enhanced static analysis:

```csharp
[DoesNotReturnIf(false)]
public static void Throw<T>(bool condition, string message) where T : Exception, new()
```

This enables:
- Better null-state analysis
- Improved unreachable code detection
- Enhanced compiler warnings and suggestions

## Thread Safety

All `Contract` methods are thread-safe as they are static methods with no shared state. Multiple threads can safely call validation methods concurrently.

## Migration from Traditional Validation

### Before (Traditional Approach)

```csharp
public void ProcessOrder(Order order, string customerEmail)
{
    if (order == null)
    {
        throw new ArgumentNullException(nameof(order));
    }
    
    if (string.IsNullOrWhiteSpace(customerEmail))
    {
        throw new ArgumentException("Customer email is required", nameof(customerEmail));
    }
    
    if (order.Items == null || order.Items.Count == 0)
    {
        throw new ArgumentException("Order must contain at least one item", nameof(order));
    }
    
    // Process order...
}
```

### After (With Syrx.Validation)

```csharp
using static Syrx.Validation.Contract;

public void ProcessOrder(Order order, string customerEmail)
{
    Throw<ArgumentNullException>(order == null, nameof(order));
    Throw<ArgumentException>(string.IsNullOrWhiteSpace(customerEmail), "Customer email is required");
    Throw<ArgumentException>(order.Items?.Count == 0, "Order must contain at least one item");
    
    // Process order...
}
```

## Error Handling Best Practices

### 1. Use Appropriate Exception Types

```csharp
// For null arguments
Throw<ArgumentNullException>(value == null, nameof(value));

// For invalid ranges
Throw<ArgumentOutOfRangeException>(index < 0 || index >= array.Length, nameof(index));

// For business rule violations
Throw<InvalidOperationException>(account.IsLocked, "Account is locked");
```

### 2. Provide Meaningful Messages

```csharp
// Good: Specific and actionable
Throw<ArgumentException>(age < 18, "Age must be 18 or older, got {0}", age);

// Avoid: Vague messages
Throw<ArgumentException>(age < 18, "Invalid age");
```

### 3. Use Exception Factories for Complex Cases

```csharp
Throw<ValidationException>(hasErrors, () => 
    new ValidationException("Validation failed", GetValidationErrors()));
```

## Framework Support

- **.NET 8.0**: Full support with nullable reference types and performance optimizations
- **.NET 9.0**: Full support with latest runtime improvements
- **Dependencies**: No external dependencies (uses only System libraries)

## Related Packages

- **[Syrx.Validation.Attributes](https://www.nuget.org/packages/Syrx.Validation.Attributes/)**: Attribute-based model validation companion package

## Contributing

Contributions are welcome! Please read our [Contributing Guidelines](CONTRIBUTING.md) for details on how to submit pull requests, report issues, and suggest improvements.

## License

This project is licensed under the MIT License - see the [license.txt](../license.txt) file for details.

## Changelog

### Version 3.0.0
- **BREAKING CHANGE**: Removed .NET 6.0 support
- Added .NET 9.0 support
- Now targets .NET 8.0 and .NET 9.0

### Version 2.0.0
- Added `DoesNotReturnIf` attributes for enhanced static analysis
- Improved nullable reference type support
- Added `Require` method aliases for better readability
- Performance optimizations
- Enhanced XML documentation

### Version 1.1.0
- Added exception factory delegate support
- Improved error message formatting
- Added inner exception support

### Version 1.0.0
- Initial release with core precondition checking functionality