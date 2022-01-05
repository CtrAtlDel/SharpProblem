using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.FileIO;

namespace MatrixSolve;

public class Parser
{
    public int size;
    public int sizeOfBuffer;
    public int[] buffer;
    public void ReadCsv(string path, string path2)
    {
        using (TextFieldParser parser = new TextFieldParser(path))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            while (!parser.EndOfData)
            {
                //Processing row
                string[] lines = parser.ReadFields();
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