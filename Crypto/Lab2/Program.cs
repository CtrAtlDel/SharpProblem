namespace lab2
{
    internal class Programm
    {
        public static void Main(string[] args)
        {
            var size = 5;
            var sizeConst = 15;
            var randomArray = ShaXx.RandomByteGenerator(size);
            Algorythm.HappyBirthday(sizeConst, randomArray);
        }
    }
};
