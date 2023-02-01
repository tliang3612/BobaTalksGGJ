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
	public AudioData(AudioClip clip, AudioSource source, AudioManager manager)
    {
		AudioClip = clip;
		AudioSource = source;
		AudioManager = manager;
    }

	public AudioClip AudioClip;
	public AudioSource AudioSource;
	public AudioManager AudioManager;
}

public class AudioManager : MonoBehaviour
{  
	public AudioSettings AudioSetting;

    private void Start()
    {
		AudioSetting.SetTrackVolume(TrackType.Master, AudioSetting.MasterVolume);
		AudioSetting.SetTrackVolume(TrackType.Sfx, AudioSetting.SfxVolume);
		AudioSetting.SetTrackVolume(TrackType.Music, AudioSetting.MusicVolume);
	}

    public void PlaySound(AudioClip audioClip, AudioSource audioSource, TrackType trackType, bool shouldLoop)
    {
		audioSource.clip = audioClip;
		
		switch (trackType)
		{
			case TrackType.Music:
				if (audioSource.isPlaying)
					return;

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
