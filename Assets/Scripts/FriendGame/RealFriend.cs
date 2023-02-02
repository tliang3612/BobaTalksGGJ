using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealFriend : MonoBehaviour, IContextable, IInteractable
{   
    [SerializeField] private Sprite _contextToShow;
    [SerializeField] private GameObject _contextBox;
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _lerpDuration;
    [SerializeField] private float _distanceOffset;

    private SpriteRenderer _spriteRenderer;
    private bool _isFollowingPlayer;
    private Player _player;
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
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        _playerTransform= FindObjectOfType<Player>().transform;
        _player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        if (!_isFollowingPlayer)
            return;

        HandleMove();
    }

    public void ShowContext()
    {
        _contextBox.SetActive(true);
    }

    public void HideContext()
    {
        _contextBox.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null && !_isFollowingPlayer)
        {
            ShowContext();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !_isFollowingPlayer)
        {
            HideContext();
        }
    }

    public void Interact()
    {
        _isFollowingPlayer = true;
        HideContext();
    }

    public void HandleMove()
    {
        var targetPosition = new Vector2(_playerTransform.position.x, transform.position.y);
        float distance = transform.position.x - _playerTransform.position.x;

        if (Mathf.Abs(distance) > _distanceOffset)
        {
            Vector2 startPosition = transform.position;
            if (startPosition.x - targetPosition.x < 0)
                _spriteRenderer.flipX = false;
            else
                _spriteRenderer.flipX = true;

            transform.position = Vector2.Lerp(startPosition, targetPosition, _followSpeed * Time.deltaTime);
        }       
    }

    public void HandleJump()
    {

    }

    public bool CheckInteractFinished()
    {
        return _isFollowingPlayer;
    }


}
