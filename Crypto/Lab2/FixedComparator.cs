namespace lab2;

public class FixedComparator : IEqualityComparer<byte[]>
{
    public bool Equals(byte[]? left, byte[]? right)
    {
        if (left == null || right == null)
        {
            return left == right;
        }

        return left.SequenceEqual(right);
    }

    public int GetHashCode(byte[] key)
    {
        if (key == null)
            throw new ArgumentNullException("key");
        
        return key.Sum(b => b);
    }
}