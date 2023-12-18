using System;
using Sandbox;

public class Configurable : GameResource
{
	[Property]
	public static ConfigurableDataContainer ConfigurableDataContainer { get; set; }

	[Property]
	public static bool Dirty { get; set; }

	[Property]
	public Action<LiveData> OnValueChanged { get; set; }

	[NonSerialized]
	private bool initialized;

	[NonSerialized]
	private bool deviatesFromSavedValue;

	[Group( "Configuration" )]
	[Property]
	protected string key { get; set; } = "ConfigurableKey";

	[Group( "Configuration" )]
	[Property]
	private string description { get; set; } = "Say something about this configurable";

	[Group( "Configuration" )]
	[Property]
	private bool persistent { get; set; } = true;

	[NonSerialized]
	[Group( "Data" )]
	private LiveData saveDataCache;

	protected LiveData liveData;

	private bool triedToInitBeforePlayerPrefs;

	[Group( "Data" )]
	[Property]
	public string Key => key;

	[Group( "Data" )]
	[Property]
	public bool DeviatesFromSavedValue => deviatesFromSavedValue;

	public void Init()
	{
		if ( !initialized )
		{
			if ( ConfigurableDataContainer == null )
			{
				ConfigurableDataContainer = ConfigurableDataContainer.LoadContainer( "data" );
			}
			liveData = LoadOrCreateSaveData();
			UpdateSaveDataCache();
			ConfigurableDataContainer.OnSaveValues = (Action)Delegate.Combine( ConfigurableDataContainer.OnSaveValues, new Action( OnSaveValues ) );
			ConfigurableDataContainer.OnResetValues = (Action)Delegate.Combine( ConfigurableDataContainer.OnResetValues, new Action( OnResetValues ) );
			initialized = true;
		}
	}

	public void ForceUpdate()
	{
		if ( OnValueChanged != null )
		{
			OnValueChanged( liveData );
		}
	}

	private void OnDestroy()
	{
		ConfigurableDataContainer.OnSaveValues = (Action)Delegate.Remove( ConfigurableDataContainer.OnSaveValues, new Action( OnSaveValues ) );
		ConfigurableDataContainer.OnResetValues = (Action)Delegate.Remove( ConfigurableDataContainer.OnResetValues, new Action( OnResetValues ) );
	}

	private void UpdateSaveDataCache()
	{
		if ( persistent && saveDataCache == null )
		{
			saveDataCache = new LiveData( liveData.key, liveData.configureableType );
		}
		if ( saveDataCache != null )
		{
			saveDataCache.CopyValuesFrom( liveData );
		}
	}

	public virtual void SetNewConfiguredValue( LiveData data )
	{
		if ( !initialized )
		{
			Init();
		}
		if ( saveDataCache == null )
		{
			deviatesFromSavedValue = true;
			switch ( data.configureableType )
			{
				case ConfigurableTypes.Int:
					liveData.IntValue = data.IntValue;
					break;
				case ConfigurableTypes.Float:
					liveData.FloatValue = data.FloatValue;
					break;
				case ConfigurableTypes.Boolean:
					liveData.BooleanValue = data.BooleanValue;
					break;
				case ConfigurableTypes.String:
					liveData.StringValue = data.StringValue;
					break;
			}
			if ( OnValueChanged != null )
			{
				OnValueChanged( liveData );
			}
			return;
		}
		switch ( data.configureableType )
		{
			case ConfigurableTypes.Int:
				deviatesFromSavedValue = saveDataCache.IntValue != data.IntValue;
				liveData.IntValue = data.IntValue;
				break;
			case ConfigurableTypes.Float:
				deviatesFromSavedValue = saveDataCache.FloatValue != data.FloatValue;
				liveData.FloatValue = data.FloatValue;
				break;
			case ConfigurableTypes.Boolean:
				deviatesFromSavedValue = saveDataCache.BooleanValue != data.BooleanValue;
				liveData.BooleanValue = data.BooleanValue;
				break;
			case ConfigurableTypes.String:
				deviatesFromSavedValue = !saveDataCache.StringValue.Equals( data.StringValue );
				liveData.StringValue = data.StringValue;
				break;
		}
		if ( OnValueChanged != null )
		{
			OnValueChanged( liveData );
		}
	}

	public virtual LiveData LoadOrCreateSaveData()
	{
		LiveData savedDataFromContainer = ConfigurableDataContainer.GetSavedDataFromContainer( key );
		if ( persistent && savedDataFromContainer != null && IsValidSaveData( savedDataFromContainer ) )
		{
			return savedDataFromContainer;
		}
		return CreateDefaultLiveData();
	}

	public virtual bool IsValidSaveData( LiveData data )
	{
		bool flag = false;
		return data.configureableType switch
		{
			ConfigurableTypes.Int => this is IntConfigurable,
			ConfigurableTypes.Float => this is FloatConfigurable,
			ConfigurableTypes.Boolean => this is BooleanConfigurable,
			ConfigurableTypes.String => this is StringConfigurable,
			_ => false,
		};
	}

	public virtual LiveData LoadOrCreateSaveData( out bool saveDataExists )
	{
		LiveData savedDataFromContainer = ConfigurableDataContainer.GetSavedDataFromContainer( key );
		if ( persistent && savedDataFromContainer != null && IsValidSaveData( savedDataFromContainer ) )
		{
			saveDataExists = true;
			return savedDataFromContainer;
		}
		saveDataExists = false;
		return CreateDefaultLiveData();
	}

	private void OnResetValues()
	{
		if ( deviatesFromSavedValue )
		{
			liveData.CopyValuesFrom( saveDataCache );
			if ( OnValueChanged != null )
			{
				OnValueChanged( liveData );
			}
			deviatesFromSavedValue = false;
		}
	}

	private void OnSaveValues()
	{
		if ( deviatesFromSavedValue )
		{
			if ( persistent )
			{
				ConfigurableDataContainer.UpdateSaveDataCache( liveData );
			}
			deviatesFromSavedValue = false;
			if ( persistent )
			{
				UpdateSaveDataCache();
			}
			if ( OnValueChanged != null )
			{
				OnValueChanged( liveData );
			}
			if ( persistent )
			{
				Dirty = true;
			}
		}
	}

	public virtual LiveData CreateDefaultLiveData()
	{
		return null;
	}

	public virtual void SaveToDiskAll()
	{
		if ( ConfigurableDataContainer == null )
		{
			ConfigurableDataContainer = ConfigurableDataContainer.LoadContainer( "data" );
		}
		ConfigurableDataContainer.SaveToPlatformPrefs( "data" );
	}

	public virtual string PrintValue()
	{
		return liveData.configureableType switch
		{
			ConfigurableTypes.Int => liveData.IntValue.ToString(),
			ConfigurableTypes.Float => liveData.FloatValue.ToString(),
			ConfigurableTypes.Boolean => liveData.BooleanValue.ToString(),
			ConfigurableTypes.String => liveData.StringValue,
			_ => string.Empty,
		};
	}

	public bool GetBooleanValue()
	{
		if ( !initialized )
		{
			Init();
		}
		return liveData.BooleanValue;
	}

	public int GetIntValue()
	{
		if ( !initialized )
		{
			Init();
		}
		return liveData.IntValue;
	}

	public float GetFloatValue()
	{
		if ( !initialized )
		{
			Init();
		}
		return liveData.FloatValue;
	}

	public string GetStringValue()
	{
		if ( !initialized )
		{
			Init();
		}
		return liveData.StringValue;
	}

	public ConfigurableTypes GetConfigurableType()
	{
		if ( !initialized )
		{
			Init();
		}
		return liveData.configureableType;
	}
}
