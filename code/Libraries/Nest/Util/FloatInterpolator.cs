using System;
using System.Text.Json.Serialization;
using Sandbox;

namespace Nest.Util
{
    public struct FloatInterpolator
    {
        public Config Configuration { get; set; }

        public float CurrentValue { get; set; }

        public float TargetValue { get; set; }

        public FloatInterpolator(float initialValue, float targetValue, Config config)
        {
            Configuration = config;
            CurrentValue = initialValue;
            TargetValue = targetValue;
            _velocity = 0f;
            _timeElapsed = 0f;
        }

        public float Step(float targetValue)
        {
            TargetValue = targetValue;
            return Step();
        }

        public float Step()
        {
            CurrentValue = Configuration.Interpolation switch
            {
                Config.InterpolationType.Linear => Ease.Linear(_timeElapsed, Configuration.InterpolationSpeed, 0.1f, 1f) * TargetValue,
                Config.InterpolationType.Sine => Ease.InOutSine(_timeElapsed, Configuration.InterpolationSpeed, 0.1f, 1f) * TargetValue,
                Config.InterpolationType.Quadratic => Ease.InQuad(_timeElapsed, Configuration.InterpolationSpeed, 0.1f, 1f) * TargetValue,
                Config.InterpolationType.Exponential => Ease.InOutExpo(_timeElapsed, Configuration.InterpolationSpeed, 0.1f, 1f) * TargetValue,
                Config.InterpolationType.DampedSpring => Ease.DampenedSpring(CurrentValue, TargetValue, ref _velocity, Configuration.InterpolationSpeed),
                Config.InterpolationType.AnimatedCurve => Configuration.Curve.Evaluate(_timeElapsed) * TargetValue,
                _ => TargetValue,
            };

            _timeElapsed += Time.Delta;
            return CurrentValue;
        }

        private float _velocity;

        private float _timeElapsed;

        [Serializable]
        public class Config
        {
            public InterpolationType Interpolation
            {
                get
                {
                    return _interpolationType;
                }

                set
                {
                    _interpolationType = value;
                }
            }

            public Curve Curve
            {
                get
                {
                    return _curve;
                }

                set
                {
                    _curve = value;
                }
            }

            public bool Enabled
            {
                get
                {
                    return Interpolation > InterpolationType.Instant;
                }
            }

            public float InterpolationSpeed
            {
                get
                {
                    return _interpolationSpeed;
                }

                set
                {
                    _interpolationSpeed = value;
                }
            }

            [JsonInclude]
            private InterpolationType _interpolationType = InterpolationType.DampedSpring;

            [JsonInclude]
            [Range(0.1f, 50f)]
            private float _interpolationSpeed { get; set; } = 5f;

            [JsonInclude]
            private Curve _curve;

            public enum InterpolationType
            {
                Instant,
                Linear,
                Sine,
                Quadratic,
                Exponential,
                DampedSpring,
                AnimatedCurve
            }
        }
    }
}