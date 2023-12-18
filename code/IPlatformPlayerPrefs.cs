public interface IPlatformPlayerPrefs
{
	void DeleteAll();

	void DeleteKey( string key );

	float GetFloat( string key );

	float GetFloat( string key, float defaultValue );

	int GetInt( string key );

	int GetInt( string key, int defaultValue );

	string GetString( string key );

	string GetString( string key, string defaultValue );

	bool HasKey( string key );

	void Save();

	void SetFloat( string key, float value );

	void SetInt( string key, int value );

	void SetString( string key, string value );
}
