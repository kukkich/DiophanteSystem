using DiophanteSystem;


static void NormalizeRow(int[,] a, int row, int variablesCount, int equationsCount)
{
    var greatestCommonDivisor = 0;
    for (var column = row; column < variablesCount; column++)
        greatestCommonDivisor = MathInt.GreatestCommonDivisor(greatestCommonDivisor, a[row, column]);
    
    var freeCoefficient = a[row, variablesCount];

    if (greatestCommonDivisor == 0)
    {
        if (freeCoefficient != 0)
            throw new InvalidOperationException("Inconsistent equation: 0 = nonzero");
    }
    var (_, remainder) = MathInt.Divide(freeCoefficient, greatestCommonDivisor);
    if (remainder != 0)
    {
        throw new Exception(
            $"Can't normalize row {row}. GCD: {greatestCommonDivisor}, freeCoefficient: {freeCoefficient}");
    }

    while (a.Row(row).Skip(row).SkipLast(1).Count(x => x != 0) > 1)
    {
        var min = a.Row(row)
            .SkipLast(1)
            .Select((value, index) => (value, index))
            .Skip(row)
            .Where(x => x.value != 0)
            .MinBy(x => Math.Abs(x.value));

        var another = a.Row(row)
            .SkipLast(1)
            .Select((value, index) => (value, index))
            .Skip(row)
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

    if (lastNonZero.index > row)
    {
        a.SwapColumns(row, lastNonZero.index); // put on diaginal;
    }
}

static Solution Solve(int equationsCount, int[,] ints, int variablesCount)
{
    try
    {
        for (var i = 0; i < equationsCount; i++)
        {
            NormalizeRow(ints, i, variablesCount, equationsCount);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
        return Solution.Empty;
    }


    var particular = new int[variablesCount];
    var freeCoefficients = new int[variablesCount, variablesCount - equationsCount];
    for (var i = 0; i < variablesCount; i++)
    {
        particular[i] = ints[equationsCount + i, variablesCount];
    }

    for (var row = 0; row < variablesCount; row++)
    {
        for (var column = 0; column < variablesCount - equationsCount; column++)
        {
            freeCoefficients[row, column] = ints[row + equationsCount, column + variablesCount - 1];
        }
    }

    var solution1 = new Solution(particular, freeCoefficients);
    return solution1;
}

var variablesCount = 3;
var equationsCount = 4;

// var coefficients = new[,] // equationsCount, variablesCount + 1
// {
//     { 3, 4, 0, -8 },
//     { 7, 0, 5, -6 },
// };
// var coefficients = new[,] // equationsCount, variablesCount + 1
// {
//     { 2, 1, 4, -3 },
//     { 3, 5, 7, -8 },
// };

var coefficients = new[,] // equationsCount, variablesCount + 1
{
    { 0, 1, 0, 2 },
    { 0, 0, 1, -3 },
    { 1, 0, 0, -12 },
    { 0, 0, 1, -3 },
};

var matrix = new int[equationsCount + variablesCount, variablesCount + 1];

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

var solution = Solve(equationsCount, matrix, variablesCount);

Console.WriteLine("End");