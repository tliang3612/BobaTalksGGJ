using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Camera _camera;

    public Vector2 RawMovementInput { get; private set; }
    public int NormalizedInputX { get; private set; }
    public int NormalizedInputY { get; private set; }

    public bool JumpInputPressed { get; private set; }
    public bool JumpInputStopped { get; private set; }

    public bool DashInputPressed { get; private set; }
    public bool DashInputStopped { get; private set; }

    public bool InteractInputPressed { get; private set; }
    public bool InteractInputStopped { get; private set; }

    public Vector2 RawDashDirectionInput { get; private set; }

    [SerializeField] private float _inputHoldTime = 0.2f;

    private float _dashInputStartTime;
    private float _jumpInputStartTime;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _camera = Camera.main;

    }

    private void Update()
    {
        CheckJumpInputHoldTime();
    }

    #region Movement
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        if (Mathf.Abs(RawMovementInput.x) > 0.5f)
            NormalizedInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        else
            NormalizedInputX = 0;


        if (Mathf.Abs(RawMovementInput.y) > 0.5f)
            NormalizedInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
        else
            NormalizedInputY = 0;

    }
    #endregion

    #region Jump
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInputPressed = true;
            JumpInputStopped = false;
            _jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStopped = true;
        }
    }

    public void UseJumpInput() => JumpInputPressed = false;

    //Jump input is true until _inputHoldTime runs out
    private void CheckJumpInputHoldTime()
    {
        if (Time.time > _jumpInputStartTime + _inputHoldTime)
        {
            JumpInputPressed = false;
        }
    }
    #endregion

    #region Dash
    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInputPressed = true;
            DashInputStopped = false;
            _dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            DashInputPressed = false;
            DashInputStopped = true;
        }
    }

    public void UseDashInput() => DashInputPressed = false;
    #endregion

    #region Interact

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            InteractInputPressed = true;
            InteractInputStopped = false;
        }

        if (context.canceled)
        {
            InteractInputPressed = false;
            InteractInputStopped = true;
        }
    }

    public void UseInteractInput() => InteractInputPressed = false;

    #endregion

}
