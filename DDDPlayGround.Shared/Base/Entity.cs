
namespace DDDPlayGround.Shared.Base
{
    public abstract class Entity : Entity<Guid>
    {
    }

    public abstract class Entity<TPrimaryKey>
    {
        public TPrimaryKey? Id { get; protected set; }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity<TPrimaryKey> other)
                return false;

            if (EqualityComparer<TPrimaryKey>.Default.Equals(Id, default))
                return false;

            return EqualityComparer<TPrimaryKey>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return Id?.GetHashCode() ?? base.GetHashCode();
        }

        public static bool operator ==(Entity<TPrimaryKey>? left, Entity<TPrimaryKey>? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<TPrimaryKey>? left, Entity<TPrimaryKey>? right)
        {
            return !Equals(left, right);
        }
    }
}
