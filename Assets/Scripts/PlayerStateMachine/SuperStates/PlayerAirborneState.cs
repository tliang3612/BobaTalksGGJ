using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneState : PlayerState
{
    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _isRising;
    private bool _isHurt;
    private bool _isTouchingSlope;

    private bool _jumpInput;
    private bool _jumpInputStopped;
    private bool _dashInput;
    private bool _dashInputStopped;
    private IJumpable _currentJumpable;

    private int _inputX;
    private bool _gracePeriod; //period of time that the player can jump without consuming a jumpsLeft after running off the ground -> airborne 
    
    

    public PlayerAirborneState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey) : base(player, stateMachine, playerData, animKey)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isGrounded = _playerReference.CheckIfGrounded();
        _isRising = _playerReference.CheckIfRising();
        _isTouchingWall = _playerReference.CheckIfTouchingWall();
        _currentJumpable = _playerReference.DetectJumpable();
        _isHurt = _playerReference.CheckIfHurt();
        _isTouchingSlope = _playerReference.CheckIfTouchingSlope();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (_isExitingState)
            return;

        HandleGracePeriod();

        _jumpInput = _playerReference.InputController.JumpInputPressed;
        _jumpInputStopped = _playerReference.InputController.JumpInputStopped;
        _inputX = _playerReference.InputController.NormalizedInputX;
        _dashInput = _playerReference.InputController.DashInputPressed;

        _playerReference.Anim.SetFloat("VelocityY", _playerReference.CurrentVelocity.y);

        /*if (_isTouchingSlope)
            _playerReference.SetVelocityToZero();*/
        
        if(_isHurt)
        {
            _stateMachine.TransitionState(_playerReference.HurtState);
        }
        else if(_isTouchingSlope && !_isRising)
        {
            _stateMachine.TransitionState(_playerReference.SlideState);
        }
        else if(_isTouchingSlope && _isRising)
        {
            _playerReference.SetVelocityX(0);
        }
        else if (_currentJumpable != null  && _currentJumpable.CanBeJumpedOn  && !_isRising)
        {
            _playerReference.JumpState.JumpVelocityModifer = 1.2f;
            _currentJumpable.OnPlayerJumpedOn();
            _stateMachine.TransitionState(_playerReference.JumpState);
        }
        else if (_isGrounded && !_isRising && _inputX != 0)
        {
            _stateMachine.TransitionState(_playerReference.MoveState);
        }
        else if (_isGrounded && !_isRising && _inputX == 0) // Check for if player landed
        {
            _playerReference.LandState.StartGracePeriod();
            _stateMachine.TransitionState(_playerReference.LandState);
        }  
        
        else if(_jumpInput && _playerReference.JumpState.CanJump()) // Check if can jump in air
        {
            HandleJumpMultipler();
            _playerReference.InputController.UseJumpInput();
            _stateMachine.TransitionState(_playerReference.JumpState);
        }
        else if(_isTouchingWall && _inputX == _playerReference.FacingDirection && !_isRising)
        {
            _stateMachine.TransitionState(_playerReference.WallSlideState);
        }
        /*else if(_dashInput && _playerReference.DashState.CheckIfCanDash())
        {
            _stateMachine.TransitionState(_playerReference.DashState);
        }*/
        else // Check for airborne movement
        {
            _playerReference.HandleFlip(_inputX);
            _playerReference.SetVelocityX(_playerData.MovementVelocity * _inputX);
            
        }
    }         

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        _currentJumpable = null;
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    private void HandleJumpMultipler()
    {
        if (_isRising &&  _jumpInputStopped)
        {         
            _playerReference.SetVelocityY(_playerReference.CurrentVelocity.y * _playerData.HeldJumpHeightMultiplier);                        
        }
    }

    private void HandleGracePeriod()
    {
        if (_gracePeriod && Time.time > _startTime + _playerData.AirborneGracePeriod)
        {        
            _gracePeriod = false;
            _playerReference.JumpState.UseJump();
        }
    }

    public void StartGracePeriod() => _gracePeriod = true;
}
