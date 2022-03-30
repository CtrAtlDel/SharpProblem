using System.Drawing;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;

namespace lab2;

public static class Algorythm
{
    public static void HappyBirthday(int size, byte[] startArray)
    {
        if (size < Const.MinXx || size > Const.MaxXx)
            throw new Exception("Bad size for message");

        var dictionary = new Dictionary<byte[], byte[]>(new FixedComparator());

        var shaCut = new ShaXx(size);
        dictionary.Add(shaCut.GetHash(startArray), startArray);


        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                var x = shaCut.RandomByteGenerator(size + i);
                var hashX = shaCut.GetHash(x);

                if (!dictionary.ContainsKey(hashX))
                {
                    dictionary.TryAdd(hashX, x);
                }
                else
                {
                    Console.Out.WriteLine("Collision hash: " + ByteToString(hashX) + " First elem: " + ByteToString(x) +
                                          " Second elem: " +
                                          ByteToString(dictionary[hashX]));
                }
            }
        }
    }
    
    public static String ByteToString(byte[] array)
    {
        return BitConverter.ToString(array);
    }

    public static int getNumber(byte[] bytes)
    {
        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);

        return BitConverter.ToInt32(bytes, 0);
    }
}