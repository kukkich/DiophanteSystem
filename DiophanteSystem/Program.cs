var input = Console.ReadLine().Split(' ');
var equationsCount = int.Parse(input[0]);
var variablesCount = int.Parse(input[1]);

var coefficients = new long[equationsCount, variablesCount + 1];

for (var i = 0; i < equationsCount; i++)
{
    var equationCoefficients = Console.ReadLine().Split(' ');
    for (var j = 0; j <= variablesCount; j++)
    {
        coefficients[i, j] = long.Parse(equationCoefficients[j]);
    }
}

var matrix = new long[equationsCount + variablesCount, variablesCount + 1];

for (var i = 0; i < equationsCount; i++)
{
    for (var j = 0; j < variablesCount + 1; j++)
    {
        matrix[i, j] = coefficients[i, j];
    }
}

for (var i = 0; i < variablesCount; i++)
{
    matrix[i + equationsCount, i] = 1;
}

var solution = DiophanteSystemSolver.Solve(equationsCount, matrix, variablesCount);

if (solution.IsEmpty)
{
    Console.WriteLine("NO SOLUTIONS");
}
else
{
    Console.WriteLine(solution.FreeVariablesCount);
    for (var solutionComponentNumber = 0; solutionComponentNumber < variablesCount; solutionComponentNumber++)
    {
        for (var freeVariableNumber = 0; freeVariableNumber < solution.FreeVariablesCount; freeVariableNumber++)
        {
            Console.Write(solution.FreeVariables[freeVariableNumber, solutionComponentNumber]);
            Console.Write(' ');
        }
        
        Console.WriteLine(solution.Particular[solutionComponentNumber]);
    }
}

public readonly record struct Solution(long[] Particular, long[,] FreeVariables)
{
    public static Solution Empty => new([], new long[0,0]);
    public int FreeVariablesCount => FreeVariables.GetLength(0);
    public bool IsEmpty => Particular.Length == 0;
}

public static class MatrixExtensions
{
    public static void AddColumn(this long[,] a, int destinationColumn, int sourceColumn, long coefficient)
    {
        var rows = a.GetLength(0);
        for (var i = 0; i < rows; i++)
        {
            a[i, destinationColumn] += a[i, sourceColumn] * coefficient;
        }
    }
    
    public static void SwapColumns(this long[,] a, int column1, int column2)
    {
        if (column1 == column2)
            return;
        
        var rows = a.GetLength(0);
        for (var i = 0; i < rows; i++)
        {
            (a[i, column1], a[i, column2]) = (a[i, column2], a[i, column1]);
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

public static class MathInt
{
    public static long GreatestCommonDivisor(long a, long b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        while (b != 0)
        {
            var t = b;
            b = a % b;
            a = t;
        }
        return a; 
    }
    
    /// <summary>
    /// a = p*b + r, q in [0, |b|).
    /// </summary>
    public static (long Quotient, long Remainder) Divide(long a, long b)
    {
        if (b == 0) throw new DivideByZeroException();

        var q = a / b;
        var r = a % b;

        if (r >= 0) 
            return (q, r);
        if (b > 0)
        {
            q -= 1;
            r += b;
        }
        else
        {
            q += 1;
            r -= b;
        }

        return (q, r);
    }
}

public static class DiophanteSystemSolver
{
    public static Solution Solve(int equationsCount, long[,] matrix, int variablesCount)
    {
        var pivot = 0;
        try
        {
            for (var i = 0; i < equationsCount; i++)
            {
                pivot = NormalizeRow(matrix, i, pivot, variablesCount);
            }
        }
        catch (Exception e)
        {
            return Solution.Empty;
        }

        var particular = new long[variablesCount];
        var freeCoefficients = new long[variablesCount - pivot, variablesCount];
        for (var i = 0; i < variablesCount; i++)
        {
            particular[i] = matrix[equationsCount + i, variablesCount];
        }

        for (var k = 0; k < variablesCount - pivot; k++)
        {
            for (var variableNumber = 0; variableNumber < variablesCount; variableNumber++)
            {
                freeCoefficients[k, variableNumber] = matrix[equationsCount + variableNumber, pivot + k];
            }
        }
      
        
        var solution = new Solution(particular, freeCoefficients);
        return solution;
    }
    
    private static int NormalizeRow(long[,] a, int row, int pivot, int variablesCount)
    {
        var greatestCommonDivisor = 0L;
        for (var column = pivot; column < variablesCount; column++)
            greatestCommonDivisor = MathInt.GreatestCommonDivisor(greatestCommonDivisor, a[row, column]);

        var freeCoefficient = a[row, variablesCount];

        if (greatestCommonDivisor == 0)
        {
            if (freeCoefficient != 0)
                throw new InvalidOperationException("Inconsistent equation: 0 = nonzero");

            return pivot;
        }

        var (_, remainder) = MathInt.Divide(freeCoefficient, greatestCommonDivisor);
        if (remainder != 0)
        {
            throw new Exception(
                $"Can't normalize row {row}. GCD: {greatestCommonDivisor}, freeCoefficient: {freeCoefficient}");
        }

        while (a.Row(row).Skip(pivot).SkipLast(1).Count(x => x != 0) > 1)
        {
            var min = a.Row(row)
                .SkipLast(1)
                .Select((value, index) => (value, index))
                .Skip(pivot)
                .Where(x => x.value != 0)
                .MinBy(x => Math.Abs(x.value));

            var another = a.Row(row)
                .SkipLast(1)
                .Select((value, index) => (value, index))
                .Skip(pivot)
                .First(x => x.value != 0 && x.index != min.index);

            var (q, _) = MathInt.Divide(another.value, min.value);

            a.AddColumn(another.index, min.index, -q);
        }

        var lastNonZero = a.Row(row)
            .SkipLast(1)
            .Select((value, index) => (value, index))
            .Last(x => x.value != 0);

        var (p, r) = MathInt.Divide(freeCoefficient, lastNonZero.value);
        if (r != 0)
        {
            throw new Exception(
                $"Can't normalize row {row}. Last: {lastNonZero.value} at {lastNonZero.index}. FreeCoefficient: {freeCoefficient}");
        }

        a.AddColumn(variablesCount, lastNonZero.index, -p);

        a.SwapColumns(pivot, lastNonZero.index); // put on diaginal;

        return pivot + 1;
    }
}