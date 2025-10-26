# Syrx.Validation

A lightweight and efficient .NET validation library providing both precondition checking and attribute-based model validation. 


## Overview

Syrx.Validation consists of two main packages:

- **Syrx.Validation**: Core validation library with precondition checking
- **Syrx.Validation.Attributes**: Attribute-based validation framework

## Features

- ✅ **Precondition Checking**: Validate method parameters with fluent syntax
- ✅ **Attribute-Based Validation**: Declarative model validation using attributes
- ✅ **High Performance**: Optimized for minimal overhead
- ✅ **Modern .NET**: Supports .NET 8.0 and .NET 9.0 with nullable reference types
- ✅ **Comprehensive**: Covers common validation scenarios out of the box
- ✅ **Extensible**: Easy to extend with custom validation attributes

## Quick Start

### Installation

Install via NuGet Package Manager:

```powershell
Install-Package Syrx.Validation
Install-Package Syrx.Validation.Attributes
```

Or via .NET CLI:

```bash
dotnet add package Syrx.Validation
dotnet add package Syrx.Validation.Attributes
```

### Basic Usage

#### Precondition Checking

```csharp
using static Syrx.Validation.Contract;

public void ProcessData(string input, int count)
{
    // Throw ArgumentNullException if input is null or empty
    Throw<ArgumentNullException>(string.IsNullOrEmpty(input), "Input cannot be null or empty");
    
    // Throw ArgumentOutOfRangeException if count is negative
    Throw<ArgumentOutOfRangeException>(count < 0, "Count must be non-negative");
    
    // Process the validated data
    Console.WriteLine($"Processing {input} with count {count}");
}
```

#### Attribute-Based Validation

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
var user = new User { /* ... */ };
var results = Validator.Validate(user);

if (!results.IsValid)
{
    foreach (var error in results.ValidationErrors)
    {
        Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
    }
}
```

## API Reference

### Syrx.Validation

#### Contract Class

The `Contract` class provides static methods for precondition checking:

```csharp
// Throw exception if condition is false
Throw<TException>(bool condition, string message);
Throw<TException>(bool condition, string message, params object[] args);
Throw<TException>(bool condition, string message, Exception innerException);
Throw<TException>(bool condition, string message, Exception innerException, params object[] args);
Throw<TException>(bool condition, Func<TException> exceptionFactory);

// Aliases for Throw methods (same signatures)
Require<TException>(...);
```

**Examples:**

```csharp
// Basic usage
Throw<ArgumentNullException>(value == null, "Value cannot be null");

// With string formatting
Throw<ArgumentException>(value < 0, "Value {0} must be non-negative", value);

// With inner exception
Throw<InvalidOperationException>(failed, "Operation failed", innerException);

// With exception factory
Throw<CustomException>(invalid, () => new CustomException("Custom message", data));

// Using Require alias
Require<ArgumentNullException>(value != null, "Value is required");
```

### Syrx.Validation.Attributes

#### Validation Attributes

##### RequiredStringAttribute

Validates string properties with length and pattern constraints:

```csharp
[RequiredString] // Not null or empty
[RequiredString(MinLength = 5)]
[RequiredString(MaxLength = 100)]
[RequiredString(MinLength = 5, MaxLength = 100)]
[RequiredString(Pattern = @"^\d+$")] // Numbers only
[RequiredString(MinLength = 5, Pattern = @"^[A-Z].*")] // Combined validation
```

##### RequiredRangeAttribute

Validates numeric properties within specified ranges:

```csharp
[RequiredRange(MinValue = 0)] // Non-negative
[RequiredRange(MaxValue = 100)] // Maximum value
[RequiredRange(MinValue = 1, MaxValue = 10)] // Range
[RequiredRange(MinValue = 0, IsMinInclusive = false)] // Exclusive minimum (> 0)
[RequiredRange(MaxValue = 100, IsMaxInclusive = false)] // Exclusive maximum (< 100)
```

Supports all numeric types: `int`, `long`, `double`, `decimal`, `float`, `short`, `byte`, etc.

##### RequiredDateAttribute

Validates DateTime properties with various options:

```csharp
[RequiredDate] // Not default DateTime
[RequiredDate(RequiredDateOptions.PastOnly)] // Must be in the past
[RequiredDate(RequiredDateOptions.FutureOnly)] // Must be in the future
[RequiredDate(RequiredDateOptions.TodayOrFuture)] // Today or future
[RequiredDate(RequiredDateOptions.TodayOrPast)] // Today or past
```

##### RequiredGuidAttribute

Validates Guid properties:

```csharp
[RequiredGuid] // Not Guid.Empty
```

##### RequiredCollectionAttribute

Validates collection properties:

```csharp
[RequiredCollection] // Not null or empty
[RequiredCollection(MinCount = 1)] // At least one item
[RequiredCollection(MaxCount = 10)] // Maximum items
[RequiredCollection(MinCount = 1, MaxCount = 5)] // Range of items
```

#### Validator Class

Static validation methods:

```csharp
// Validate a single object
ValidationResult result = Validator.Validate(object instance);

// Check validation result
if (result.IsValid)
{
    // Validation passed
}
else
{
    foreach (var error in result.ValidationErrors)
    {
        Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
    }
}
```

#### ValidationResult Class

Contains validation results:

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

## Example

In one of the finest traditions of our craft, let's take a look at a contrived and trivial example. 

Let's assume we have a basic `Person` class which we'll be saving to a database. Seems pretty straightforward, right? Here's what the `Person` looks like without any validation. 

### No validation
```cs
public class Person
{
    public string Name { get; }
    public DateTime DateOfBirth { get; }

    public Person(string name, DateTime dateOfBirth)
    {
        Name = name;
        DateOfBirth = dateOfBirth;
    }
}
```

## What could go wrong? 

_Hold my beer..._

We said earlier we'd be saving the `Person` to a database. Here - in no particular order - are a couple of things that _could_ go wrong. 

* No name supplied. Oops. 
* Name is supplied to the class. It's 51 characters long but the field on the table can only hold 50. Oops. 
* DateOfBirth is equal to either DateTime.MinValue, DateTime.MaxValue or is sometime in the future. Oops.

### Vanilla Validation

Okay, so we want to catch those pesky validation problems while we instantiate our `Person` (yeah, there's a rude joke in there somewhere). Using the traditional `if (condition) throw exception` approach, our class might now look like this which - in my humble opinion - ain't too cool. 

While it does the job of catching validation problems, it's caused our simple class to look quite a bit less simple.
The premise behind the if statements is `if (false) throw exception`.

```cs
public class Person
{
    public string Name { get; }
    public DateTime DateOfBirth { get; }

    public Person(string name, DateTime dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (name.Length >= 50)
        {
            throw new ArgumentOutOfRangeException(nameof(name));
        }

        if (dateOfBirth == DateTime.MinValue || 
            dateOfBirth == DateTime.MaxValue || 
            dateOfBirth > DateTime.Now)
        {
            throw new ArgumentOutOfRangeException(nameof(dateOfBirth));
        }

        Name = name;
        DateOfBirth = dateOfBirth;
    }
}
```

### Better, I think.

Using the `Contract` precondtion checker we can collapse those conditions into much simpler statements - _require_ the condition to be true, otherwise _throw_ an exception. 


```cs
using static Syrx.Validation.Contract;

public class Person
{
    public string Name { get; }
    public DateTime DateOfBirth { get; }

    public Person(string name, DateTime dateOfBirth)
    {
        Throw<ArgumentNullException>(!string.IsNullOrWhiteSpace(name), nameof(name));
        Throw<ArgumentOutOfRangeException>(name.Length <= 50, nameof(name));
        Throw<ArgumentOutOfRangeException>(!(dateOfBirth == DateTime.MinValue ||
                                               dateOfBirth == DateTime.MaxValue ||
                                               dateOfBirth > DateTime.Now), nameof(dateOfBirth));

        Name = name;
        DateOfBirth = dateOfBirth;
    }
}

```

### Now with added delegates!

Useful as the `Throw<T>` method is, it can be a little limiting when you need very fine control over the exception. Never fear though - the 1.1.0 version now allows you to pass your own `Func<TException>` delegate to be invoked if your required condition evaluates to false. 

Using our super trivial example again, our validation can be re-written again. It's functionally equivalent, and a couple more keystrokes, but shows how you can roll your own exception invocation. 


```cs
using static Syrx.Validation.Contract;

public class Person
{
    public string Name { get; }
    public DateTime DateOfBirth { get; }

    public Person(string name, DateTime dateOfBirth)
    {
        Throw(!string.IsNullOrWhiteSpace(name), () => new ArgumentNullException(nameof(name)));
        Throw(name.Length <= 50, () => new ArgumentOutOfRangeException(nameof(name)));
        Throw(!(dateOfBirth == DateTime.MinValue || 
                  dateOfBirth == DateTime.MaxValue ||
                  dateOfBirth > DateTime.Now), () => new ArgumentOutOfRangeException(nameof(dateOfBirth)));
        
        Name = name;
        DateOfBirth = dateOfBirth;
    }
}

```

## Framework Support

- **.NET 8.0**: Full support with nullable reference types and performance optimizations
- **.NET 9.0**: Full support with latest runtime improvements

## Documentation

- **[Architecture Guide](.docs/architecture.md)**: Detailed technical architecture and design patterns
- **[Syrx.Validation Guide](.docs/Syrx.Validation.README.md)**: Comprehensive guide for the core validation library
- **[Syrx.Validation.Attributes Guide](.docs/Syrx.Validation.Attributes.README.md)**: Complete reference for attribute-based validation

## Installation

### NuGet Package Manager

```powershell
Install-Package Syrx.Validation
Install-Package Syrx.Validation.Attributes
```

### .NET CLI

```bash
dotnet add package Syrx.Validation
dotnet add package Syrx.Validation.Attributes
```

## Contributing

Contributions are welcome! Please feel free to submit issues, feature requests, or pull requests.

## License

This project is licensed under the MIT License - see the [license.txt](license.txt) file for details.

## Changelog

### Version 3.0.0
- **BREAKING CHANGE**: Removed .NET 6.0 support
- Added .NET 9.0 support
- Now targets .NET 8.0 and .NET 9.0
- Enhanced attribute validation with new attributes: RequiredRangeAttribute and RequiredStringAttribute

### Version 2.0.0
- Added comprehensive attribute-based validation framework
- Enhanced precondition checking with exception factories
- Improved nullable reference type support

### Version 1.0.0
- Initial release with core precondition checking functionality 
