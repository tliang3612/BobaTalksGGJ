using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField] private AudioClip _musicClip;
    private AudioSource _audioSource;
    private AudioManager _audioManager;

    public bool ShouldLoop = true;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();    
        _audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        if(_musicClip)
            _audioManager.PlaySound(_musicClip, _audioSource, TrackType.Music, ShouldLoop);
    }

    public void StopMusic()
    {
        _audioManager.StopSound(_audioSource);
    }
}
