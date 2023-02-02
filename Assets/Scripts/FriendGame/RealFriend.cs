using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealFriend : MonoBehaviour, IContextable, IInteractable
{   
    [SerializeField] private Sprite _contextToShow;
    [SerializeField] private GameObject _contextBox;
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _distanceOffset;

    private Rigidbody2D _rb;
    private bool _isFollowingPlayer;
    private Transform _playerTransform;

    public Sprite ContextSprite
    {
        get { return _contextToShow; }        
    }

    private void Awake()
    {
        _contextBox.GetComponentInChildren<SpriteRenderer>().sprite = ContextSprite;
        _contextBox.SetActive(false);
        _rb = GetComponent<Rigidbody2D>();
        _isFollowingPlayer = false;
    }

    private void Start()
    {
        _playerTransform= FindObjectOfType<Player>().transform;
    }

    private void Update()
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
        if(Vector2.Distance(transform.position, _playerTransform.position) >= _distanceOffset)
        {
            float followDirection = (_playerTransform.position.x - transform.position.x) > 0 ? 1 : -1;
            _rb.velocity = new Vector2(followDirection * _followSpeed, 0);
            
        }       
        else
        {
            _rb.velocity = Vector2.zero;
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
