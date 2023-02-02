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
    private Player _player;
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
        _player = FindObjectOfType<Player>();
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
        /*targetPosition = new Vector2(player.transform.position.x, player.transform.position.y + 1f);

        StartCoroutine(LerpPosition(targetPosition, .1f));
        if (shouldFlip)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }*/

        var deltaX = _playerTransform.position.x - transform.position.x;
        if (Mathf.Abs(deltaX) >= _distanceOffset)
        {
            float followDirection = deltaX > 0 ? 1 : -1;
            transform.position = Vector2.Lerp(transform.position, _playerTransform.position, Time.deltaTime * _followSpeed);        
        }       
        else
        {
            //_rb.velocity = Vector2.zero;
        }
    }

    public void HandleJump()
    {

    }

    /*private IEnumerator LerpPosition(Vector2 targetPosition, float duration)
    {
        float time = 0;
        Vector2 startPosition = transform.position;
        if (startPosition.x - targetPosition.x < 0)
        {
            shouldFlip = true;
        }
        else
        {
            shouldFlip = false;
        }

        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }*/


    public bool CheckInteractFinished()
    {
        return _isFollowingPlayer;
    }


}
