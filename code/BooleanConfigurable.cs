using System;
using Sandbox;

[Serializable]
[GameResource( "Boolean Configurable", null, null, Category = "Configurables" )]
public class BooleanConfigurable : Configurable
{
	[Property]
	private bool defaultValue { get; set; }

	public override LiveData CreateDefaultLiveData()
	{
		return new LiveData( key, ConfigurableTypes.Boolean )
		{
			BooleanValue = defaultValue
		};
	}

	public override void SetNewConfiguredValue( LiveData argument )
	{
		base.SetNewConfiguredValue( argument );
	}

	public void SetValue( bool value )
	{
		liveData.BooleanValue = value;
		SetNewConfiguredValue( liveData );
	}
}
