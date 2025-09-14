namespace DiophanteSystem.Tests;

public class DiophantineSolverTests
{
    [Test(Description = "2x = 4")]
    public void SingleEquation_SimpleSolution()
    {
        long[,] coefficients =
        {
            { 2, -4 }
        }; 
        var solution = RunSolver(1, 1, coefficients);

        Assert.Multiple(() =>
        {
            DiophantineSolutionVerifier.VerifySolution(solution, coefficients);
            Assert.That(solution.FreeVariablesCount, Is.EqualTo(0));
        });
    }

    [Test(Description = "0*x = 5 → несовместна")]

    public void SingleEquation_NoSolutions()
    {
        long[,] coefficients =
        {
            { 0, -5 }
        };
        var solution = RunSolver(1, 1, coefficients);

        Assert.That(solution.IsEmpty, Is.True);
    }

    [Test(Description = """
                        x + y = 2
                        2x + 2y = 4  (1-е * 2)
                        """)]
    public void DependentEquations_InfiniteSolutions()
    {

        long[,] coefficients =
        {
            { 1, 1, -2 },
            { 2, 2, -4 }
        };
        var solution = RunSolver(2, 2, coefficients);

        Assert.Multiple(() =>
        {
            DiophantineSolutionVerifier.VerifySolution(solution, coefficients);
            Assert.That(solution.FreeVariablesCount, Is.EqualTo(1));
        });
    }

    [Test(Description = """
                        x + y = 2
                        x - y = 0
                        """)]
    public void UniqueSolution_FullRank()
    {
        long[,] coefficients =
        {
            { 1, 1, -2 },
            { 1, -1, 0 }
        };
        var solution = RunSolver(2, 2, coefficients);

        Assert.Multiple(() =>
        {
            DiophantineSolutionVerifier.VerifySolution(solution, coefficients);
            Assert.That(solution.FreeVariablesCount, Is.EqualTo(0));
        });
    }

    [Test(Description = """
                        2x = 2  (2-е / 2)
                        x = 1
                        """)]
    public void MoreEquationsThanVariables_Consistent()
    {

        long[,] coefficients =
        {
            { 2, -2 },
            { 1, -1 }
        };
        var solution = RunSolver(2, 1, coefficients);

        Assert.Multiple(() =>
        {
            DiophantineSolutionVerifier.VerifySolution(solution, coefficients);
            Assert.That(solution.FreeVariablesCount, Is.EqualTo(0));
        });
    }

    [Test(Description = """
                        x = 1
                        x = 2  (противоречие)
                        """)]
    public void MoreEquationsThanVariables_Inconsistent()
    {
        long[,] coefficients =
        {
            { 1, -1 },
            { 1, -2 }
        };
        var solution = RunSolver(2, 1, coefficients);

        Assert.That(solution.IsEmpty, Is.True);
    }

    [Test]
    public void DuplicatedRows_Consistent()
    {
        long[,] coefficients =
        {
            { 0, 1, 0,  2 },
            { 0, 0, 1, -3 },
            { 1, 0, 0, -12 },
            { 0, 0, 1, -3 }
        };
        var solution = RunSolver(4, 3, coefficients);

        DiophantineSolutionVerifier.VerifySolution(solution, coefficients);
        Assert.That(solution.FreeVariablesCount, Is.EqualTo(0));
    }

    [Test]
    public void ByHandSolved()
    {
        long[,] coefficients =
        {
            { 3, 5, 7, -8 },
            { 2, 1, 4, -3 },
        };
        var solution = RunSolver(2, 3, coefficients);

        DiophantineSolutionVerifier.VerifySolution(solution, coefficients);
        Assert.That(solution.FreeVariablesCount, Is.EqualTo(1));
    }
    
    [Test]
    public void Book_Example3()
    {
        long[,] coefficients =
        {
            { 3, 4, 0, -8 },
            { 7, 0, 5, -6 },
        };
        var solution = RunSolver(2, 3, coefficients);

        DiophantineSolutionVerifier.VerifySolution(solution, coefficients);
        Assert.That(solution.FreeVariablesCount, Is.EqualTo(1));
    }
    
    [Test]
    public void Book_Example4()
    {
        long[,] coefficients =
        {
            { 3, 6, 0, -8 },
            { 7, 0, 5, -6 },
        };
        var solution = RunSolver(2, 3, coefficients);

        Assert.That(solution.IsEmpty, Is.True);
    }
    
    [Test]
    public void Book_Example4_RowSwapped()
    {
        long[,] coefficients =
        {
            { 7, 0, 5, -6 },
            { 3, 6, 0, -8 },
        };
        var solution = RunSolver(2, 3, coefficients);

        Assert.That(solution.IsEmpty, Is.True);
    }
    
    private Solution RunSolver(int equationsCount, int variablesCount, long[,] coefficients)
    {
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
        return DiophanteSystemSolver.Solve(equationsCount, matrix, variablesCount);
    }
}