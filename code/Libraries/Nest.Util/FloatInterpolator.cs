using System;
using Sandbox;
namespace Nest.Util;
public struct FloatInterpolator
{
	[Serializable]
	public class Config
	{
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

		[Property]
		private InterpolationType _interpolationType { get; set; } = InterpolationType.DampedSpring;

		[Property]
		[Range( 0.1f, 50f )]
		private float _interpolationSpeed { get; set; } = 5f;

		/*
		[Property]
		private AnimationCurve _curve { get; set; } = AnimationCurve.EaseInOut( 0f, 0f, 1f, 1f );
		*/

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

		/*
		public AnimationCurve Curve
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
		*/

		public bool Enabled => Interpolation != InterpolationType.Instant;

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
	}

	private float _velocity;

	private float _timeElapsed;

	public Config Configuration { get; set; }

	public float CurrentValue { get; set; }

	public float TargetValue { get; set; }

	public FloatInterpolator( float initialValue, float targetValue, Config config )
	{
		Configuration = config;
		CurrentValue = initialValue;
		TargetValue = targetValue;
		_velocity = 0f;
		_timeElapsed = 0f;
	}

	public float Step( float targetValue )
	{
		TargetValue = targetValue;
		return Step();
	}

	public float Step()
	{
		switch ( Configuration.Interpolation )
		{
			case Config.InterpolationType.Exponential:
				// CurrentValue = Ease.InOutExpo( _timeElapsed, Configuration.InterpolationSpeed ) * TargetValue;
				break;
			case Config.InterpolationType.DampedSpring:
				// CurrentValue = Ease.DampenedSpring( CurrentValue, TargetValue, ref _velocity, Configuration.InterpolationSpeed );
				break;
			case Config.InterpolationType.AnimatedCurve:
				// CurrentValue = Configuration.Curve.Evaluate( _timeElapsed ) * TargetValue;
				break;
			case Config.InterpolationType.Sine:
				// CurrentValue = Ease.InOutSine( _timeElapsed, Configuration.InterpolationSpeed ) * TargetValue;
				break;
			case Config.InterpolationType.Quadratic:
				// CurrentValue = Ease.InQuad( _timeElapsed, Configuration.InterpolationSpeed ) * TargetValue;
				break;
			case Config.InterpolationType.Linear:
				// CurrentValue = Ease.Linear( _timeElapsed, Configuration.InterpolationSpeed ) * TargetValue;
				break;
			default:
				CurrentValue = TargetValue;
				break;
		}
		_timeElapsed += Time.Delta;
		return CurrentValue;
	}
}
