using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int _inputX;
    protected int _inputY;

    private bool _jumpInput;
    private bool _dashInput;

    private bool _isGrounded;
    protected bool _isTouchingSlope;

    private bool _interactInput;
    private IInteractable _currentInteractable;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey, AudioData audioData = null) : base(player, stateMachine, playerData, animKey, audioData)
    {

    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        _playerReference.JumpState.ResetJumps();
        _playerReference.DashState.CanDash = true;
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        _inputX = _playerReference.InputController.NormalizedInputX;
        _jumpInput = _playerReference.InputController.JumpInputPressed;
        _dashInput = _playerReference.InputController.DashInputPressed;

        _interactInput = _playerReference.InputController.InteractInputPressed;
        _currentInteractable = _playerReference.DetectInteractable();

        if (_interactInput && _currentInteractable != null)
        {
            _playerReference.InputController.UseInteractInput();
            _playerReference.InteractState.Interactable = _currentInteractable;
            _stateMachine.TransitionState(_playerReference.InteractState);
        }
        else if (_jumpInput && _playerReference.JumpState.CanJump())
        {
            _playerReference.InputController.UseJumpInput();
            _stateMachine.TransitionState(_playerReference.JumpState);

        }
        else if (!_isGrounded && !_isTouchingSlope)
        {
            //_playerReference.AirborneState.StartGracePeriod();
            _stateMachine.TransitionState(_playerReference.AirborneState);
        }
        else if (_dashInput && _playerReference.DashState.CheckIfCanDash())
        {
            _stateMachine.TransitionState(_playerReference.DashState);
        }


    }

    public override void DoChecks()
    {
        base.DoChecks();
        _isGrounded = _playerReference.CheckIfGrounded();
        _isTouchingSlope = _playerReference.CheckIfTouchingSlope();
    }
}
