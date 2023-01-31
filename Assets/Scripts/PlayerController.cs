using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _groundDetection;
    [SerializeField] private Transform _wallDetection;

    [Header("Ground/Wall Check Values")]
    [SerializeField] private float _groundDetectionRadius;
    [SerializeField] private float _wallDetectionDistance;
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody2D _rigidbody;
    private Animator _anim;
    private SpriteRenderer _sprite;
    private int _jumpsLeft;

    //Checks
    private float _movementInputDirection;
    private bool _isFacingRight;

    private bool _isRunning;
    private bool _isGrounded;
    private bool _canJump;
    private bool _isWallSliding;

    [Header("Movement")]
    public float MoveSpeed = 10;
    public float JumpForce = 15;
    public float WallSlideSpeed = 5;
    public int NumJumps = 2;
    public float MovementForceInAir;
    public float AirDragMultipler;
    public float VariableJumpHeightMultiplier = 0.5f;



    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        _isFacingRight = true;
    }

    private void Update()
    {
        CheckInput();
        CheckMovementDirection();        
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void CheckMovementDirection()
    {
        CheckIfRunning();

        if (_isFacingRight && _movementInputDirection < 0)
            FlipSprite(true);
        else if(!_isFacingRight && _movementInputDirection > 0)
            FlipSprite(false);       
    }

    private void FlipSprite(bool shouldFaceLeft)
    {
        _isFacingRight = !_isFacingRight;
        _sprite.flipX = shouldFaceLeft;
    }

    private void CheckInput()
    {
        _movementInputDirection = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if(Input.GetButtonUp("Jump"))
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * VariableJumpHeightMultiplier);
        }
    }

    private bool CheckIfGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundDetection.position, _groundDetectionRadius, _groundLayer);
        return _isGrounded;
    }

    private bool CheckIfRunning()
    {
        _isRunning = _rigidbody.velocity.x != 0 ? true : false;
        return _isRunning;
    }

    private bool CheckIfCanJump()
    {
        if(CheckIfGrounded() && _rigidbody.velocity.y <=0)
        {
            _jumpsLeft = NumJumps;
        }

        if (_jumpsLeft < 1) return false;
        else return true;
    }

    private bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(_wallDetection.position, transform.right, _wallDetectionDistance, _groundLayer);
    }

    private bool CheckIfWallSliding()
    {
        _isWallSliding = CheckIfTouchingWall() && !CheckIfGrounded() && _rigidbody.velocity.y < 0;
        return _isWallSliding;
    }

    private void Jump()
    {
        if(CheckIfCanJump() && NumJumps > 0)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, JumpForce);
            _jumpsLeft--;
        }
           
    }

    private void Move()
    {
        if (!CheckIfGrounded() && !CheckIfWallSliding() && _movementInputDirection == 0)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x * AirDragMultipler, _rigidbody.velocity.y);
        }
        else
        {
            _rigidbody.velocity = new Vector2(MoveSpeed * _movementInputDirection, _rigidbody.velocity.y);
        }       

        if(CheckIfWallSliding())
        {
            if(_rigidbody.velocity.y < -WallSlideSpeed)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -WallSlideSpeed);
            }
        }
    }

    private void UpdateAnimations()
    {
        _anim.SetBool("IsGrounded", CheckIfGrounded());
        _anim.SetBool("IsRunning", _isRunning);    
        _anim.SetFloat("VelocityY", _rigidbody.velocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundDetection.position, _groundDetectionRadius);
        Gizmos.DrawLine(_wallDetection.position, new Vector3(_wallDetection.position.x + _wallDetectionDistance, _wallDetection.position.y, _wallDetection.position.z));
    }

}
