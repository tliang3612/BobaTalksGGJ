using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialNpc : MonoBehaviour, IContextable
{
    [SerializeField] private Sprite _contextToShow;
    [SerializeField] private GameObject _contextBox;
    [SerializeField] private float _detectionRadius;
    [SerializeField] private LayerMask _playerLayer;

    private SpriteRenderer _sprite;
    private Transform _playerTransform;

    public Sprite ContextSprite
    {
        get { return _contextToShow; }
    }

    private void Awake()
    {
        _contextBox.GetComponentInChildren<SpriteRenderer>().sprite = ContextSprite;
        _contextBox.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _playerTransform = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        HandlePlayerDetection();
        if((_playerTransform.position.x - transform.position.x) < 0)
            _sprite.flipX = true;
        else
            _sprite.flipX = false;
    }

    public void ShowContext()
    {
        _contextBox.GetComponent<SpriteRenderer>().DOFade(1, 1);  
    }

    public void HideContext()
    {
        _contextBox.GetComponent<SpriteRenderer>().DOFade(0, .7f);
    }

    public void HandlePlayerDetection()
    {
        var hit = Physics2D.OverlapCircle(transform.position, _detectionRadius, _playerLayer);
        if (hit)
        {
            ShowContext();
        }
        else
        {
            HideContext();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}
