namespace DiophanteSystem;

public readonly record struct Solution(int[] Particular, int[,] FreeVariables)
{
    public static Solution Empty => new([], new int[0,0]);
    public int FreeVariablesCount => FreeVariables.GetLength(0);
    public bool IsEmpty => Particular.Length == 0;
}