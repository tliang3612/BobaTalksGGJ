using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField] private AudioClip _musicClip;
    [SerializeField] private AudioSource _audioSource;

    public bool ShouldLoop = true;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();    
    }

    private void Start()
    {
        FindObjectOfType<AudioManager>().PlaySound(_musicClip, _audioSource, TrackType.Music, ShouldLoop);
    }
}
