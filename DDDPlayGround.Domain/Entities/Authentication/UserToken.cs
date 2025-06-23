using DDDPlayGround.Shared.Base;

namespace DDDPlayGround.Domain.Entities.Authentication
{
    public class UserToken : AuditableEntity
    {
        public Guid UserId { get; private set; }
        public string Token { get; private set; } = default!;
        public DateTime Expires { get; private set; }
        public bool IsRevoked { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string? ReplacedByToken { get; private set; }
        public string? CreatedByIp { get; private set; }
        public string? RevokedByIp { get; private set; }

        public virtual User User { get; private set; } = default!;

        private UserToken() { }

        public UserToken(Guid userId, string token, DateTime expires, string? createdByIp)
        {
            UserId = userId;
            Token = token;
            Expires = expires;
            CreatedByIp = createdByIp;
            IsRevoked = false;
        }

        public void Revoke(string? revokedByIp, string? replacedByToken)
        {
            IsRevoked = true;
            RevokedAt = DateTime.UtcNow;
            RevokedByIp = revokedByIp;
            ReplacedByToken = replacedByToken;
        }

        public bool IsExpired => DateTime.UtcNow >= Expires;
    }
}
