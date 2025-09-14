namespace DiophanteSystem.Tests;

public static class DiophantineSolutionVerifier
{
    public static void VerifySolution(Solution solution, int[,] coefficients)
    {
        var variablesCount = coefficients.GetLength(1) - 1;

        var particular = solution.Particular;
        var freeVectors = solution.FreeVariables;

        // 1. Проверка: частное решение удовлетворяет системе
        Assert.That(Satisfies(coefficients, particular), Is.True);

        // 2. Проверка для каждого свободного вектора:
        for (var k = 0; k < solution.FreeVariablesCount; k++)
        {
            var candidate = new int[variablesCount];
            for (var j = 0; j < variablesCount; j++)
                candidate[j] = particular[j] + freeVectors[k, j];

            Assert.That(Satisfies(coefficients, candidate), Is.True);
        }
    }

    private static bool Satisfies(int[,] coeffs, int[] vector)
    {
        var n = coeffs.GetLength(0);
        var m = coeffs.GetLength(1) - 1;

        for (var i = 0; i < n; i++)
        {
            long lhs = 0;
            for (var j = 0; j < m; j++)
                lhs += (long)coeffs[i, j] * vector[j];

            if (lhs != -1 * coeffs[i, m])
                return false;
        }

        return true;
    }
}