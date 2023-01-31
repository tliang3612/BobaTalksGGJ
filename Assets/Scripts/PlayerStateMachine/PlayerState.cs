using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected Player _playerReference;
    protected PlayerStateMachine _stateMachine;
    protected PlayerData _playerData;
    protected float _startTime;

    protected bool _isAnimationFinished;
    protected bool _isExitingState;

    private string _animBoolKey;
    private AudioClip _audioClipToPlay;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey, AudioClip audioToPlay)
    {
        _playerReference = player;
        _stateMachine = stateMachine;
        _playerData = playerData;
        _animBoolKey = animKey;
        _audioClipToPlay = audioToPlay;
    }

    public virtual void OnStateEnter()
    {
        _startTime = Time.time;
        _playerReference.Anim.SetBool(_animBoolKey, true);
        Debug.Log(_animBoolKey);
        _isAnimationFinished = false;
        _isExitingState = false;
    }

    public virtual void OnStateExit()
    {
        _playerReference.Anim.SetBool(_animBoolKey, false);
        _isExitingState = true;
    }

    public virtual void LogicUpdate()
    {
        DoChecks();
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void DoChecks()
    {

    }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishedTrigger() => _isAnimationFinished = true;

}
