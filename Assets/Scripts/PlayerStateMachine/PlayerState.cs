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

    private AudioData _audioData;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey, AudioData audioData = null)
    {
        _playerReference = player;
        _stateMachine = stateMachine;
        _playerData = playerData;
        _animBoolKey = animKey;
        _audioData = audioData;
    }

    public virtual void OnStateEnter()
    {
        _startTime = Time.time;
        _playerReference.Anim.SetBool(_animBoolKey, true);
        Debug.Log(_animBoolKey);
        _isAnimationFinished = false;
        _isExitingState = false;

        if(_audioData != null)
            _audioData.AudioManager.PlaySound(_audioData.AudioClip, _audioData.AudioSource, TrackType.Sfx, false);
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
