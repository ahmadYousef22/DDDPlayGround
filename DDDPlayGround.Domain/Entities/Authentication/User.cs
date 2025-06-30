using DDDPlayGround.Domain.Base;
using DDDPlayGround.Domain.Entities.Authentication;
using DDDPlayGround.Domain.Enums;
using DDDPlayGround.Domain.Interfaces;
using DDDPlayGround.Domain.ValueObjects;

public class User : AuditableEntity, IAggregateRoot
{
    private readonly List<UserToken> _tokens = new();

    private User() { }

    public User(string username, Email email, UserRole role, PasswordHash passwordHash, FullName fullName) 
    {
        Username = !string.IsNullOrWhiteSpace(username) ? username : throw new ArgumentNullException(nameof(username));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Role = role;
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        IsActive = true;
        FullName = fullName ?? throw new ArgumentNullException( nameof(fullName));
    }

    public string Username { get; private set; }
    public Email Email { get; private set; }
    public FullName? FullName { get; private set; }
    public UserRole Role { get; private set; }
    public PasswordHash PasswordHash { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? LastPasswordChanged { get; private set; }
    public string? PasswordResetToken { get; private set; }
    public DateTime? PasswordResetTokenExpiry { get; private set; }

    public IReadOnlyCollection<UserToken> Tokens => _tokens.AsReadOnly();

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
        _tokens.Add(token);
    }

    public void SetRole(UserRole role)
    {
        Role = role;
    }

    public void ChangeEmail(Email newEmail)
    {
        Email = newEmail;
    }
}
