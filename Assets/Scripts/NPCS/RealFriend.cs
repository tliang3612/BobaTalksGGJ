using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealFriend : MonoBehaviour, IContextable, IInteractable
{   
    [SerializeField] private Sprite _contextToShow;
    [SerializeField] private GameObject _contextBox;
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _distanceOffset;
    [SerializeField] private float _detectionRadius;
    [SerializeField] private LayerMask _playerLayer;

    private SpriteRenderer _sprite;
    private bool _isFollowingPlayer;
    private Transform _playerTransform;
    private Animator _anim;

    public Sprite ContextSprite
    {
        get { return _contextToShow; }        
    }

    private void Awake()
    {
        _contextBox.GetComponentInChildren<SpriteRenderer>().sprite = ContextSprite;
        _contextBox.SetActive(false);
        _isFollowingPlayer = false;
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _playerTransform = FindObjectOfType<Player>().transform;
    }

    private void FixedUpdate()
    {
        if (!_isFollowingPlayer)
            return;

        HandleMove();
    }

    private void Update()
    {
        HandlePlayerDetection();
    }

    public void ShowContext()
    {
        _contextBox.SetActive(true);
    }

    public void HideContext()
    {
        _contextBox.SetActive(false);
    }

    public void HandlePlayerDetection()
    {
        if (_isFollowingPlayer)
            return;

        var hit = Physics2D.OverlapCircle(transform.position, _detectionRadius, _playerLayer);
        if(hit)
        {
            ShowContext();
        }
        else
        {
            HideContext();
        }
    }

    public void HandleMove()
    {
        var targetPosition = new Vector2(_playerTransform.position.x, transform.position.y);
        float distance = transform.position.x - _playerTransform.position.x;

        if (Mathf.Abs(distance) > _distanceOffset)
        {
            Vector2 startPosition = transform.position;
            if (startPosition.x - targetPosition.x < 0)
                _sprite.flipX = true;
            else
                _sprite.flipX = false;

            transform.position = Vector2.Lerp(startPosition, targetPosition, _followSpeed * Time.deltaTime);
        }       
    }

    public void Interact()
    {
        Debug.Log("Interacted");
        _isFollowingPlayer = true;
        HideContext();
    }

    public bool CheckInteractFinished()
    {
        return _isFollowingPlayer;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }

}
