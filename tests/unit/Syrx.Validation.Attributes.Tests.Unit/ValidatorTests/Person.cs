namespace Syrx.Validation.Attributes.Tests.Unit.ValidatorTests
{
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
}
