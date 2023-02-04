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

    [SerializeField] private Image _volumeHandle;
    [SerializeField] private Sprite _mutedSprite;
    [SerializeField] private Sprite _unMutedSprite;

    private void Start()
    {
        if (_volumeSlider.value <= 0)
            _volumeHandle.sprite = _mutedSprite;
        else

        _volumeHandle.sprite = _unMutedSprite;
        _volumeSlider.maxValue = _audioSettings.MaxVolume;
        _text.text = ((int)(_volumeSlider.value * 20)).ToString();
        _volumeSlider.value = (float)_audioSettings.GetTrackVolume(_trackType);
    }

    public void OnVolumeChanged()
    {
        _text.text = ((int)(_volumeSlider.value * 20)).ToString();
        _audioSettings.SetTrackVolume(_trackType, _volumeSlider.value);
        if(_volumeSlider.value <= 0)
            _volumeHandle.sprite = _mutedSprite;
        else
            _volumeHandle.sprite = _unMutedSprite;
    }



}
