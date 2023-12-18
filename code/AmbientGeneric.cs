using Sandbox;
using System;
using System.Collections;
using System.Diagnostics;

public class AmbientGeneric : HammerEntity
{
	// private AudioSource source;

	[Property]
	public float volume { get; set; } = 1f;

	[Property]
	public MinMaxAttribute pitchRange { get; set; } = new MinMaxAttribute( 1f, 1f );

	[Property]
	public SoundEvent[] clips { get; set; }

	[Property]
	public string sourceEntityName { get; set; } = "";

	[Property]
	public GameObject sourceEntity { get; set; }

	protected override void OnAwake()
	{
		/*
		source = GetComponent<AudioSource>();
		if ( source == null )
		{
			Debug.LogWarning( "ambient_generic " + base.name + " didn't have an audio source", base.gameObject );
			source = sourceEntity.AddComponent<AudioSource>();
		}
		if ( sourceEntity != base.gameObject )
		{
			AudioSource audioSource = source;
			source = sourceEntity.AddComponent<AudioSource>();
			source.clip = audioSource.clip;
			source.loop = audioSource.loop;
			source.volume = audioSource.volume;
			source.spatialBlend = audioSource.spatialBlend;
			source.minDistance = audioSource.minDistance;
			source.maxDistance = audioSource.maxDistance;
			source.playOnAwake = audioSource.playOnAwake;
			source.pitch = audioSource.pitch;
			source.reverbZoneMix = audioSource.reverbZoneMix;
			source.bypassReverbZones = audioSource.bypassReverbZones;
			source.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
			Object.Destroy( audioSource );
		}
		if ( clips.Length != 0 )
		{
			source.pitch = pitchRange.Random();
			source.volume = volume;
			source.clip = clips[Random.Range( 0, clips.Length )];
			if ( source.playOnAwake )
			{
				source.Play();
			}
		}
		GameMaster.OnPause += Pause;
		GameMaster.OnResume += Resume;
		*/
	}

	protected override void OnDestroy()
	{
		// GameMaster.OnPause -= Pause;
		// GameMaster.OnResume -= Resume;
	}

	private void Pause()
	{
		/*
		if ( source != null )
		{
			source.Pause();
		}
		*/
	}

	private void Resume()
	{
		/*
		if ( source != null )
		{
			source.UnPause();
		}
		*/
	}

	private void OnValidate()
	{
		if ( sourceEntity == null || sourceEntity.Name != sourceEntityName || sourceEntity != base.GameObject )
		{
			/*
			GameObject gameObject = GameObject.Find( sourceEntityName );
			if ( gameObject != null )
			{
				sourceEntity = gameObject;
			}
			else
			{
				sourceEntity = base.GameObject;
			}
			*/
		}
	}

	public void Input_PlaySound()
	{
		/*
		if ( !(source == null) )
		{
			source.clip = clips[Random.Range( 0, clips.Length )];
			source.Play();
		}
		*/
	}

	public void Input_StopSound()
	{
		/*
		if ( !(source == null) )
		{
			source.Stop();
		}
		*/
	}

	public void Input_FadeOut( float duration )
	{
		/*
		if ( !(source == null) )
		{
			StartCoroutine( Fade( -1f, 0f, duration ) );
		}
		*/
	}

	public void Input_FadeIn( float duration )
	{
		/*
		if ( !(source == null) )
		{
			StartCoroutine( Fade( 0f, -1f, duration ) );
		}
		*/
	}

	/*
	private IEnumerator Fade( float startVol, float endVol, float duration )
	{
		float startTime = Singleton<GameMaster>.Instance.GameTime;
		float endTime = startTime + duration;
		if ( startVol < 0f )
		{
			startVol = volume;
		}
		if ( endVol < 0f )
		{
			endVol = volume;
		}
		while ( Singleton<GameMaster>.Instance.GameTime < endTime )
		{
			float t = Mathf.InverseLerp( startTime, endTime, Singleton<GameMaster>.Instance.GameTime );
			if ( source == null )
			{
				yield break;
			}
			source.volume = Mathf.Lerp( startVol, endVol, t );
			yield return new WaitForEndOfFrame();
		}
		if ( !(source == null) )
		{
			source.volume = endVol;
			if ( source.volume == 0f )
			{
				source.Stop();
				source.volume = startVol;
			}
		}
	}
	*/
}
