
namespace DDDPlayGround.Shared.Base
{
    public abstract class AuditableEntity : Entity
    {
        public DateTime CreatedDate { get; protected set; }
        public string? CreatedBy { get; protected set; }
        public DateTime? ModifiedDate { get; protected set; }
        public string? ModifiedBy { get; protected set; }

        public void SetCreated(string user)
        {
            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("User cannot be null or empty", nameof(user));

            CreatedBy = user;
            CreatedDate = DateTime.UtcNow;
        }
        public void SetModified(string user)
        {
            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("User cannot be null or empty", nameof(user));

            ModifiedBy = user;
            ModifiedDate = DateTime.UtcNow;
        }
    }

}
