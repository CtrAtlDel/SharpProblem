namespace lab3;

public static class Padding
{
    public static byte[] Pks5(byte[] data)
    {
        int oldLength = data.Length;

        Array.Resize(ref data, Const.AesMsgSize);

        for (int i = oldLength; i < data.Length; i++)
        {
            data[i] = (byte) (Const.AesMsgSize - oldLength);
        }

        return data;
    }

    public static byte[] None(byte[] data, int length)
    {
        var list = new Span<byte>(data);
        return list.Slice(0, length).ToArray();
    }

    public static byte[] Ten(byte[] data)
    {
        return null;
    }
}