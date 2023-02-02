using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{

    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey) : base(player, stateMachine, playerData, animKey)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (_isExitingState)
            return;

        if (_inputX != 0)
        {
            _stateMachine.TransitionState(_playerReference.MoveState);
        }
        else
        {
            _playerReference.SetVelocityToZero();
        }
        
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();        
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        _playerReference.SetVelocityToZero();
    }
}
