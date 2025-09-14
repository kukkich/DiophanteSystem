namespace DiophanteSystem;

public readonly record struct Solution(long[] Particular, long[,] FreeVariables)
{
    public static Solution Empty => new([], new long[0,0]);
    public int FreeVariablesCount => FreeVariables.GetLength(0);
    public bool IsEmpty => Particular.Length == 0;
}