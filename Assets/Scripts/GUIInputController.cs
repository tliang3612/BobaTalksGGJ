using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GUIInputController : MonoBehaviour
{
    private PlayerInput _playerInput;

    public bool RInputPressed { get; private set; }

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

}
