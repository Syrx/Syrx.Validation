# Syrx.Validation
A tiny little library with a very simple precondition checker and a handful of validation attributes. 


## What

The precondition checker the `Contract` class is probably the most useful as it can be used by any method. The `Validator` is pretty useful too, but really only useful for POCOs that are decorated with validation attributes. 

## Why

Checking conditions in your code is a good thing **but** it can get really verbose really quickly and ruin the readbility/intent of your code. More often than not, precondition checks take the form of multiline `if (condition) throw exception` statements. We wrote this library to convert the multiline statements into single line statements and give you control over which exceptions are thrown when the condition fails. 

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

Using the `Contract` precondtion checker we can collapse those conditions into much simpler statements - _require_ the condition to be true, otherwise throw an exception. 


```cs
using static Syrx.Validation.Contract;

public class Person
{
    public string Name { get; }
    public DateTime DateOfBirth { get; }

    public Person(string name, DateTime dateOfBirth)
    {
        Require<ArgumentNullException>(!string.IsNullOrWhiteSpace(name), nameof(name));
        Require<ArgumentOutOfRangeException>(name.Length <= 50, nameof(name));
        Require<ArgumentOutOfRangeException>(!(dateOfBirth == DateTime.MinValue ||
                                               dateOfBirth == DateTime.MaxValue ||
                                               dateOfBirth > DateTime.Now), nameof(dateOfBirth));

        Name = name;
        DateOfBirth = dateOfBirth;
    }
}

```

### Now with added delegates!

Useful as the `Require<T>` method is, it can be a little limiting when you need very fine control over the exception. Never fear though - the 1.1.0 version now allows you to pass your own `Func<TException>` delegate to be invoked if your required condition evaluates to false. 

Using our super trivial example again, our validation can be re-written again. It's functionally equivalent, and a couple more keystrokes, but shows how you can roll your own exception invocation. 


```cs
using static Syrx.Validation.Contract;

public class Person
{
    public string Name { get; }
    public DateTime DateOfBirth { get; }

    public Person(string name, DateTime dateOfBirth)
    {
        Require(!string.IsNullOrWhiteSpace(name), () => new ArgumentNullException(nameof(name)));
        Require(name.Length <= 50, () => new ArgumentOutOfRangeException(nameof(name)));
        Require(!(dateOfBirth == DateTime.MinValue || 
                  dateOfBirth == DateTime.MaxValue ||
                  dateOfBirth > DateTime.Now), () => new ArgumentOutOfRangeException(nameof(dateOfBirth)));
        
        Name = name;
        DateOfBirth = dateOfBirth;
    }
}

```

### That's great! Where can I get this?

[Nuget](https://www.nuget.org/packages/Syrx.Validation/), of course. 

```Install-Package Syrx.Validation```


#### Anything else I should know? 

First, I find that the `Contract.Require<T>` method is insanely useful. Your mileage may vary. You might like using this library. Then again, you might not. I hope you do. 

Second, this is probably the most documentation that you're going to get on this. It's a really simple library and you're smart. 

Thanks! 
