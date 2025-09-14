using DiophanteSystem;

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
