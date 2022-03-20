namespace lab2;

public class Algorythm
{
    public static void HappyBirthday(int size, byte[] startArray)
    {
        if (size < Const.MinXx || size > Const.MaxXx)
            throw new Exception("Bad size for message");

        var map = new Dictionary<byte[], byte[]>(new FixedComparator());
        var shaCut = new ShaXx(size);
        map.Add(shaCut.GetHash(startArray), startArray);
        
        

        while (true)
        {
            var x = ShaXx.RandomByteGenerator(shaCut.GetHash(startArray).Length);
            var hashX = shaCut.GetHash(x);
            
            if (map.ContainsKey(hashX))
            {
                Console.Out.WriteLine("Collision hash: " + hashX + " First elem: " + x + " Second elem: " + map[hashX]);
                map.TryAdd(hashX, x);
                Console.Out.WriteLine("Size: " + map.Count);
            }
            else
            {
                map.TryAdd(hashX, x);
                Console.Out.WriteLine("Size: " + map.Count);
            }
        }
    }
}