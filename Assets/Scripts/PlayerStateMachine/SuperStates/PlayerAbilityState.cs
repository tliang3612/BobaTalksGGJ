using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool _isAbilityDone;
    private bool _isGrounded;

    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey, AudioClip audioToPlay) : base(player, stateMachine, playerData, animKey, audioToPlay)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isGrounded = _playerReference.CheckIfGrounded();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isAbilityDone)
        {
            if (_isGrounded && _playerReference.CurrentVelocity.y < 0.01f)
                _stateMachine.TransitionState(_playerReference.IdleState);
            else
                _stateMachine.TransitionState(_playerReference.AirborneState);
        }          
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        _isAbilityDone = false;
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
