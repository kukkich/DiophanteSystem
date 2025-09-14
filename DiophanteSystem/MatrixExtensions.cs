namespace DiophanteSystem;

public static class MatrixExtensions
{
    public static void AddColumn(this int[,] a, int destinationColumn, int sourceColumn, int coefficient)
    {
        var rows = a.GetLength(0);
        for (var i = 0; i < rows; i++)
        {
            a[i, destinationColumn] += a[i, sourceColumn] * coefficient;
        }
    }
    
    public static void SwapColumns(this int[,] a, int column1, int column2)
    {
        if (column1 == column2)
            return;
        
        var rows = a.GetLength(0);
        for (var i = 0; i < rows; i++)
        {
            (a[i, column1], a[i, column2]) = (a[i, column2], a[i, column1]);
        }
    }

    public static void SwapRows(this int[,] a, int row1, int row2)
    {
        var columns = a.GetLength(1);
        for (var i = 0; i < columns; i++)
        {
            (a[row1, i], a[row2, i]) = (a[row2, i], a[row1, i]);
        }
    }

    public static IEnumerable<T> Row<T>(this T[,] a, int row)
    {
        var columns = a.GetLength(1);
        for (var column = 0; column < columns; column++)
        {
            yield return a[row, column];
        }
    }
}