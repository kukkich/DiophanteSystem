namespace DiophanteSystem;

public static class MathInt
{
    public static int GreatestCommonDivisor(int a, int b)
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
    public static (int Quotient, int Remainder) Divide(int a, int b)
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