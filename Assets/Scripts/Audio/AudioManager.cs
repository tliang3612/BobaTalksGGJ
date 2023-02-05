using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrackType
{
	Master,
    Music,
    Sfx
}

public class AudioData
{
	public AudioData(AudioClip clip, AudioSource source, AudioManager manager, bool shouldLoop)
    {
		AudioClip = clip;
		AudioSource = source;
		AudioManager = manager;
		ShouldLoop = shouldLoop;
    }

	public AudioClip AudioClip;
	public AudioSource AudioSource;
	public AudioManager AudioManager;
	public bool ShouldLoop;
}

public class AudioManager : MonoBehaviour
{  
	public AudioSettings AudioSetting;

    private void Start()
    {
		AudioSetting.SetTrackVolume(TrackType.Master, AudioSetting.MasterVolume);
		AudioSetting.SetTrackVolume(TrackType.Sfx, AudioSetting.GetTrackVolume(TrackType.Sfx));
		AudioSetting.SetTrackVolume(TrackType.Music, AudioSetting.GetTrackVolume(TrackType.Music));
	}

    public void PlaySound(AudioClip audioClip, AudioSource audioSource, TrackType trackType, bool shouldLoop)
    {
		audioSource.clip = audioClip;

		if (audioSource.isPlaying && shouldLoop)
			return;

		switch (trackType)
		{
			case TrackType.Music:
				audioSource.outputAudioMixerGroup = AudioSetting.MusicAudioMixerGroup;
				audioSource.Play();
				break;
			case TrackType.Sfx:
				audioSource.outputAudioMixerGroup = AudioSetting.SfxAudioMixerGroup;
				audioSource.PlayOneShot(audioClip);
				break;
		}

		audioSource.loop = shouldLoop;

	}

	public void PauseSound(AudioSource source)
	{
		source.Pause();
	}

	public void ResumeSound(AudioSource source)
	{
		source.Play();
	}

	public void StopSound(AudioSource source)
	{
		source.Stop();
	}

}
