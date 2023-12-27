using Sandbox;

public struct fint32
{
    public fint32(uint i)
    {
        integer = i;
        fraction = 0U;
    }

    public fint32(uint i, uint f)
    {
        integer = i;
        fraction = f;
    }

    public static implicit operator fint32(float f)
    {
        if (f < 0f)
        {
            return new fint32(0U);
        }

        fint32 fint;
        fint.integer = (uint)MathX.FloorToInt(f);
        fint.fraction = (uint)(100000000f * (f - fint.integer));
        return fint;
    }

    public static implicit operator float(fint32 fi)
    {
        return fi.integer + fi.fraction / FINT32_MAX_FRACTION;
    }

    public static fint32 operator +(fint32 a, fint32 b)
    {
        fint32 fint;
        fint.integer = a.integer + b.integer;
        uint num = a.fraction + b.fraction;
        if (num > FINT32_MAX_FRACTION)
        {
            num -= FINT32_MAX_FRACTION;
            fint.integer += 1U;
        }
        fint.fraction = num;
        return fint;
    }

    public static fint32 operator +(fint32 a, float b)
    {
        fint32 fint = b;
        fint32 fint2;
        fint2.integer = a.integer + fint.integer;
        uint num = a.fraction + fint.fraction;

        if (num > FINT32_MAX_FRACTION)
        {
            num -= FINT32_MAX_FRACTION;
            fint2.integer += 1U;
        }

        fint2.fraction = num;
        return fint2;
    }

    public static bool operator >(fint32 a, fint32 b)
    {
        return a.integer > b.integer || (a.integer >= b.integer && a.fraction > b.fraction);
    }

    public static bool operator <(fint32 a, fint32 b)
    {
        return a.integer < b.integer || (a.integer <= b.integer && a.fraction < b.fraction);
    }

    public override string ToString()
    {
        return string.Concat(new object[]
        {
            "( ",
            integer,
            ".",
            fraction.ToString("D8"),
            " )"
        });
    }

    private uint integer;

    private uint fraction;

    public static readonly uint FINT32_MAX_FRACTION = 100000000U;
}