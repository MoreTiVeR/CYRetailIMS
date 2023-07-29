using System.Text;

namespace CYRetailIMS.Domain.Common;

public abstract class BaseValueObject
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    protected static bool EqualOperator(BaseValueObject? left, BaseValueObject? right)
    {
        if (ReferenceEquals(left, objB: null) || ReferenceEquals(right, objB: null))
        {
            return false;
        }
        return ReferenceEquals(left, objB: null) || left.Equals(right);
    }

    protected static bool NotEqualOperator(BaseValueObject left, BaseValueObject right) => !EqualOperator(left, right);
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType()) return false;
        var other = (BaseValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode() => GetEqualityComponents().Select(x => x != null ? x.GetHashCode() : 0).Aggregate((x, y) => x ^ y);
}
