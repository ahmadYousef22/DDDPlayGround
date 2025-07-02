namespace DDDPlayGround.Domain.ValueObjects
{
    public record PasswordHash
    {
        public string Value { get; }
        private PasswordHash() { }
        public PasswordHash(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Password hash cannot be empty.", nameof(value));

            if (value.Length < 60) 
                throw new ArgumentException("Password hash is too short.", nameof(value));

            Value = value;
        }

        public override string ToString() => Value;
    }
}
