namespace DiophanteSystem.Tests;

public static class DiophantineSolutionVerifier
{
    public static void VerifySolution(Solution solution, long[,] coefficients)
    {
        var variablesCount = coefficients.GetLength(1) - 1;

        var particular = solution.Particular;
        var freeVectors = solution.FreeVariables;

        // частное решение
        Assert.That(Satisfies(coefficients, particular), Is.True);

        // Проверка для каждого свободного вектора:
        for (var k = 0; k < solution.FreeVariablesCount; k++)
        {
            var candidate = new long[variablesCount];
            for (var j = 0; j < variablesCount; j++)
                candidate[j] = particular[j] + freeVectors[k, j];

            Assert.That(Satisfies(coefficients, candidate), Is.True);
        }
    }

    private static bool Satisfies(long[,] coeffs, long[] vector)
    {
        var n = coeffs.GetLength(0);
        var m = coeffs.GetLength(1) - 1;

        for (var i = 0; i < n; i++)
        {
            long lhs = 0;
            for (var j = 0; j < m; j++)
                lhs += coeffs[i, j] * vector[j];

            if (lhs != -1 * coeffs[i, m])
                return false;
        }

        return true;
    }
}