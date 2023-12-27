using System;
using Sandbox;

namespace Nest.Util
{
    internal static class Ease
    {
        public static float Linear(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            return time / duration;
        }

        public static float InSine(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            return -MathF.Cos(time / duration * _piOver2) + 1f;
        }

        public static float OutSine(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            return MathF.Sin(time / duration * _piOver2);
        }

        public static float InOutSine(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            return -0.5f * (MathF.Cos(3.1415927f * time / duration) - 1f);
        }

        public static float InQuad(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            return (time /= duration) * time;
        }

        public static float OutQuad(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            return -(time /= duration) * (time - 2f);
        }

        public static float InOutQuad(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            if ((time /= duration * 0.5f) < 1f)
            {
                return 0.5f * time * time;
            }

            return -0.5f * ((time -= 1f) * (time - 2f) - 1f);
        }

        public static float InCubic(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            return (time /= duration) * time * time;
        }

        public static float OutCubic(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            return (time = time / duration - 1f) * time * time + 1f;
        }

        public static float InOutCubic(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            if ((time /= duration * 0.5f) < 1f)
            {
                return 0.5f * time * time * time;
            }

            return 0.5f * ((time -= 2f) * time * time + 2f);
        }

        public static float InQuart(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            return (time /= duration) * time * time * time;
        }

        public static float OutQuart(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            return -((time = time / duration - 1f) * time * time * time - 1f);
        }

        public static float InOutQuart(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            if ((time /= duration * 0.5f) < 1f)
            {
                return 0.5f * time * time * time * time;
            }

            return -0.5f * ((time -= 2f) * time * time * time - 2f);
        }

        public static float InQuint(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            return (time /= duration) * time * time * time * time;
        }

        public static float OutQuint(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            return (time = time / duration - 1f) * time * time * time * time + 1f;
        }

        public static float InOutQuint(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            if ((time /= duration * 0.5f) < 1f)
            {
                return 0.5f * time * time * time * time * time;
            }

            return 0.5f * ((time -= 2f) * time * time * time * time + 2f);
        }

        public static float InExpo(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            if (time != 0f)
            {
                return MathF.Pow(2f, 10f * (time / duration - 1f));
            }

            return 0f;
        }

        public static float OutExpo(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            if (time == duration)
            {
                return 1f;
            }

            return -MathF.Pow(2f, -10f * time / duration) + 1f;
        }

        public static float InOutExpo(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            if (time == 0f)
            {
                return 0f;
            }

            if (time == duration)
            {
                return 1f;
            }

            if ((time /= duration * 0.5f) < 1f)
            {
                return 0.5f * MathF.Pow(2f, 10f * (time - 1f));
            }

            return 0.5f * (-MathF.Pow(2f, -10f * (time -= 1f)) + 2f);
        }

        public static float InCirc(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            return -(MathF.Sqrt(1f - (time /= duration) * time) - 1f);
        }

        public static float OutCirc(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            return MathF.Sqrt(1f - (time = time / duration - 1f) * time);
        }

        public static float InOutCirc(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            if ((time /= duration * 0.5f) < 1f)
            {
                return -0.5f * (MathF.Sqrt(1f - time * time) - 1f);
            }

            return 0.5f * (MathF.Sqrt(1f - (time -= 2f) * time) + 1f);
        }

        public static float InElastic(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            if (time == 0f)
            {
                return 0f;
            }

            if ((time /= duration) == 1f)
            {
                return 1f;
            }

            if (period == 0f)
            {
                period = duration * 0.3f;
            }

            float num;

            if (overshootOrAmplitude < 1f)
            {
                overshootOrAmplitude = 1f;
                num = period / 4f;
            }
            else
            {
                num = period / _twoPi * MathF.Asin(1f / overshootOrAmplitude);
            }

            return -(overshootOrAmplitude * MathF.Pow(2f, 10f * (time -= 1f)) * MathF.Sin((time * duration - num) * _twoPi / period));
        }

        public static float OutElastic(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            if (time == 0f)
            {
                return 0f;
            }

            if ((time /= duration) == 1f)
            {
                return 1f;
            }

            if (period == 0f)
            {
                period = duration * 0.3f;
            }

            float num;

            if (overshootOrAmplitude < 1f)
            {
                overshootOrAmplitude = 1f;
                num = period / 4f;
            }
            else
            {
                num = period / _twoPi * MathF.Asin(1f / overshootOrAmplitude);
            }

            return overshootOrAmplitude * MathF.Pow(2f, -10f * time) * MathF.Sin((time * duration - num) * _twoPi / period) + 1f;
        }

        public static float InOutElastic(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            if (time == 0f)
            {
                return 0f;
            }

            if ((time /= duration * 0.5f) == 2f)
            {
                return 1f;
            }

            if (period == 0f)
            {
                period = duration * 0.45000002f;
            }

            float num;

            if (overshootOrAmplitude < 1f)
            {
                overshootOrAmplitude = 1f;
                num = period / 4f;
            }
            else
            {
                num = period / _twoPi * MathF.Asin(1f / overshootOrAmplitude);
            }

            if (time < 1f)
            {
                return -0.5f * (overshootOrAmplitude * MathF.Pow(2f, 10f * (time -= 1f)) * MathF.Sin((time * duration - num) * _twoPi / period));
            }

            return overshootOrAmplitude * MathF.Pow(2f, -10f * (time -= 1f)) * MathF.Sin((time * duration - num) * _twoPi / period) * 0.5f + 1f;
        }

        public static float InBack(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            return (time /= duration) * time * ((overshootOrAmplitude + 1f) * time - overshootOrAmplitude);
        }

        public static float OutBack(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            return (time = time / duration - 1f) * time * ((overshootOrAmplitude + 1f) * time + overshootOrAmplitude) + 1f;
        }

        public static float InOutBack(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
        {
            if ((time /= duration * 0.5f) < 1f)
            {
                return 0.5f * (time * time * (((overshootOrAmplitude *= 1.525f) + 1f) * time - overshootOrAmplitude));
            }

            return 0.5f * ((time -= 2f) * time * (((overshootOrAmplitude *= 1.525f) + 1f) * time + overshootOrAmplitude) + 2f);
        }

        public static float DampenedSpring(float current, float target, ref float velocity, float omega)
        {
            float deltaTime = Time.Delta;
            float num = velocity - (current - target) * (omega * omega * deltaTime);
            float num2 = 1f + omega * deltaTime;
            velocity = num / (num2 * num2);
            return current + velocity * deltaTime;
        }

        private static float _piOver2 = 1.5707964f;

        private static float _twoPi = 6.2831855f;
    }
}