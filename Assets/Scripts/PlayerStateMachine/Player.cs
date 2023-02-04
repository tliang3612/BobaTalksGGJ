using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    //State Machine
    public PlayerStateMachine StateMachine { get; private set; }

    private readonly int _maxPlayerHealth = 3;
    private int _currentPlayerHealth;
    private bool _isCurrentlyHurt;

    #region PlayerState Variables  
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirborneState AirborneState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    //public PlayerDashState DashState { get; private set; }
    public PlayerInteractState InteractState { get; private set; }
    public PlayerHurtState HurtState { get; private set; }
    public PlayerSlideState SlideState { get; private set; }
    #endregion

    #region Reference Variables
    [SerializeField] private PlayerData _playerData;
    private AudioSource _audioSource;
    private AudioManager _audioManager;

    public PlayerInputController InputController { get; private set; }
    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public SpriteRenderer PlayerSprite { get; private set; }
    public Collider2D PlayerCollider { get; private set; }
    public bool CanInteractWithCollideables { get; set; }
 
    #endregion

    #region Components
    [SerializeField] private Transform _groundDetector;
    [SerializeField] private Transform _wallDetector;
    [SerializeField] private Transform _slopeDetector;
    #endregion

    #region MovementVariables
    public Vector2 CurrentVelocity
    {
        get { return Rb.velocity; }
        private set { }
    }

    public bool IsFacingRight { get; private set; }
    public int FacingDirection
    {
        get { return IsFacingRight ? 1 : -1; }
        private set { }
    }
    #endregion

    #region Events
    public Action<Player> PlayerDeathEvent;
    public Action<int> PlayerHealthChangedEvent;

    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        _audioSource = GetComponent<AudioSource>();
        _audioManager = FindObjectOfType<AudioManager>();

        PlayerSprite = GetComponent<SpriteRenderer>();
        PlayerCollider = GetComponent<Collider2D>();
        InputController = GetComponent<PlayerInputController>();
        Anim = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();

        InitializeStates();
    }
    private void Start()
    {
        PlayerSprite.color = Color.white;
        IsFacingRight = true;
        StateMachine.Initialize(IdleState);
        _currentPlayerHealth = _maxPlayerHealth;
        CanInteractWithCollideables = true;
    }

    private void InitializeStates()
    {
        IdleState = new PlayerIdleState(this, StateMachine, _playerData, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, _playerData, "Move");
        JumpState = new PlayerJumpState(this, StateMachine, _playerData, "Airborne", new AudioData(_playerData.JumpAudio, _audioSource, _audioManager, false)); //JumpState would just call airborne animation
        AirborneState = new PlayerAirborneState(this, StateMachine, _playerData, "Airborne");
        LandState = new PlayerLandState(this, StateMachine, _playerData, "Land", new AudioData(_playerData.LandAudio, _audioSource, _audioManager, false));
        WallSlideState = new PlayerWallSlideState(this, StateMachine, _playerData, "WallSlide");
        //DashState = new PlayerDashState(this, StateMachine, _playerData, "Dash");
        InteractState = new PlayerInteractState(this, StateMachine, _playerData, "Idle");
        HurtState = new PlayerHurtState(this, StateMachine, _playerData, "Hurt", new AudioData(_playerData.HurtAudio, _audioSource, _audioManager, false));
        SlideState = new PlayerSlideState(this, StateMachine, _playerData, "Land", new AudioData(_playerData.LandAudio, _audioSource, _audioManager, false));
    }

    private void Update()
    {
        CurrentVelocity = Rb.velocity;
        if (StateMachine != null)
            StateMachine.CurrentState.StateUpdate();
    }

    #endregion

    #region Getters

    #endregion

    #region Setters
    public void SetVelocityX(float velocityX)
    {
        Rb.velocity = new Vector2(velocityX, Rb.velocity.y);
    }

    public void SetVelocityY(float velocityY)
    {
        Rb.velocity = new Vector2(Rb.velocity.x, velocityY);
    }

    public void SetVelocityToZero()
    {
        Rb.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }

    public void AddVelocity(float velocity, Vector2 direction)
    {
        Vector2 newVelocity = direction.normalized * velocity;
        Rb.velocity += newVelocity;
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    public void FadePlayerOut()
    {
        PlayerSprite.DOFade(0, _playerData.FadeDuration);
    }

    #endregion

    #region Checks

    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(_groundDetector.position, _playerData.GroundDetectionRadius, _playerData.GroundLayer);
    }

    public bool CheckIfRising()
    {
        return CurrentVelocity.y > 0.01f;
    }

    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(_wallDetector.position, Vector2.right * FacingDirection, _playerData.WallDetectionDistance, _playerData.GroundLayer);
    }

    public IJumpable DetectJumpable()
    {
        var hit = Physics2D.OverlapCircle(_groundDetector.position, _playerData.GroundDetectionRadius, _playerData.ProjectileLayer);
        if(hit && hit.GetComponent<IJumpable>() != null)
        {
            return hit.GetComponent<IJumpable>();
        }

        return null;       
    }

    public bool CheckIfTouchingSlope()
    {
        if (CheckIfGrounded() || _slopeDetector == null)
            return false;

        var hitVertical = Physics2D.Raycast(_slopeDetector.position, Vector2.down, _playerData.SlopeDetectionDistance, _playerData.GroundLayer);
        var hitHoriztonal = Physics2D.Raycast(_slopeDetector.position, Vector2.right, _playerData.SlopeDetectionDistance, _playerData.GroundLayer);

        if (hitVertical && hitHoriztonal)
            return true;

        return false;
    }

    public bool CheckIfHurt()
    {
        return _isCurrentlyHurt;
    }

    /// <summary>
    /// Returns null if there is no interactable
    /// </summary>
    /// <returns></returns>
    public IInteractable DetectInteractable()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, _playerData.InteractDetectionRadius, _playerData.InteractLayer);

        if (colliders != null)
        {
            foreach(var collider in colliders)
            {
                if(!collider.GetComponent<IInteractable>().CheckInteractFinished())
                {
                    return collider.GetComponent<IInteractable>();
                }
            }            
        }

        return null;
    }
    #endregion

    #region Actions
    public void HandleFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    private void Flip()
    {
        IsFacingRight = !IsFacingRight;
        PlayerSprite.flipX = !IsFacingRight;
    }   

    public void TakeDamage(int directionX)
    {
        _currentPlayerHealth--;
        PlayerHealthChangedEvent?.Invoke(_currentPlayerHealth);

        if (_currentPlayerHealth <= 0)
            Die();
        else
            StartCoroutine(TakeDamageCoroutine(directionX));
    }

    public IEnumerator TakeDamageCoroutine(int directionX)
    {
        Rb.velocity = new Vector2(directionX * _playerData.KnockbackVelocityX, _playerData.KnockbackVelocityY);
        HandleDamaged(true);

        yield return new WaitForSeconds(_playerData.HurtDuration);

        HandleDamaged(false);
    }

    private void HandleDamaged(bool isDamaged)
    {
        CanInteractWithCollideables = !isDamaged;
        _isCurrentlyHurt = isDamaged;
        PlayerSprite.color = isDamaged ? Color.gray : Color.white;
    }

    public void IncreaseHealth()
    {
        _currentPlayerHealth++;
        Mathf.Clamp(_currentPlayerHealth, 0, 3);
        PlayerHealthChangedEvent?.Invoke(_currentPlayerHealth);
    }

    public void Die()
    {
        StateMachine.CurrentState.OnStateExit();
        SetVelocityToZero();
        StateMachine = null;
        CanInteractWithCollideables = false;
        _audioManager.PlaySound(_playerData.HurtAudio, _audioSource, TrackType.Sfx, true);
        PlayerDeathEvent?.Invoke(this);
    }

    public IEnumerator PlayDeathAnimation()
    {
        FadePlayerOut();       
        yield return new WaitForSeconds(_playerData.FadeDuration);
    }

    #endregion

    #region Callbacks
/*    private void OnPowerupReceived(PowerupType powerup)
    {
        PowerupInventory.AddPowerup(powerup);
    }*/

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundDetector.position, _playerData.GroundDetectionRadius);
        Gizmos.DrawWireSphere(transform.position, _playerData.InteractDetectionRadius);
        Gizmos.DrawRay(_wallDetector.position, Vector2.right * _playerData.WallDetectionDistance);

        if (_slopeDetector == null) return;
             
        Gizmos.DrawRay(_slopeDetector.position, Vector2.down * _playerData.SlopeDetectionDistance);
        Gizmos.DrawRay(_slopeDetector.position, Vector2.right * _playerData.SlopeDetectionDistance);
    }
}
