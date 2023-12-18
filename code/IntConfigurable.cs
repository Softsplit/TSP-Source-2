using System;
using Sandbox;

[Serializable]
[GameResource( "Int Configurable", null, null, Category = "Configurables" )]
public class IntConfigurable : Configurable
{
	[Property]
	private int defaultValue { get; set; }

	[Property]
	private int minimumValue { get; set; }

	[Property]
	private int maximumValue { get; set; } = 1;

	[Property]
	private bool clampOnOverflow {  get; set; }

	[Property]
	public int MinValue => minimumValue;

	[Property]
	public int MaxValue => maximumValue;

	public override LiveData CreateDefaultLiveData()
	{
		return new LiveData( key, ConfigurableTypes.Int )
		{
			IntValue = defaultValue
		};
	}

	public void IncreaseValue()
	{
		liveData.IntValue++;
		if ( !clampOnOverflow && liveData.IntValue > maximumValue )
		{
			liveData.IntValue = minimumValue;
		}
		SetNewConfiguredValue( liveData );
	}

	public void DecreaseValue()
	{
		liveData.IntValue--;
		if ( !clampOnOverflow && liveData.IntValue < minimumValue )
		{
			liveData.IntValue = maximumValue;
		}
		SetNewConfiguredValue( liveData );
	}

	public void SetValue( int value )
	{
		liveData.IntValue = value;
		SetNewConfiguredValue( liveData );
	}

	public void SetNewMaxValue( int newMaxValue )
	{
		maximumValue = newMaxValue;
	}

	public void SetNewMinValue( int newMinValue )
	{
		minimumValue = newMinValue;
	}

	public override void SetNewConfiguredValue( LiveData argument )
	{
		if ( argument.IntValue > maximumValue )
		{
			argument.IntValue = maximumValue;
		}
		if ( argument.IntValue < minimumValue )
		{
			argument.IntValue = minimumValue;
		}
		base.SetNewConfiguredValue( argument );
	}
}
