using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    private Image _image;
    public float FadeDuration;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void FadeIn()
    {
        _image.DOFade(1, FadeDuration);
    }

    public void FadeOut()
    {
        _image.DOFade(0, FadeDuration);
    }
}
