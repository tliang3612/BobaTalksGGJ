using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey) : base(player, stateMachine, playerData, animKey)
    { 
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        _playerReference.HandleFlip(_inputX);
        _playerReference.SetVelocityX(_playerData.MovementVelocity * _inputX);
        
        if(_inputX == 0 && !_isExitingState)
        {
            _stateMachine.TransitionState(_playerReference.IdleState);
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
