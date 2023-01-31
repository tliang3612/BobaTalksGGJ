using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey, AudioClip audioToPlay) : base(player, stateMachine, playerData, animKey, audioToPlay)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(_inputX != 0 && !_isExitingState)
        {
            _stateMachine.TransitionState(_playerReference.MoveState);
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

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
