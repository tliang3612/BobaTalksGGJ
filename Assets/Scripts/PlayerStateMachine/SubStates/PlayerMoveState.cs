using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    private bool _isTouchingSlope;

    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey) : base(player, stateMachine, playerData, animKey)
    { 
    }

    public override void DoChecks()
    {
        base.DoChecks();
        _isTouchingSlope = _playerReference.CheckIfTouchingSlope();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
       
        if(_isTouchingSlope)
        {
            return;
        }

        else if(_inputX == 0 && !_isExitingState && !_isTouchingSlope)
        {
            _stateMachine.TransitionState(_playerReference.IdleState);
        }

        _playerReference.HandleFlip(_inputX);
        _playerReference.SetVelocityX(_playerData.MovementVelocity * _inputX);
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
