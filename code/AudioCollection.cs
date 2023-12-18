using System;
using System.Collections.Generic;
using Sandbox;

[GameResource( "Audio Collection", null, null, Category = "Data" )]
public class AudioCollection : GameResource
{
	[Property]
	[Range( 0f, 2f )]
	private float masterVolume { get; set; } = 1f;

	[Property]
	private int index { get; set; }

	[Property]
	[Range( 0f, 3f )]
	private float averageDuration { get; set; } = 0.25f;

	[Property]
	private bool usePlaylistSorting { get; set; }

	[Property]
	private bool sequentialPlaylist { get; set; }

	[Property]
	public List<AudioEntry> AudioEntries { get; set; }

	[NonSerialized]
	private AudioEntry[] entryPlaylist;

	[NonSerialized]
	private int playlistLength;

	[NonSerialized]
	private int playlistIndex;

	[NonSerialized]
	private bool setup;

	public const bool SHOW_DEBUG = false;

	[Property]
	public float AverageDuration => averageDuration;

	public int GetIndex()
	{
		return index;
	}

	/*
	public bool SetVolumeAndPitchAndPlayClip(AudioSource audioSource)
	{
		if (GetRandomEntry(out var entry) && audioSource != null && entry.GetClip(out var clip))
		{
			audioSource.clip = clip;
			audioSource.volume = entry.GetVolume() * masterVolume;
			audioSource.pitch = entry.GetPitch();
			if (audioSource.gameObject.activeInHierarchy)
			{
				audioSource.Play();
			}
			return true;
		}
		return false;
	}
	*/

	/*
	private bool GetRandomEntry(out AudioEntry entry)
	{
		if (usePlaylistSorting && AudioEntries.Count > 0)
		{
			if (!setup)
			{
				UpdatePlaylist();
				entry = GetNextEntryFromPlaylist();
				setup = true;
				return true;
			}
			entry = GetNextEntryFromPlaylist();
			return true;
		}
		if (AudioEntries.Count > 0)
		{
			entry = AudioEntries[UnityEngine.Random.Range(0, AudioEntries.Count)];
			return true;
		}
		entry = null;
		return false;
	}
	*/

	/*
	private void UpdatePlaylist()
	{
		entryPlaylist = new AudioEntry[AudioEntries.Count];
		Array.Copy(AudioEntries.ToArray(), 0, entryPlaylist, 0, AudioEntries.Count);
		if (!sequentialPlaylist)
		{
			new System.Random().Shuffle(entryPlaylist);
		}
		playlistIndex = 0;
		playlistLength = entryPlaylist.Length;
	}
	*/

	private AudioEntry GetNextEntryFromPlaylist()
	{
		if (playlistIndex >= playlistLength)
		{
			// UpdatePlaylist();
		}
		AudioEntry result = entryPlaylist[playlistIndex];
		playlistIndex++;
		return result;
	}
}
