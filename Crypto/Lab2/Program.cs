namespace lab2
{
    internal class Programm
    {
        public static void Main(string[] args)
        {
            var size = 5;
            var sizeConst = 20;
            var randomArray = ShaXx.RandomByteGenerator(size);
            Algorythm.HappyBirthday(sizeConst, randomArray);
            
            //
            // byte[] key1 = {0, 0, 0, 0};
            // byte[] arr1 = {1, 2, 3, 4};
            //
            // var mapp = new Dictionary<byte[], byte[]>(new FixedComparator());
            // mapp.Add(key1, arr1);
            //
            // byte[] key2 = {0, 0, 0, 0};
            // byte[] arr2 = {1, 2, 3, 4};
            //
            //
            // var bol = mapp.ContainsKey(key2);
            //
            // Console.Out.WriteLine($"Equals: {bol}");
        }
    }
};
