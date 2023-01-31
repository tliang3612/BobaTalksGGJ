using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int _jumpsLeft;
    public float JumpVelocityModifer { get; set; }

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey, AudioClip audioToPlay) : base(player, stateMachine, playerData, animKey, audioToPlay)
    {
        _jumpsLeft = _playerData.NumJumps;
    }

    //State is entered for only one frame
    public override void OnStateEnter()
    {
        base.OnStateEnter();

        UseJump();
        _playerReference.SetVelocityY(_playerData.JumpVelocity);
        _isAbilityDone = true;
        //_playerReference.AirborneState.SetIsRisingTrue();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();

        JumpVelocityModifer = 1;
    }

    public bool CanJump()
    {
        return _jumpsLeft > 0;
    }

    public void ResetJumps() => _jumpsLeft = _playerData.NumJumps;
    public void UseJump() => _jumpsLeft--;
}
