using System;
using Sandbox;

[Serializable]
[GameResource( "String Configurable", null, null, Category = "Configurables" )]
public class StringConfigurable : Configurable
{
	[Property]
	private string defaultValue { get; set; } = "";

	public override LiveData CreateDefaultLiveData()
	{
		return new LiveData( key, ConfigurableTypes.String )
		{
			StringValue = defaultValue
		};
	}

	public void SetValue( string value )
	{
		liveData.StringValue = value;
		SetNewConfiguredValue( liveData );
	}
}
