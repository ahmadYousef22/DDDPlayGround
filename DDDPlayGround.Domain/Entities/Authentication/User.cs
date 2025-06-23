using DDDPlayGround.Domain.ValueObjects;
using DDDPlayGround.Shared.Base;
using DDDPlayGround.Shared.Enums;

namespace DDDPlayGround.Domain.Entities.Authentication
{
    public class User : AuditableEntity
    {
        private User() { }
        public User(string username, Email? email, UserRole role, PasswordHash passwordHash)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Email = email;
            Role = role;
            PasswordHash = passwordHash;
            IsActive = true;
        }

        public string Username { get; private set; } = default!;
        public Email? Email { get; private set; }
        public UserRole Role { get; private set; }
        public PasswordHash PasswordHash { get; private set; } = default!;
        public bool IsActive { get; private set; } = true;
        public DateTime? LastPasswordChanged { get; private set; }
        public string? PasswordResetToken { get; private set; }
        public DateTime? PasswordResetTokenExpiry { get; private set; }
        public virtual ICollection<UserToken> Tokens { get; private set; } = new List<UserToken>();

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
        public void SetPassword(PasswordHash hashedPassword)
        {
            PasswordHash = hashedPassword;
            LastPasswordChanged = DateTime.UtcNow;
        }
        public void SetPasswordResetToken(string token, DateTime expiry)
        {
            PasswordResetToken = token;
            PasswordResetTokenExpiry = expiry;
        }
        public void ClearPasswordResetToken()
        {
            PasswordResetToken = null;
            PasswordResetTokenExpiry = null;
        }
        public void AddToken(UserToken token)
        {
            Tokens.Add(token);
        }
        public void SetRole(UserRole role)
        {
            Role = role;
        }
    }
}
