using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : PlayerState
{
    private bool _isGrounded;
    private bool _isSliding;

    public PlayerSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey, AudioData audioData = null) : base(player, stateMachine, playerData, animKey, audioData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        _isGrounded = _playerReference.CheckIfGrounded();
        _isSliding = _playerReference.CheckIfTouchingSlope();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (_isExitingState)
            return;

        _playerReference.SetVelocityY(-_playerData.SlopeSlideVelocity);
        
        if(!_isSliding)
        {
            _stateMachine.TransitionState(_playerReference.LandState);
        }
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        _playerReference.SetVelocityToZero();
        _playerReference.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        _playerReference.transform.rotation = new Quaternion(0, 0, 0, 0);        
    }

    
}
