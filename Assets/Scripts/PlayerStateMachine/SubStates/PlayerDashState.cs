using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; set; }

    private bool _dashInputStop;
    private bool _isTouchingWall;


    private Vector2 _dashDirection;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey) : base(player, stateMachine, playerData, animKey)
    {
    }

    public bool CheckIfCanDash()
    {
        return CanDash && PowerupInventory.CheckIfAvailable(PowerupType.Crescendo);
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isExitingState)
            return;

        _isTouchingWall = _playerReference.CheckIfTouchingWall();
        _dashDirection = _playerReference.InputController.RawMovementInput.normalized;

        if (_isTouchingWall)
            _dashDirection = Vector2.zero;
        else if (_dashDirection == Vector2.zero)     
            _dashDirection = Vector2.right * _playerReference.FacingDirection;
        
        HandleDash(_dashDirection);
        
            
        if(Time.time >= _startTime + _playerData.DashTime)
        {
            _playerReference.Rb.drag = 0f;
            _isAbilityDone = true;
            _playerReference.SetVelocityToZero();
        }
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        _playerReference.InputController.UseDashInput();

        _startTime = Time.time;
    }

    public override void OnStateExit()
    {
        base.OnStateExit();

        _playerReference.Rb.gravityScale = 3;
        _playerReference.SetVelocityY(_playerReference.CurrentVelocity.y * _playerData.DashEndMultipler);
        CanDash = false;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }

    public void HandleDash(Vector2 dir)
    {
        _playerReference.SetVelocityToZero();
        var newVelocity = Vector2.zero;
        newVelocity += dir.normalized * _playerData.DashVelocity;

        _playerReference.SetVelocityX(newVelocity.x);
        _playerReference.SetVelocityY(newVelocity.y);

        _playerReference.Rb.gravityScale = 0f;

        _playerReference.HandleFlip(Mathf.RoundToInt(_dashDirection.x));
        _playerReference.Rb.drag = _playerData.AirDrag;
    }
}
