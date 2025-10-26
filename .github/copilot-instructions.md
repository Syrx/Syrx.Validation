# GitHub Copilot Instructions for Syrx.Validation

## Project Overview

**Syrx.Validation** is a high-performance, lightweight .NET validation library consisting of two complementary packages:

- **Syrx.Validation**: Core precondition checking with fluent API for method parameter validation
- **Syrx.Validation.Attributes**: Comprehensive attribute-based model validation framework

### Key Characteristics
- **Performance-First**: Zero-overhead validation with minimal allocations
- **Modern .NET**: Targets .NET 8.0 and .NET 9.0 with nullable reference types
- **Static Analysis Support**: Enhanced with `[DoesNotReturnIf]` attributes for compiler optimization
- **Thread-Safe**: All operations are thread-safe with no shared mutable state
- **Extensible**: Easy to extend with custom validation attributes and exception factories

## Architecture

### Core Components

#### 1. Contract Class (`Syrx.Validation`)
- **Purpose**: Fluent precondition checking with immediate exception throwing
- **Design Pattern**: Static factory methods for clean API without instantiation
- **Key Features**:
  - Multiple overloads for flexibility (message formatting, inner exceptions, factory delegates)
  - `Require` aliases for semantic readability
  - `[DoesNotReturnIf(false)]` attributes for static analysis

#### 2. Validation Attributes (`Syrx.Validation.Attributes`)
- **Architecture**: Attribute-based validation following .NET patterns
- **Validator Class**: Central orchestrator using reflection-based property scanning
- **Performance Optimizations**: Compiled regex patterns, cached reflection metadata

#### 3. Validation Attributes Hierarchy
```
ValidationAttribute (base)
├── RequiredStringAttribute     # String validation with length/pattern constraints
├── RequiredRangeAttribute      # Numeric range validation for all numeric types
├── RequiredDateAttribute       # DateTime validation with temporal constraints
├── RequiredGuidAttribute       # GUID validation (not Guid.Empty)
└── RequiredCollectionAttribute # Collection validation with count constraints
```

## Code Style & Patterns

### Naming Conventions
- **Classes**: PascalCase with descriptive names
- **Methods**: PascalCase following .NET conventions
- **Properties**: PascalCase with clear intent
- **Fields**: Private fields with underscore prefix (`_regex`)
- **Constants**: PascalCase in dedicated constants classes
- **Files**: Avoid ALL CAPS names (prefer lowercase like `license.txt`)

### Error Handling
- **Consistent Messaging**: Use `ErrorMessages` static class for standardized error templates
- **Validation Patterns**: Always validate inputs early with `Contract.Throw<>`
- **Exception Types**: Use appropriate .NET exception types (`ArgumentNullException`, `ArgumentOutOfRangeException`, etc.)

### Performance Considerations
- **Regex Compilation**: Use `RegexOptions.Compiled` for pattern validation
- **Early Returns**: Validate simple conditions first before expensive operations
- **Memory Efficiency**: Minimize allocations in hot paths
- **Caching**: Cache expensive operations like reflection metadata

## Development Guidelines

### Adding New Validation Attributes

1. **Inherit from ValidationAttribute**:
```csharp
public sealed class NewValidationAttribute : ValidationAttribute
{
    // Constructor validation using Contract.Throw<>
    public NewValidationAttribute(parameters)
    {
        Throw<ArgumentException>(validCondition, "Clear error message");
    }
    
    public override bool IsValid(object? value)
    {
        // Handle null values explicitly
        if (value == null)
        {
            ErrorMessage = ErrorMessages.ValueCannotBeNull;
            return false;
        }
        
        // Type checking
        if (value is not ExpectedType typedValue)
        {
            ErrorMessage = string.Format(ErrorMessages.ValueMustBeOfType, "ExpectedType", value.GetType().Name);
            return false;
        }
        
        // Validation logic with specific error messages
        return true;
    }
}
```

2. **Create comprehensive unit tests** in corresponding test project
3. **Add to documentation** with usage examples and patterns
4. **Update ErrorMessages** class with new message templates

### Testing Standards

- **xUnit Framework**: Use for all unit tests
- **Theory-based Testing**: Use `[Theory]` with `[MemberData]` for parameterized tests
- **Test Organization**: Group tests by feature in dedicated folders
- **Coverage**: Aim for comprehensive test coverage including edge cases
- **Assert Patterns**: Use static imports for cleaner assertions (`using static Xunit.Assert`)

### Documentation Requirements

- **XML Documentation**: All public APIs must have comprehensive XML docs
- **README Files**: Each package has its own comprehensive README
- **Architecture Documentation**: Maintain technical architecture documentation
- **Examples**: Include practical usage examples in all documentation

## Project Structure Standards

### Solution Organization
```
Syrx.Validation/
├── .docs/                           # Hidden documentation folder
│   ├── architecture.md             # Technical architecture guide
│   ├── Syrx.Validation.README.md   # Detailed core library guide
│   └── Syrx.Validation.Attributes.README.md # Detailed attributes guide
├── .github/
│   └── workflows/                   # CI/CD workflows
├── src/
│   ├── Syrx.Validation/            # Core validation library
│   └── Syrx.Validation.Attributes/ # Attribute validation framework
├── tests/
│   └── unit/
│       ├── Syrx.Validation.Tests.Unit/
│       └── Syrx.Validation.Attributes.Tests.Unit/
├── license.txt                     # MIT license (lowercase)
└── README.md                       # Project overview with quick start
```

### Project File Standards

- **Multi-targeting**: Target both .NET 8.0 and .NET 9.0
- **NuGet Configuration**: Include `GeneratePackageOnBuild=true`, README files, and proper metadata
- **Global Using Files**: Each project has `Usings.cs` for common imports
- **Nullable Reference Types**: Enable with `#nullable enable`

## Coding Best Practices

### Contract Usage Patterns

**Preferred Pattern**:
```csharp
using static Syrx.Validation.Contract;

public void ProcessData(string input, int count)
{
    // Use descriptive error messages
    Throw<ArgumentNullException>(string.IsNullOrEmpty(input), "Input cannot be null or empty");
    Throw<ArgumentOutOfRangeException>(count < 0, "Count must be non-negative, got {0}", count);
    
    // Process validated data
}
```

**Exception Factory Pattern**:
```csharp
Throw<CustomException>(violatesBusinessRule, () => 
    new CustomException("BR001", "Business rule violated", additionalContext));
```

### Validation Attribute Patterns

**String Validation**:
```csharp
[RequiredString(MinLength = 2, MaxLength = 50)]                    // Length constraints
[RequiredString(Pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")] // Pattern validation
[RequiredString(MinLength = 5, MaxLength = 20, Pattern = @"^[A-Za-z0-9]+$")]     // Combined validation
```

**Numeric Range Validation**:
```csharp
[RequiredRange(MinValue = 0, MaxValue = 100)]                      // Inclusive range
[RequiredRange(MinValue = 0, IsMinInclusive = false)]              // Exclusive minimum
[RequiredRange(MinValue = 0, MaxValue = 1, IsMinInclusive = false, IsMaxInclusive = false)] // Exclusive bounds
```

## Integration Patterns

### Service Layer Integration
```csharp
public class UserService
{
    public void CreateUser(CreateUserRequest request)
    {
        // 1. Attribute-based validation
        var validationResult = Validator.Validate(request);
        Contract.Throw<ArgumentException>(!validationResult.IsValid, 
            "Validation failed: {0}", string.Join(", ", validationResult.ValidationErrors.Select(e => e.ErrorMessage)));
        
        // 2. Business rule validation using Contract
        Contract.Throw<InvalidOperationException>(UserExists(request.Email), 
            "User with email {0} already exists", request.Email);
        
        // Process validated request
    }
}
```

### ASP.NET Core Integration
```csharp
[ApiController]
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
        
        // Process validated request
    }
}
```

## Common Pitfalls to Avoid

1. **Performance Issues**:
   - Don't create new Regex objects in validation loops
   - Avoid reflection in hot paths without caching
   - Don't allocate unnecessary objects in validation methods

2. **Error Message Consistency**:
   - Always use ErrorMessages constants, not hardcoded strings
   - Include context in error messages (actual vs expected values)
   - Use appropriate exception types for different validation scenarios

3. **Threading Issues**:
   - Don't share mutable state between validation operations
   - Ensure attribute instances are immutable after construction
   - Use thread-safe operations for any shared resources

4. **API Design**:
   - Maintain backward compatibility in public APIs
   - Use semantic versioning for breaking changes
   - Follow established .NET naming and design conventions

## Build and Testing

### CI/CD Pipeline
- **GitHub Actions**: Automated build, test, and publish workflows
- **Multi-framework Testing**: Tests run on both .NET 8.0 and .NET 9.0
- **NuGet Publishing**: Automated package publishing on releases

### Local Development
```bash
# Build solution
dotnet build

# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Pack NuGet packages
dotnet pack --configuration Release
```

## Version Management

### Current Version: 3.0.0
- **Breaking Changes**: Document in release notes and README changelogs
- **Semantic Versioning**: Follow strict semver for NuGet package versioning
- **Framework Support**: Currently .NET 8.0 and .NET 9.0

### Release Process
1. Update version numbers in project files
2. Update CHANGELOG sections in README files
3. Ensure all tests pass on target frameworks
4. Create GitHub release with appropriate tag
5. NuGet packages are automatically published via GitHub Actions

## Extension Points

The library is designed for extensibility:

1. **Custom Validation Attributes**: Inherit from ValidationAttribute
2. **Custom Exception Types**: Use with Contract.Throw<> factory patterns
3. **Custom Validators**: Extend validation logic in service layers
4. **Integration Patterns**: ASP.NET Core, dependency injection, etc.

## Support and Maintenance

- **Documentation**: Maintain comprehensive documentation in `.docs/` folder
- **Examples**: Include practical examples in all documentation
- **Testing**: Maintain high test coverage with meaningful test cases
- **Performance**: Regular performance testing and optimization
- **Community**: Accept contributions following established patterns and standards

This instruction set ensures consistent development practices, high code quality, and maintainable architecture for the Syrx.Validation library.