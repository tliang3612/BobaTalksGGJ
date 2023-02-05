using System;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
[CreateAssetMenu(menuName = "Audio/AudioSettings")]
public class AudioSettings : ScriptableObject
{
    public string MasterVolumeName = "MasterVolume";
    public string MusicVolumeName = "MusicVolume";
    public string SfxVolumeName = "SfxVolume";

    public AudioMixer TargetAudioMixer;

    public AudioMixerGroup MasterAudioMixerGroup;
    public AudioMixerGroup MusicAudioMixerGroup;
    public AudioMixerGroup SfxAudioMixerGroup;

    public float MixerValuesMultiplier = 20;
    public float MaxVolume = 5f;


    [Range(0, 5)]
    public float MasterVolume = 1;
    public bool MasterOn = true;

    [Range(0, 5)]
    public float MusicVolume = 1;
    public bool MusicOn = true;

    [Range(0, 5)]
    public float SfxVolume = 1;
    public bool SfxOn = true;

    public void SetTrackVolume(TrackType trackType, float volume)
    {
        if (volume <= 0f)
        {
            volume = 0.000001f;
        }

        switch (trackType)
        {
            case TrackType.Master:
                TargetAudioMixer.SetFloat(MasterVolumeName, NormalizedToMixerVolume(volume));
                MasterVolume = volume;
                break;
            case TrackType.Music:
                TargetAudioMixer.SetFloat(MusicVolumeName, NormalizedToMixerVolume(volume));
                MusicVolume = volume;
                PlayerPrefs.SetFloat("Music", volume);
                break;
            case TrackType.Sfx:
                TargetAudioMixer.SetFloat(SfxVolumeName, NormalizedToMixerVolume(volume));
                SfxVolume = volume;
                PlayerPrefs.SetFloat("Sfx", volume);
                break;
        }
    }

    public float GetTrackVolume(TrackType track)
    {
        switch (track)
        {
            case TrackType.Music:
                return PlayerPrefs.GetFloat("Music");
            case TrackType.Sfx:
                return PlayerPrefs.GetFloat("Sfx");
        }

        return 1f;
    }

    public float NormalizedToMixerVolume(float normalizedVolume)
    {
        return Mathf.Log10(normalizedVolume) * MixerValuesMultiplier;
    }

    public float MixerVolumeToNormalized(float mixerVolume)
    {
        return (float)Math.Pow(10, (mixerVolume / MixerValuesMultiplier));
    }

}
