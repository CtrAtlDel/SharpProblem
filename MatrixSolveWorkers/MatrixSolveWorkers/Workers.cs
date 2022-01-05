namespace MatrixSolve;

using System.Threading.Tasks;

public class Workers
{
    public static List<int> Multiplication(List<int> line, List<int> column) //reduce
    {
        List<int> result = new List<int>();
        for (int i = 0; i < line.Count; i++)
            result.Add(0);
        Parallel.ForEach(Enumerable.Range(0, line.Count), k => { result[k] = line[k] * column[k]; });
        return result;
    }

    public static int Sum(List<int> list)
    {
        return list.Sum();
    }
}