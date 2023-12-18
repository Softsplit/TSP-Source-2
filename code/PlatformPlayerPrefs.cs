public static class PlatformPlayerPrefs
{
	private static IPlatformPlayerPrefs playerPrefs;

	public static void Init( IPlatformPlayerPrefs platformPlayerPrefs )
	{
		playerPrefs = platformPlayerPrefs;
	}

	public static void DeleteAll()
	{
		playerPrefs?.DeleteAll();
		playerPrefs?.Save();
	}

	public static void DeleteKey( string key )
	{
		playerPrefs?.DeleteKey( key );
		playerPrefs?.Save();
	}

	public static float GetFloat( string key )
	{
		return playerPrefs?.GetFloat( key ) ?? 0f;
	}

	public static float GetFloat( string key, float defaultValue )
	{
		return playerPrefs?.GetFloat( key, defaultValue ) ?? defaultValue;
	}

	public static int GetInt( string key )
	{
		return playerPrefs?.GetInt( key ) ?? 0;
	}

	public static int GetInt( string key, int defaultValue )
	{
		return playerPrefs?.GetInt( key, defaultValue ) ?? defaultValue;
	}

	public static string GetString( string key )
	{
		return playerPrefs?.GetString( key );
	}

	public static string GetString( string key, string defaultValue )
	{
		return playerPrefs?.GetString( key, defaultValue );
	}

	public static bool HasKey( string key )
	{
		return playerPrefs?.HasKey( key ) ?? false;
	}

	public static void Save()
	{
		playerPrefs?.Save();
	}

	public static void SetFloat( string key, float value )
	{
		playerPrefs?.SetFloat( key, value );
		playerPrefs?.Save();
	}

	public static void SetInt( string key, int value )
	{
		playerPrefs?.SetInt( key, value );
		playerPrefs?.Save();
	}

	public static void SetString( string key, string value )
	{
		playerPrefs?.SetString( key, value );
		playerPrefs?.Save();
	}
}
