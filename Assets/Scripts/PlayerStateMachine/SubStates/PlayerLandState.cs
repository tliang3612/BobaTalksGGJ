using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    private bool _gracePeriod;
    private bool _audioPlayed;

    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey, AudioData audioData) : base(player, stateMachine, playerData, animKey, audioData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        HandleGracePeriod();

        if (_isExitingState || (_gracePeriod && _inputX == 0))
            return;
     
        if (_inputX != 0)
        {
            _stateMachine.TransitionState(_playerReference.MoveState);
        }
        else if(_isTouchingSlope && _inputX > 0)
        {
            return;
        }
        else
        {
            if (!_audioPlayed)
            {
                _audioData.AudioManager.PlaySound(_audioData.AudioClip, _audioData.AudioSource, TrackType.Sfx, false);
                _audioPlayed = true;
            }

            _playerReference.SetVelocityX(0);
        }
        
    }

    private void HandleGracePeriod()
    {
        if (_gracePeriod && Time.time > _startTime + 0.075f)
        {
            _gracePeriod = false;
        }
    }


    public void StartGracePeriod()
    {
        _gracePeriod = true;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        _playerReference.SetVelocityToZero();
        _audioPlayed = false;
    }
}
