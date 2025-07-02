namespace DDDPlayGround.Domain.Constants
{
    public static class RegularExpression
    {
        public const string PhoneNumber = @"^(?:\+962|0)?7[789]\d{7}$";
        public const string Email = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        public const string StrongPassword = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";
        public const string Numeric = @"^\d+$";
        public const string Alphabetic = @"^[A-Za-z]+$";
        public const string AlphaNumeric = @"^[A-Za-z0-9]+$";
    }
}
