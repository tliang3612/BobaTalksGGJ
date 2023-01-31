using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
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

    [Range(0, 10)]
    public float MasterVolume = 1;
    public bool MasterOn = true;

    [Range(0, 10)]
    public float MusicVolume = 1;
    public bool MusicOn = true;

    [Range(0, 10)]
    public float SfxVolume = 1;
    public bool SfxOn = true;

    public void SetTrackVolume(TrackType trackType, float volume)
    {
        if (volume <= 0f)
        {
            volume = 0;
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
                break;
            case TrackType.Sfx:
                TargetAudioMixer.SetFloat(SfxVolumeName, NormalizedToMixerVolume(volume));
                SfxVolume = volume;
                break;
        }
    }

    public float GetTrackVolume(TrackType track)
    {
        float volume = 1f;
        switch (track)
        {
            case TrackType.Master:
                TargetAudioMixer.GetFloat(MasterVolumeName, out volume);
                break;
            case TrackType.Music:
                TargetAudioMixer.GetFloat(MusicVolumeName, out volume);
                break;
            case TrackType.Sfx:
                TargetAudioMixer.GetFloat(SfxVolumeName, out volume);
                break;
        }

        return MixerVolumeToNormalized(volume);
    }

    public virtual float NormalizedToMixerVolume(float normalizedVolume)
    {
        return Mathf.Log10(normalizedVolume) * MixerValuesMultiplier;
    }

    public virtual float MixerVolumeToNormalized(float mixerVolume)
    {
        return (float)Math.Pow(10, (mixerVolume / MixerValuesMultiplier));
    }

}
