using DiophanteSystem;

static void NormalizeRow(int [,] a, int row, int variablesCount, int equationsCount)
{
    var greatestCommonDivisor = MathInt.GreatestCommonDivisor(a[row, 0],  a[row, 1]);
    for (var i = 2; i < variablesCount; i++)
    {
        greatestCommonDivisor = MathInt.GreatestCommonDivisor(greatestCommonDivisor, a[row, i]);
    }

    var freeCoefficient = a[row, variablesCount];
    var (_, remainder) = MathInt.Divide(freeCoefficient, greatestCommonDivisor);
    if (remainder != 0)
    {
        throw new Exception($"Can't normalize row {row}. GCD: {greatestCommonDivisor}, freeCoefficient: {freeCoefficient}");
    }

    if (row > 0)
        throw new NotImplementedException();

    while (a.Row(row).SkipLast(1).Count(x => x != 0) > row + 1)
    {
        var min = a.Row(row)
            .SkipLast(1)
            .Select((value, index) => (value, index))
            .Where(x => x.value != 0)
            .MinBy(x => Math.Abs(x.value));
        
        var another = a.Row(row)
            .SkipLast(1)
            .Select((value, index) => (value, index))
            .First(x => x.value != 0 && x.index != min.index);
        
        var (q, _) = MathInt.Divide(another.value, min.value);
        
        a.AddColumn(another.index, min.index, -q);
    }

    var lastNonZero = a.Row(row)
        .SkipLast(1)
        .Select((value, index) => (value, index))
        .First(x => x.value != 0);
    
    var (p, r) = MathInt.Divide(freeCoefficient, lastNonZero.value);
    if (r == 0)
    {
        throw new Exception($"Can't normalize row {row}. Last: {lastNonZero.value} at {lastNonZero.index}. FreeCoefficient: {freeCoefficient}");
    }
    
    a.AddColumn(variablesCount, lastNonZero.index, -p);
}


var variablesCount = 3;
var equationsCount = 2;

var coefficients = new [,] // equationsCount, variablesCount + 1
{
    {3, 4, 0, -8},
    {7, 0, 5, -6},
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


NormalizeRow(matrix, 0, variablesCount, equationsCount);

Console.WriteLine("Hello, World!");


