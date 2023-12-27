using System;

namespace Sandbox
{
    public struct MathXHelper
    {
        public static float Sin(float f)
        {
            return (float)Math.Sin((double)f);
        }

        public static float Cos(float f)
        {
            return (float)Math.Cos((double)f);
        }

        public static float Tan(float f)
        {
            return (float)Math.Tan((double)f);
        }

        public static float Asin(float f)
        {
            return (float)Math.Asin((double)f);
        }

        public static float Acos(float f)
        {
            return (float)Math.Acos((double)f);
        }

        public static float Atan(float f)
        {
            return (float)Math.Atan((double)f);
        }

        public static float Atan2(float y, float x)
        {
            return (float)Math.Atan2((double)y, (double)x);
        }

        public static float Sqrt(float f)
        {
            return (float)Math.Sqrt((double)f);
        }

        public static float Abs(float f)
        {
            return Math.Abs(f);
        }

        public static int Abs(int value)
        {
            return Math.Abs(value);
        }

        public static float Min(float a, float b)
        {
            return (a < b) ? a : b;
        }

        public static float Min(params float[] values)
        {
            int num = values.Length;
            bool flag = num == 0;
            float num2;
            if (flag)
            {
                num2 = 0f;
            }
            else
            {
                float num3 = values[0];
                for (int i = 1; i < num; i++)
                {
                    bool flag2 = values[i] < num3;
                    if (flag2)
                    {
                        num3 = values[i];
                    }
                }
                num2 = num3;
            }
            return num2;
        }

        public static int Min(int a, int b)
        {
            return (a < b) ? a : b;
        }

        public static int Min(params int[] values)
        {
            int num = values.Length;
            bool flag = num == 0;
            int num2;
            if (flag)
            {
                num2 = 0;
            }
            else
            {
                int num3 = values[0];
                for (int i = 1; i < num; i++)
                {
                    bool flag2 = values[i] < num3;
                    if (flag2)
                    {
                        num3 = values[i];
                    }
                }
                num2 = num3;
            }
            return num2;
        }

        public static float Max(float a, float b)
        {
            return (a > b) ? a : b;
        }

        public static float Max(params float[] values)
        {
            int num = values.Length;
            bool flag = num == 0;
            float num2;
            if (flag)
            {
                num2 = 0f;
            }
            else
            {
                float num3 = values[0];
                for (int i = 1; i < num; i++)
                {
                    bool flag2 = values[i] > num3;
                    if (flag2)
                    {
                        num3 = values[i];
                    }
                }
                num2 = num3;
            }
            return num2;
        }

        public static int Max(int a, int b)
        {
            return (a > b) ? a : b;
        }

        public static int Max(params int[] values)
        {
            int num = values.Length;
            bool flag = num == 0;
            int num2;
            if (flag)
            {
                num2 = 0;
            }
            else
            {
                int num3 = values[0];
                for (int i = 1; i < num; i++)
                {
                    bool flag2 = values[i] > num3;
                    if (flag2)
                    {
                        num3 = values[i];
                    }
                }
                num2 = num3;
            }
            return num2;
        }

        public static float Pow(float f, float p)
        {
            return (float)Math.Pow((double)f, (double)p);
        }

        public static float Exp(float power)
        {
            return (float)Math.Exp((double)power);
        }

        public static float Log(float f, float p)
        {
            return (float)Math.Log((double)f, (double)p);
        }

        public static float Log(float f)
        {
            return (float)Math.Log((double)f);
        }

        public static float Log10(float f)
        {
            return (float)Math.Log10((double)f);
        }

        public static float Ceil(float f)
        {
            return (float)Math.Ceiling((double)f);
        }

        public static float Floor(float f)
        {
            return (float)Math.Floor((double)f);
        }

        public static float Round(float f)
        {
            return (float)Math.Round((double)f);
        }

        public static int CeilToInt(float f)
        {
            return (int)Math.Ceiling((double)f);
        }

        public static int FloorToInt(float f)
        {
            return (int)Math.Floor((double)f);
        }

        public static int RoundToInt(float f)
        {
            return (int)Math.Round((double)f);
        }

        public static float Sign(float f)
        {
            return (f >= 0f) ? 1f : (-1f);
        }

        public static float Clamp(float value, float min, float max)
        {
            bool flag = value < min;
            if (flag)
            {
                value = min;
            }
            else
            {
                bool flag2 = value > max;
                if (flag2)
                {
                    value = max;
                }
            }
            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            bool flag = value < min;
            if (flag)
            {
                value = min;
            }
            else
            {
                bool flag2 = value > max;
                if (flag2)
                {
                    value = max;
                }
            }
            return value;
        }

        public static float Clamp01(float value)
        {
            bool flag = value < 0f;
            float num;
            if (flag)
            {
                num = 0f;
            }
            else
            {
                bool flag2 = value > 1f;
                if (flag2)
                {
                    num = 1f;
                }
                else
                {
                    num = value;
                }
            }
            return num;
        }

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Clamp01(t);
        }

        public static float LerpUnclamped(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        public static float LerpAngle(float a, float b, float t)
        {
            float num = Repeat(b - a, 360f);
            bool flag = num > 180f;
            if (flag)
            {
                num -= 360f;
            }
            return a + num * Clamp01(t);
        }

        public static float MoveTowards(float current, float target, float maxDelta)
        {
            bool flag = Abs(target - current) <= maxDelta;
            float num;
            if (flag)
            {
                num = target;
            }
            else
            {
                num = current + Sign(target - current) * maxDelta;
            }
            return num;
        }

        public static float MoveTowardsAngle(float current, float target, float maxDelta)
        {
            float num = DeltaAngle(current, target);
            bool flag = -maxDelta < num && num < maxDelta;
            float num2;
            if (flag)
            {
                num2 = target;
            }
            else
            {
                target = current + num;
                num2 = MoveTowards(current, target, maxDelta);
            }
            return num2;
        }

        public static float SmoothStep(float from, float to, float t)
        {
            t = Clamp01(t);
            t = -2f * t * t * t + 3f * t * t;
            return to * t + from * (1f - t);
        }

        public static float Gamma(float value, float absmax, float gamma)
        {
            bool flag = false;
            bool flag2 = value < 0f;
            if (flag2)
            {
                flag = true;
            }
            float num = Abs(value);
            bool flag3 = num > absmax;
            float num2;
            if (flag3)
            {
                num2 = (flag ? (-num) : num);
            }
            else
            {
                float num3 = Pow(num / absmax, gamma) * absmax;
                num2 = (flag ? (-num3) : num3);
            }
            return num2;
        }

        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
        {
            float deltaTime = Time.Delta;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime)
        {
            float deltaTime = Time.Delta;
            float positiveInfinity = float.PositiveInfinity;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, positiveInfinity, deltaTime);
        }

        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, [DefaultValue("Infinity")] float maxSpeed, [DefaultValue("Time.deltaTime")] float deltaTime)
        {
            smoothTime = Max(0.0001f, smoothTime);
            float num = 2f / smoothTime;
            float num2 = num * deltaTime;
            float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
            float num4 = current - target;
            float num5 = target;
            float num6 = maxSpeed * smoothTime;
            num4 = Clamp(num4, -num6, num6);
            target = current - num4;
            float num7 = (currentVelocity + num * num4) * deltaTime;
            currentVelocity = (currentVelocity - num * num7) * num3;
            float num8 = target + (num4 + num7) * num3;
            bool flag = num5 - current > 0f == num8 > num5;
            if (flag)
            {
                num8 = num5;
                currentVelocity = (num8 - num5) / deltaTime;
            }
            return num8;
        }

        public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
        {
            float deltaTime = Time.Delta;
            return SmoothDampAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime)
        {
            float deltaTime = Time.Delta;
            float positiveInfinity = float.PositiveInfinity;
            return SmoothDampAngle(current, target, ref currentVelocity, smoothTime, positiveInfinity, deltaTime);
        }

        public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, [DefaultValue("Infinity")] float maxSpeed, [DefaultValue("Time.deltaTime")] float deltaTime)
        {
            target = current + DeltaAngle(current, target);
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static float Repeat(float t, float length)
        {
            return Clamp(t - Floor(t / length) * length, 0f, length);
        }

        public static float PingPong(float t, float length)
        {
            t = Repeat(t, length * 2f);
            return length - Abs(t - length);
        }

        public static float InverseLerp(float a, float b, float value)
        {
            bool flag = a != b;
            float num;
            if (flag)
            {
                num = Clamp01((value - a) / (b - a));
            }
            else
            {
                num = 0f;
            }
            return num;
        }

        public static float DeltaAngle(float current, float target)
        {
            float num = Repeat(target - current, 360f);
            bool flag = num > 180f;
            if (flag)
            {
                num -= 360f;
            }
            return num;
        }

        internal static bool LineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, ref Vector2 result)
        {
            float num = p2.x - p1.x;
            float num2 = p2.y - p1.y;
            float num3 = p4.x - p3.x;
            float num4 = p4.y - p3.y;
            float num5 = num * num4 - num2 * num3;
            bool flag = num5 == 0f;
            bool flag2;
            if (flag)
            {
                flag2 = false;
            }
            else
            {
                float num6 = p3.x - p1.x;
                float num7 = p3.y - p1.y;
                float num8 = (num6 * num4 - num7 * num3) / num5;
                result.x = p1.x + num8 * num;
                result.y = p1.y + num8 * num2;
                flag2 = true;
            }
            return flag2;
        }

        internal static bool LineSegmentIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, ref Vector2 result)
        {
            float num = p2.x - p1.x;
            float num2 = p2.y - p1.y;
            float num3 = p4.x - p3.x;
            float num4 = p4.y - p3.y;
            float num5 = num * num4 - num2 * num3;
            bool flag = num5 == 0f;
            bool flag2;
            if (flag)
            {
                flag2 = false;
            }
            else
            {
                float num6 = p3.x - p1.x;
                float num7 = p3.y - p1.y;
                float num8 = (num6 * num4 - num7 * num3) / num5;
                bool flag3 = num8 < 0f || num8 > 1f;
                if (flag3)
                {
                    flag2 = false;
                }
                else
                {
                    float num9 = (num6 * num2 - num7 * num) / num5;
                    bool flag4 = num9 < 0f || num9 > 1f;
                    if (flag4)
                    {
                        flag2 = false;
                    }
                    else
                    {
                        result.x = p1.x + num8 * num;
                        result.y = p1.y + num8 * num2;
                        flag2 = true;
                    }
                }
            }
            return flag2;
        }

        internal static long RandomToLong(Random r)
        {
            byte[] array = new byte[8];
            r.NextBytes(array);
            return (long)(BitConverter.ToUInt64(array, 0) & 9223372036854775807UL);
        }

        public const float PI = 3.1415927f;

        public const float Infinity = float.PositiveInfinity;

        public const float NegativeInfinity = float.NegativeInfinity;

        public const float Deg2Rad = 0.017453292f;

        public const float Rad2Deg = 57.29578f;
    }
}