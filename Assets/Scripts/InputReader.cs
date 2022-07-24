using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, InputPlayer.IPlayerActions
{
    public event Action UpEvent;
    public event Action DownEvent;
    public event Action LeftEvent;
    public event Action RightEvent;

    public event Action UpReleasedEvent;
    public event Action DownReleasedEvent;
    public event Action LeftReleasedEvent;
    public event Action RightReleasedEvent;

    public event Action ActionEvent;

    private InputPlayer _inputPlayer;

    public Vector2 MovementValue { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        _inputPlayer = new InputPlayer();
        _inputPlayer.Player.SetCallbacks(this);

        _inputPlayer.Player.Enable();
    }

    private void OnDestroy()
    {
        if (_inputPlayer != null)
        {
            _inputPlayer.Player.Disable();
        }
    }

    public void OnUp(InputAction.CallbackContext context)
    {
        // if (!context.performed)
        // {
        //     return;
        // }

        if (context.performed)
        {
            UpEvent?.Invoke();
        }
        else
        {
            UpReleasedEvent?.Invoke();
        }
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        ActionEvent?.Invoke();
    }

    public void OnDown(InputAction.CallbackContext context)
    {
        // if (!context.performed)
        // {
        //     return;
        // }

        if (context.performed)
        {
            DownEvent?.Invoke();
        }
        else
        {
            DownReleasedEvent?.Invoke();
        }
    }

    public void OnLeft(InputAction.CallbackContext context)
    {
        // if (!context.performed)
        // {
        //     return;
        // }

        if (context.performed)
        {
            LeftEvent?.Invoke();
        }
        else
        {
            LeftReleasedEvent?.Invoke();
        }
    }

    public void OnRight(InputAction.CallbackContext context)
    {
        // if (!context.performed)
        // {
        //     return;
        // }

        if (context.performed)
        {
            RightEvent?.Invoke();
        }
        else
        {
            RightReleasedEvent?.Invoke();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }
}