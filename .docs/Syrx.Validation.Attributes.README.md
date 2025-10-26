# Syrx.Validation.Attributes

> Comprehensive attribute-based validation framework for .NET applications

[![NuGet Version](https://img.shields.io/nuget/v/Syrx.Validation.Attributes.svg)](https://www.nuget.org/packages/Syrx.Validation.Attributes/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Syrx.Validation.Attributes.svg)](https://www.nuget.org/packages/Syrx.Validation.Attributes/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Overview

**Syrx.Validation.Attributes** provides a comprehensive set of validation attributes for declarative model validation in .NET applications. It offers a clean, attribute-based approach to validating object properties with support for strings, numbers, dates, GUIDs, and collections.

## Features

- ✅ **Comprehensive Validation**: String, numeric, date, GUID, and collection validation
- ✅ **Advanced String Validation**: Length constraints and regex pattern matching
- ✅ **Flexible Numeric Ranges**: Support for all numeric types with inclusive/exclusive boundaries
- ✅ **Date Validation Options**: Past, future, and relative date validation
- ✅ **Collection Constraints**: Min/max count validation for any collection type
- ✅ **High Performance**: Optimized reflection and compiled regex patterns
- ✅ **Consistent Error Messages**: Standardized, localization-friendly error messages
- ✅ **Modern .NET**: Full support for .NET 8.0+ with nullable reference types

## Installation

Install via NuGet Package Manager:

```powershell
Install-Package Syrx.Validation.Attributes
```

Or via .NET CLI:

```bash
dotnet add package Syrx.Validation.Attributes
```

## Quick Start

```csharp
public class User
{
    [RequiredString(MinLength = 2, MaxLength = 50)]
    public string FirstName { get; set; } = string.Empty;
    
    [RequiredString(Pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    public string Email { get; set; } = string.Empty;
    
    [RequiredRange(MinValue = 18, MaxValue = 120)]
    public int Age { get; set; }
    
    [RequiredDate(RequiredDateOptions.FutureOnly)]
    public DateTime AppointmentDate { get; set; }
    
    [RequiredGuid]
    public Guid UserId { get; set; }
    
    [RequiredCollection(MinCount = 1)]
    public List<string> Tags { get; set; } = new();
}

// Validate the model
var user = new User { /* populate properties */ };
var result = Validator.Validate(user);

if (!result.IsValid)
{
    foreach (var error in result.ValidationErrors)
    {
        Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
    }
}
```

## Validation Attributes

### RequiredStringAttribute

Validates string properties with length and pattern constraints.

#### Basic Usage

```csharp
[RequiredString] // Not null or empty
public string Name { get; set; } = string.Empty;
```

#### Length Validation

```csharp
[RequiredString(MinLength = 5)]
public string Username { get; set; } = string.Empty;

[RequiredString(MaxLength = 100)]
public string Description { get; set; } = string.Empty;

[RequiredString(MinLength = 8, MaxLength = 50)]
public string Password { get; set; } = string.Empty;
```

#### Pattern Validation

```csharp
[RequiredString(Pattern = @"^\d+$")] // Numbers only
public string PhoneNumber { get; set; } = string.Empty;

[RequiredString(Pattern = @"^[A-Z][a-z]+$")] // Capitalized words
public string FirstName { get; set; } = string.Empty;

[RequiredString(Pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")] // Email
public string Email { get; set; } = string.Empty;
```

#### Combined Validation

```csharp
[RequiredString(MinLength = 5, MaxLength = 20, Pattern = @"^[A-Za-z0-9]+$")]
public string Username { get; set; } = string.Empty; // 5-20 alphanumeric characters
```

### RequiredRangeAttribute

Validates numeric properties within specified ranges. Supports all numeric types.

#### Basic Range Validation

```csharp
[RequiredRange(MinValue = 0)] // Non-negative
public int Count { get; set; }

[RequiredRange(MaxValue = 100)] // Maximum value
public int Percentage { get; set; }

[RequiredRange(MinValue = 1, MaxValue = 10)] // Range
public int Rating { get; set; }
```

#### Exclusive Boundaries

```csharp
[RequiredRange(MinValue = 0, IsMinInclusive = false)] // Greater than 0
public decimal Price { get; set; }

[RequiredRange(MaxValue = 100, IsMaxInclusive = false)] // Less than 100
public double Temperature { get; set; }

[RequiredRange(MinValue = 0, MaxValue = 1, IsMinInclusive = false, IsMaxInclusive = false)]
public double Probability { get; set; } // Between 0 and 1 (exclusive)
```

#### Different Numeric Types

```csharp
[RequiredRange(MinValue = 1, MaxValue = long.MaxValue)]
public long Id { get; set; }

[RequiredRange(MinValue = 0.01, MaxValue = 999999.99)]
public decimal Amount { get; set; }

[RequiredRange(MinValue = -273.15)] // Absolute zero
public float TemperatureInCelsius { get; set; }
```

### RequiredDateAttribute

Validates DateTime properties with various temporal constraints.

#### Basic Date Validation

```csharp
[RequiredDate] // Not default DateTime (DateTime.MinValue)
public DateTime CreatedDate { get; set; }
```

#### Temporal Constraints

```csharp
[RequiredDate(RequiredDateOptions.PastOnly)] // Must be in the past
public DateTime BirthDate { get; set; }

[RequiredDate(RequiredDateOptions.FutureOnly)] // Must be in the future
public DateTime AppointmentDate { get; set; }

[RequiredDate(RequiredDateOptions.TodayOrFuture)] // Today or future
public DateTime EventDate { get; set; }

[RequiredDate(RequiredDateOptions.TodayOrPast)] // Today or past
public DateTime CompletionDate { get; set; }
```

### RequiredGuidAttribute

Validates Guid properties to ensure they are not empty.

```csharp
[RequiredGuid] // Not Guid.Empty
public Guid UserId { get; set; }

[RequiredGuid]
public Guid OrderId { get; set; }
```

### RequiredCollectionAttribute

Validates collection properties (arrays, lists, IEnumerable, etc.).

#### Basic Collection Validation

```csharp
[RequiredCollection] // Not null or empty
public List<string> Tags { get; set; } = new();
```

#### Count Constraints

```csharp
[RequiredCollection(MinCount = 1)] // At least one item
public string[] Categories { get; set; } = Array.Empty<string>();

[RequiredCollection(MaxCount = 10)] // Maximum items
public List<string> RecentSearches { get; set; } = new();

[RequiredCollection(MinCount = 1, MaxCount = 5)] // Range of items
public HashSet<string> Keywords { get; set; } = new();
```

## Validator Class

The `Validator` class provides static methods for validating objects decorated with validation attributes.

### Basic Validation

```csharp
public static ValidationResult Validate(object instance)
```

Example:

```csharp
var person = new Person
{
    FirstName = "John",
    Email = "john@example.com",
    Age = 25
};

ValidationResult result = Validator.Validate(person);

if (result.IsValid)
{
    Console.WriteLine("Validation passed!");
}
else
{
    Console.WriteLine("Validation failed:");
    foreach (var error in result.ValidationErrors)
    {
        Console.WriteLine($"- {error.PropertyName}: {error.ErrorMessage}");
    }
}
```

## ValidationResult Class

Contains the results of validation operations.

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

## Advanced Usage Examples

### Complex Model Validation

```csharp
public class Order
{
    [RequiredGuid]
    public Guid OrderId { get; set; }
    
    [RequiredString(MinLength = 1, MaxLength = 200)]
    public string Description { get; set; } = string.Empty;
    
    [RequiredRange(MinValue = 0.01, MaxValue = 999999.99)]
    public decimal TotalAmount { get; set; }
    
    [RequiredDate(RequiredDateOptions.FutureOnly)]
    public DateTime DeliveryDate { get; set; }
    
    [RequiredCollection(MinCount = 1, MaxCount = 100)]
    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem
{
    [RequiredString(MinLength = 1, MaxLength = 100)]
    public string ProductName { get; set; } = string.Empty;
    
    [RequiredRange(MinValue = 1)]
    public int Quantity { get; set; }
    
    [RequiredRange(MinValue = 0)]
    public decimal UnitPrice { get; set; }
}
```

### Service Layer Integration

```csharp
public class UserService
{
    public async Task<User> CreateUserAsync(CreateUserRequest request)
    {
        // Validate the request model
        var validationResult = Validator.Validate(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.ValidationErrors.Select(e => e.ErrorMessage));
            throw new ValidationException($"Invalid user data: {errors}");
        }
        
        // Process validated request
        return await CreateUserInternalAsync(request);
    }
}

public class CreateUserRequest
{
    [RequiredString(MinLength = 2, MaxLength = 50)]
    public string FirstName { get; set; } = string.Empty;
    
    [RequiredString(MinLength = 2, MaxLength = 50)]
    public string LastName { get; set; } = string.Empty;
    
    [RequiredString(Pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    public string Email { get; set; } = string.Empty;
    
    [RequiredRange(MinValue = 13, MaxValue = 120)]
    public int Age { get; set; }
    
    [RequiredDate(RequiredDateOptions.PastOnly)]
    public DateTime DateOfBirth { get; set; }
    
    [RequiredCollection(MinCount = 1, MaxCount = 10)]
    public List<string> Interests { get; set; } = new();
}
```

### Web API Integration

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var validationResult = Validator.Validate(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.ValidationErrors.ToDictionary(
                e => e.PropertyName, 
                e => new[] { e.ErrorMessage }
            );
            return BadRequest(new { Errors = errors });
        }
        
        // Process the validated request
        var user = userService.CreateUser(request);
        return Ok(user);
    }
}
```

## Common Patterns

### Phone Number Validation

```csharp
[RequiredString(Pattern = @"^\+?[\d\s\-\(\)]{10,}$")]
public string PhoneNumber { get; set; } = string.Empty;
```

### URL Validation

```csharp
[RequiredString(Pattern = @"^https?://[^\s/$.?#].[^\s]*$")]
public string Website { get; set; } = string.Empty;
```

### Credit Card Validation (Basic)

```csharp
[RequiredString(Pattern = @"^\d{13,19}$")]
public string CreditCardNumber { get; set; } = string.Empty;
```

### Password Strength

```csharp
[RequiredString(MinLength = 8, Pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")]
public string Password { get; set; } = string.Empty; // At least 8 chars, 1 upper, 1 lower, 1 digit, 1 special
```

## Error Messages

All validation attributes provide clear, consistent error messages:

### RequiredString
- Basic: "The field {PropertyName} is required."
- Length only: "The field {PropertyName} must be between {MinLength} and {MaxLength} characters."
- Pattern only: "The field {PropertyName} format is invalid."
- Combined: "The field {PropertyName} is required and must be between {MinLength} and {MaxLength} characters."

### RequiredRange
- "The field {PropertyName} must be between {MinValue} and {MaxValue}."
- "The field {PropertyName} must be greater than {MinValue}." (exclusive minimum)
- "The field {PropertyName} must be less than {MaxValue}." (exclusive maximum)

### RequiredDate
- "The field {PropertyName} is required."
- "The field {PropertyName} must be a valid date in the past."
- "The field {PropertyName} must be a valid date in the future."
- "The field {PropertyName} must be today or a future date."
- "The field {PropertyName} must be today or a past date."

### RequiredGuid
- "The field {PropertyName} must be a valid GUID."

### RequiredCollection
- "The field {PropertyName} is required."
- "The field {PropertyName} must contain at least {MinCount} items."
- "The field {PropertyName} must contain no more than {MaxCount} items."
- "The field {PropertyName} must contain between {MinCount} and {MaxCount} items."

## Performance Characteristics

- **Efficient Reflection**: Cached property access and attribute metadata
- **Compiled Regex**: Pattern matching uses compiled regex for better performance
- **Memory Efficient**: Minimal allocations during validation
- **Thread Safe**: All validation operations are thread-safe

## Framework Support

- **.NET 6.0**: Full support with nullable reference types
- **.NET 8.0**: Full support with latest performance optimizations
- **Dependencies**: 
  - Syrx.Validation (core validation library)
  - System libraries only

## Best Practices

### 1. Combine Validation Layers

```csharp
public class UserService
{
    public void CreateUser(CreateUserRequest request)
    {
        // 1. Attribute-based validation
        var validationResult = Validator.Validate(request);
        Contract.Throw<ArgumentException>(!validationResult.IsValid, 
            "Validation failed: {0}", string.Join(", ", validationResult.ValidationErrors.Select(e => e.ErrorMessage)));
        
        // 2. Business rule validation
        Contract.Throw<InvalidOperationException>(UserExists(request.Email), 
            "User with email {0} already exists", request.Email);
        
        // Process the validated request
        ProcessCreateUser(request);
    }
}
```

### 2. Use Appropriate Validation Levels

```csharp
// Model-level validation for data integrity
public class User
{
    [RequiredString(MinLength = 1)]
    public string Email { get; set; } = string.Empty;
}

// Method-level validation for business rules
public void SendEmail(User user, string subject, string body)
{
    Contract.Throw<ArgumentNullException>(user == null, nameof(user));
    Contract.Throw<InvalidOperationException>(!user.IsEmailVerified, "User email must be verified");
    
    // Send email
}
```

### 3. Consistent Error Handling

```csharp
public class ValidationHelper
{
    public static void ValidateAndThrow<T>(T model) where T : class
    {
        var result = Validator.Validate(model);
        if (!result.IsValid)
        {
            var message = string.Join("; ", result.ValidationErrors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            throw new ValidationException($"Validation failed for {typeof(T).Name}: {message}");
        }
    }
}
```

## Related Packages

- **[Syrx.Validation](https://www.nuget.org/packages/Syrx.Validation/)**: Core precondition checking library

## Contributing

Contributions are welcome! Please read our [Contributing Guidelines](CONTRIBUTING.md) for details on how to submit pull requests, report issues, and suggest improvements.

## License

This project is licensed under the MIT License - see the [license.txt](../license.txt) file for details.

## Framework Support

- **.NET 8.0**: Full support with nullable reference types and performance optimizations
- **.NET 9.0**: Full support with latest runtime improvements
- **Dependencies**: System.ComponentModel.Annotations, Syrx.Validation (core library)

## Changelog

### Version 3.0.0
- **BREAKING CHANGE**: Removed .NET 6.0 support
- Added .NET 9.0 support
- Now targets .NET 8.0 and .NET 9.0
- Enhanced with new validation attributes and improved performance

### Version 2.0.0
- Added `RequiredStringAttribute` with pattern and length validation
- Added `RequiredRangeAttribute` for numeric validation with inclusive/exclusive boundaries
- Improved error message consistency and standardization
- Enhanced nullable reference type support
- Performance optimizations for regex compilation and caching
- Added comprehensive validation constants

### Version 1.0.0
- Initial release with basic validation attributes
- `RequiredDateAttribute`, `RequiredGuidAttribute`, `RequiredCollectionAttribute`
- Core `Validator` infrastructure