using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private AudioSettings _audioSettings;
    [SerializeField] private TrackType _trackType;

    private void Start()
    {
        _volumeSlider.maxValue = _audioSettings.MaxVolume;
        _text.text = ((int)(_volumeSlider.value * 20)).ToString();
        _volumeSlider.value = (float)_audioSettings.GetTrackVolume(_trackType);
    }

    public void OnVolumeChanged()
    {
        _text.text = ((int)(_volumeSlider.value * 20)).ToString();
        _audioSettings.SetTrackVolume(_trackType, _volumeSlider.value);
    }



}
