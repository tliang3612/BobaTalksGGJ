using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerWallContactState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey, AudioClip audioToPlay) : base(player, stateMachine, playerData, animKey,  audioToPlay)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _playerReference.SetVelocityY(-_playerData.WallSlideVelocity);
    }

}
