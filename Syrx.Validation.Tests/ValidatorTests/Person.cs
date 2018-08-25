using System;
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
