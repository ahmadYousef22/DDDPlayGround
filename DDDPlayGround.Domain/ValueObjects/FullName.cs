
namespace DDDPlayGround.Domain.ValueObjects
{
    public class FullName
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private FullName() { } // Required for EF
        public FullName(string firstName, string lastname)
        {
            FirstName = firstName;
            LastName = lastname;
        }
        public override string ToString() => $"{FirstName} {LastName}";
    }
}
