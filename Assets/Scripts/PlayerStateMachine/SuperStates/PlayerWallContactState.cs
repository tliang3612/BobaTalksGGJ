using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallContactState : PlayerState
{
    protected bool _isGrounded;
    protected bool _isTouchingWall;
    protected int _inputX;


    public PlayerWallContactState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey) : base(player, stateMachine, playerData, animKey)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isGrounded = _playerReference.CheckIfGrounded();
        _isTouchingWall = _playerReference.CheckIfTouchingWall();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        _inputX = _playerReference.InputController.NormalizedInputX;

        if(_isGrounded)
        {
            _stateMachine.TransitionState(_playerReference.IdleState);
        }
        else if(!_isTouchingWall || _inputX != _playerReference.FacingDirection)
        {
            _stateMachine.TransitionState(_playerReference.AirborneState);
        }
        else if(_isTouchingWall)
        {
            _stateMachine.TransitionState(_playerReference.WallSlideState);
        }
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
