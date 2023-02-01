using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool _isAbilityDone;
    private bool _isGrounded;

    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey, AudioData audioData = null) : base(player, stateMachine, playerData, animKey, audioData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isGrounded = _playerReference.CheckIfGrounded();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

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

}
