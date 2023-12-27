using System;
using Sandbox;

[Serializable]
public struct MinMax
{
    public MinMax(float low, float high)
    {
        Min = low;
        Max = high;
    }

    public MinMax(float i)
    {
        Min = i;
        Max = i;
    }

    public float Range()
    {
        return MathF.Abs(Max - Min);
    }

    public float Random()
    {
        return (float)(Game.Random.NextDouble() * (Max - Min) + Min);
    }

    public float MinOrMax()
    {
        if (Game.Random.NextDouble() > 0.5f)
        {
            return Min;
        }

        return Max;
    }

    public float Lerp(float t)
    {
        t = MathXHelper.Clamp01(t);
        return LerpUnclamped(t);
    }

    public float LerpUnclamped(float t)
    {
        return (1f - t) * Min + t * Max;
    }

    public float Average()
    {
        return Lerp(0.5f);
    }

    public float ILerp(float t)
    {
        return (t - Min) / Range();
    }

    public static MinMax operator *(MinMax mm, float f)
    {
        return new MinMax(mm.Min * f, mm.Max * f);
    }

    public static MinMax operator *(MinMax mm, MinMax mm2)
    {
        return new MinMax(mm.Min * mm2.Min, mm.Max * mm2.Max);
    }

    public float Min;

    public float Max;
}