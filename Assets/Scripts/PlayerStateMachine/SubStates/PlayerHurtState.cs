using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtState : PlayerAbilityState
{
    private bool _isGrounded;

    public PlayerHurtState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey, AudioData audioData = null) : base(player, stateMachine, playerData, animKey, audioData)
    {
    }

    public override void DoChecks()
    {
        _isGrounded = _playerReference.CheckIfGrounded();
        base.DoChecks();
        
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (_isGrounded)
        {
            _playerReference.SetVelocityToZero();
            _isAbilityDone = true;            
        }      
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        _audioData.AudioManager.PlaySound(_audioData.AudioClip, _audioData.AudioSource, TrackType.Sfx, false);
        _playerReference.JumpState.UseJump();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    
}
