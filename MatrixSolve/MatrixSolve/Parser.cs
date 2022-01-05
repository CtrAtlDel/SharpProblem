using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.FileIO;

namespace MatrixSolve;

public class Parser
{
    public int size;
    public int sizeOfBuffer;
    public void ReadCsv(string path, string path2)
    {
        path = path2;
        using (TextFieldParser parser = new TextFieldParser(path))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            while (!parser.EndOfData)
            {
                //Processing row
                var testLine = parser.ReadLine();
                string[] lines = parser.ReadFields();
                // var testLine = parser.ReadLine();
                Console.WriteLine($"{testLine}");
                // parser.Re
                size = lines.Length; //количество элементов матрицы
                sizeOfBuffer = GetBufferSize(size);
                foreach (string field in lines)
                {
                    Console.WriteLine($"String: {field}");
                }
            }
        }
    }
    
    /// <summary>
    /// Return int buffer with int
    /// </summary>
    /// <param name="stringBuffer"></param>
    /// <returns></returns>
    public List<int> CreateBuffer(string stringBuffer)
    {
        List<int> buffer = new List<int>();
        var arrayString = stringBuffer.Split(",");
        for (int i = 0; i < arrayString.Length; i++)
        {
            buffer.Add(Int32.Parse(arrayString[i]));
        }
        return buffer;
    }

    public int GetBufferSize(int sizeMatrix)
    {
        int pow = 0;
        while (sizeMatrix > 0)
        {
            sizeMatrix <<= 1;
            pow++;
        }
        pow--;
        return pow;
    }
    
}