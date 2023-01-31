using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrackType
{
	Master,
    Music,
    Sfx
}

public class AudioManager : MonoBehaviour
{  
	public AudioSettings AudioSettings;

	public void PlaySound(AudioClip audioClip, AudioSource audioSource, TrackType trackType, bool shouldLoop)
    {
		audioSource.clip = audioClip;

		switch (trackType)
		{
			case TrackType.Music:
				audioSource.outputAudioMixerGroup = AudioSettings.MusicAudioMixerGroup;
				audioSource.Play();
				break;
			case TrackType.Sfx:
				audioSource.outputAudioMixerGroup = AudioSettings.SfxAudioMixerGroup;
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
