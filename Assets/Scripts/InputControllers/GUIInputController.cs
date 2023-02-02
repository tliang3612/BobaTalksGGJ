using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GUIInputController : MonoBehaviour
{
    private PlayerInput _playerInput;

    public bool RInputPressed { get; private set; }
    public bool EscapeInputPressed { get; private set; }
    public bool ClickInputPressed { get; private set; }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

    }

    private void Update()
    {

    }

    #region RInput
    public void OnRInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            RInputPressed = true;
        }

        if (context.canceled)
        {
            RInputPressed = false;
        }
    }

    public void UseRInput() => RInputPressed = false;
    #endregion

    #region EscapeInput
    public void OnEscapeInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            EscapeInputPressed = true;
        }

        if (context.canceled)
        {
            EscapeInputPressed = false;
        }
    }

    public void UseEscapeInput() => EscapeInputPressed = false;
    #endregion

    #region Click
    public void OnClickInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ClickInputPressed = true;
        }

        if (context.canceled)
        {
            ClickInputPressed = false;
        }
    }

    public void UseClickInput() => ClickInputPressed = false;
    #endregion

}
