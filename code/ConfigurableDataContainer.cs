using System;
using System.Collections.Generic;
using Sandbox;

[Serializable]
public class ConfigurableDataContainer
{
	public static Action OnSaveValues;

	public static Action OnResetValues;

	public List<LiveData> saveDataCache = new List<LiveData>();

	public static ConfigurableDataContainer LoadContainer( string fileName )
	{
		if ( PlatformPlayerPrefs.HasKey( fileName ) )
		{
			return Json.Deserialize<ConfigurableDataContainer>( PlatformPlayerPrefs.GetString( fileName ) );
		}
		return new ConfigurableDataContainer();
	}

	public LiveData GetSavedDataFromContainer( string key )
	{
		for ( int i = 0; i < saveDataCache.Count; i++ )
		{
			if ( saveDataCache[i].key.Equals( key ) )
			{
				return saveDataCache[i];
			}
		}
		return null;
	}

	public void UpdateSaveDataCache( LiveData data )
	{
		bool flag = true;
		for ( int i = 0; i < saveDataCache.Count; i++ )
		{
			if ( data.key.Equals( saveDataCache[i].key ) )
			{
				saveDataCache[i] = data;
				flag = false;
				break;
			}
		}
		if ( flag )
		{
			saveDataCache.Add( data );
		}
	}

	public void DeleteEntry( string key )
	{
		for ( int i = 0; i < saveDataCache.Count; i++ )
		{
			if ( saveDataCache[i].key.Equals( key ) )
			{
				saveDataCache.RemoveAt( i );
			}
		}
		SaveToPlatformPrefs( "data", forceSave: true );
	}

	public void SaveToPlatformPrefs( string fileName, bool forceSave = false )
	{
		if ( OnSaveValues != null )
		{
			OnSaveValues();
		}
		if ( forceSave || Configurable.Dirty )
		{
			string value = Json.Serialize( this );
			PlatformPlayerPrefs.SetString( fileName, value );
			PlatformPlayerPrefs.Save();
			Configurable.Dirty = false;
		}
	}
}
