using System;
using Sandbox;

[Serializable]
[GameResource( "Float Configurable", null, null, Category = "Configurables")]
public class FloatConfigurable : Configurable
{
	[Property]
	private float defaultValue { get; set; }

	[Property]
	private float minimumValue { get; set; }

	[Property]
	private float maximumValue { get; set; } = 1f;

	[Property]
	public float MinValue => minimumValue;

	[Property]
	public float MaxValue => maximumValue;

	public override LiveData CreateDefaultLiveData()
	{
		return new LiveData( key, ConfigurableTypes.Float )
		{
			FloatValue = defaultValue
		};
	}

	public override void SetNewConfiguredValue( LiveData argument )
	{
		if ( argument.FloatValue > maximumValue )
		{
			argument.FloatValue = maximumValue;
		}
		if ( argument.FloatValue < minimumValue )
		{
			argument.FloatValue = minimumValue;
		}
		base.SetNewConfiguredValue( argument );
	}

	public void SetValue( float value )
	{
		liveData.FloatValue = value;
		SetNewConfiguredValue( liveData );
	}

	public float GetNormalizedFloatValue()
	{
		return MathX.LerpInverse( MinValue, maximumValue, GetFloatValue() );
	}

	public override string PrintValue()
	{
		return liveData.FloatValue.ToString( "F0" );
	}
}
