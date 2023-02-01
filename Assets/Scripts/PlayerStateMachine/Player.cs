using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    //State Machine
    public PlayerStateMachine StateMachine { get; private set; }

    private int _maxPlayerHealth = 3;
    private int _currentPlayerHealth;

    #region PlayerState Variables  
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirborneState AirborneState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerInteractState InteractState { get; private set; }
    #endregion

    #region Reference Variables
    [SerializeField] private PlayerData _playerData;

    public PlayerInputController InputController { get; private set; }
    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public SpriteRenderer PlayerSprite { get; private set; }
    public Collider2D PlayerCollider { get; private set; }
    public bool CanInteractWithCollideables { get; set; }
    
    private AudioSource _audioSource;
    private AudioManager _audioManager;
    #endregion

    #region Components
    [SerializeField] private Transform _groundDetector;
    [SerializeField] private Transform _wallDetector;
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

        if(FindObjectOfType<DialogueManager>())
            FindObjectOfType<DialogueManager>().PowerupReceivedEvent += OnPowerupReceived;
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
        JumpState = new PlayerJumpState(this, StateMachine, _playerData, "Airborne", new AudioData(_playerData.JumpAudio, _audioSource, _audioManager)); //JumpState would just call airborne animation
        AirborneState = new PlayerAirborneState(this, StateMachine, _playerData, "Airborne");
        LandState = new PlayerLandState(this, StateMachine, _playerData, "Land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, _playerData, "WallSlide");
        DashState = new PlayerDashState(this, StateMachine, _playerData, "Dash");
        InteractState = new PlayerInteractState(this, StateMachine, _playerData, "Idle");
    }

    private void Update()
    {
        CurrentVelocity = Rb.velocity;
        if (StateMachine != null)
            StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        if(StateMachine != null)
            StateMachine.CurrentState.PhysicsUpdate();
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

    public void SetVelocity(float velocity, Vector2 direction)
    {
        Vector2 newVelocity = direction.normalized * velocity;
        Rb.velocity += newVelocity;
        CurrentVelocity += newVelocity;
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

    public bool CheckIfOnJumpable()
    {
        var hit = Physics2D.OverlapCircle(_groundDetector.position, _playerData.GroundDetectionRadius, _playerData.ProjectileLayer);
        if(hit && hit.GetComponent<IJumpable>() != null)
        {
            return true;
        }

        return false;
        
    }

    /// <summary>
    /// Returns null if there is no interactable
    /// </summary>
    /// <returns></returns>
    public IInteractable DetectInteractable()
    {
        var collider = Physics2D.OverlapCircle(transform.position, _playerData.InteractDetectionRadius, _playerData.InteractLayer);

        if (collider != null)
        {
            return collider.GetComponent<IInteractable>();
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

    public void TakeDamage()
    {
        _currentPlayerHealth--;
        PlayerHealthChangedEvent?.Invoke(_currentPlayerHealth);

        if (_currentPlayerHealth <= 0)
            Die();
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
        PlayerDeathEvent?.Invoke(this);
    }

    public IEnumerator PlayDeathAnimation()
    {
        FadePlayerOut();
        
        yield return new WaitForSeconds(_playerData.FadeDuration);
    }

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
    private void AnimationFinishedTrigger() => StateMachine.CurrentState.AnimationFinishedTrigger();


    #endregion

    #region Callbacks
    private void OnPowerupReceived(PowerupType powerup)
    {
        PowerupInventory.AddPowerup(powerup);
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundDetector.position, _playerData.GroundDetectionRadius);
        Gizmos.DrawWireSphere(transform.position, _playerData.InteractDetectionRadius);
        Gizmos.DrawRay(_wallDetector.position, Vector2.right * FacingDirection * _playerData.WallDetectionDistance);
    }
}
