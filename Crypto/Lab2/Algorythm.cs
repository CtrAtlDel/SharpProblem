using System.Net.NetworkInformation;
using System.Reflection;

namespace lab2;

public class Algorythm
{
    public static void HappyBirthday(int size, byte[] startArray)
    {
        if (size < Const.MinXx || size > Const.MaxXx)
            throw new Exception("Bad size for message");

        var map = new Dictionary<byte[], byte[]>(new FixedComparator());

        var intMap = new Dictionary<int, int>();


        var shaCut = new ShaXx(size);
        map.Add(shaCut.GetHash(startArray), startArray);


        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                var x = ShaXx.RandomByteGenerator(size + i);
                var hashX = shaCut.GetHash(x);

                if (map.ContainsKey(hashX))
                {
                    Console.Out.WriteLine("Collision hash: " + hashX + " First elem: " + x + " Second elem: " +
                                          map[hashX]);
                }
                else
                {
                    map.Add(hashX, x);
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

        int i = BitConverter.ToInt32(bytes, 0);

        return i;
    }
}