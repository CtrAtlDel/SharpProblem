using System;
using Microsoft.VisualBasic.FileIO;

namespace task4
{
    public class Parser
    {
        public int size;

        public void ParseCsv(string PATH)
        {
            using (TextFieldParser parser = new TextFieldParser(@"c:\temp\test.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData) 
                {
                    //Processing row
                    string[] fields = parser.ReadFields();
                    foreach (string field in fields) 
                    {
                        Console.WriteLine($"String: {fields}");
                    }
                }
            }
        }
    }
}