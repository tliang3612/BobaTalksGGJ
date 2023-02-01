using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractState : PlayerAbilityState
{
    public IInteractable Interactable { get; set; }

    private bool _interactStateFinished;
    private bool _interactInput;

    public PlayerInteractState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animKey) : base(player, stateMachine, playerData, animKey)
    {
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        _interactStateFinished = Interactable.CheckInteractFinished();

        if (_interactStateFinished)
        {
            _stateMachine.TransitionState(_playerReference.IdleState);
        }
        else if(_interactInput)
        {
            _playerReference.InputController.UseInteractInput();
            Interactable.Interact();
        }

        _interactInput = _playerReference.InputController.InteractInputPressed;
        

    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Interactable.Interact();
        _playerReference.SetVelocityToZero();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();

        Interactable = null;
        _isAbilityDone = true;
    }
}
