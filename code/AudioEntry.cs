using System;
using Sandbox;

[Serializable]
public class AudioEntry
{
	[Property]
	private SoundEvent audioClip { get; set; }

	[Property]
	[Range( 0f, 1f )]
	private float volume { get; set; } = 1f;

	[Property]
	private float minimumPitch { get; set; } = 1f;

	[Property]
	private float maximumPitch { get; set; } = 1f;

	public AudioEntry( SoundEvent clip )
	{
		audioClip = clip;
	}

	public bool GetClip( out SoundEvent clip )
	{
		clip = audioClip;
		return audioClip != null;
	}

	public float GetPitch()
	{
		return Game.Random.Float( minimumPitch, maximumPitch );
	}

	public float GetVolume()
	{
		return volume;
	}
}
