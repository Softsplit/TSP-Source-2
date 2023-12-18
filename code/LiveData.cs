using System;

[Serializable]
public class LiveData
{
	public string key;

	public ConfigurableTypes configureableType;

	public int IntValue;

	public float FloatValue;

	public bool BooleanValue;

	public string StringValue;

	public LiveData( string _key, ConfigurableTypes _type )
	{
		key = _key;
		configureableType = _type;
	}

	public void CopyValuesFrom( LiveData data )
	{
		IntValue = data.IntValue;
		FloatValue = data.FloatValue;
		BooleanValue = data.BooleanValue;
		StringValue = data.StringValue;
	}
}
