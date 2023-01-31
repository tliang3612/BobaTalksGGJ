using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //State Machine
    public PlayerStateMachine StateMachine { get; private set; } 

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
    public PlayerInputController InputController { get; private set; }
    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public SpriteRenderer PlayerSprite { get; private set; }

    [SerializeField] private PlayerData _playerData;
    #endregion

    #region Components
    [SerializeField] private Transform _groundDetector;
    [SerializeField] private Transform _wallDetector;

    #endregion

    #region MovementVariables
    public Vector2 CurrentVelocity { get; private set; }
    public bool IsFacingRight { get; private set; }
    public int FacingDirection
    {
        get { return IsFacingRight ? 1 : -1; }
        private set { }
    }
    #endregion  

    #region Unity Callback Functions
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        PlayerSprite = GetComponent<SpriteRenderer>();
        InputController = GetComponent<PlayerInputController>();
        Anim = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();

        IdleState = new PlayerIdleState(this, StateMachine, _playerData, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, _playerData, "Move");
        JumpState = new PlayerJumpState(this, StateMachine, _playerData, "Airborne"); //JumpState would just call airborne animation
        AirborneState = new PlayerAirborneState(this, StateMachine, _playerData, "Airborne");
        LandState = new PlayerLandState(this, StateMachine, _playerData, "Land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, _playerData, "WallSlide");
        DashState = new PlayerDashState(this, StateMachine, _playerData, "Dash");
        InteractState = new PlayerInteractState(this, StateMachine, _playerData, "Idle");

        FindObjectOfType<DialogueManager>().PowerupReceivedEvent += OnPowerupReceived;
    }
    private void Start()
    {
        IsFacingRight = true;
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        CurrentVelocity = Rb.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Getters

    #endregion

    #region Setters
    public void SetVelocityX(float velocityX)
    {
        Vector2 newVelocity = new Vector2();

        newVelocity.Set(velocityX, CurrentVelocity.y);
        Rb.velocity = newVelocity;
        CurrentVelocity = newVelocity;
    }

    public void SetVelocityY(float velocityY)
    {
        Vector2 newVelocity = new Vector2();

        newVelocity.Set(CurrentVelocity.x, velocityY);
        Rb.velocity = newVelocity;
        CurrentVelocity = newVelocity;
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
        Gizmos.DrawRay(_wallDetector.position, Vector2.right * _playerData.WallDetectionDistance);
    }
}
