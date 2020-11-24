using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMaster : MonoBehaviour
{
    public static InputMaster Instance { get; private set; }
    public Gamepad_Input input;

    public float m = 0;
    public bool j = false;
    public bool c = false;
    public bool x = false;
    public bool y = false;
    public bool b = false;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        input = new Gamepad_Input();

        input.Game.Move.performed += OnMove;
        input.Game.Jump.performed += OnJump;
        input.Game.Crouch.performed += OnCrouch;
        input.Game.X.performed += OnX;
        input.Game.Y.performed += OnY;
        input.Game.B.performed += OnB;
    }

    void LateUpdate()
    {
        j = false;
        x = false;
        y = false;
        b = false;
        
    }

    void OnEnable()
    {
        input.Enable();
        input.Game.Jump.Enable();
        input.Game.Move.Enable();
        input.Game.Crouch.Enable();
    }
    void OnDisable()
    {
        input.Game.Jump.Disable();
        input.Game.Move.Disable();
        input.Game.Crouch.Disable();
        input.Disable();
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        var value = ctx.ReadValue<float>();
        if (Mathf.Abs(value) > 0.7f)
            m = value;
        else
            m = 0;
    }

    void OnJump(InputAction.CallbackContext ctx)
    {
        var value = ctx.ReadValue<float>();
        if (Mathf.Abs(value) > 0.7f)
            j = true;
    }

    void OnCrouch(InputAction.CallbackContext ctx)
    {
        var value = ctx.ReadValue<float>();
        if (Mathf.Abs(value) > 0.7f)
            c = true;
        else
            c = false;
    }

    void OnX(InputAction.CallbackContext ctx)
    {
        var value = ctx.ReadValue<float>();
        x = (value == 1) ? true : false;
    }

    void OnY(InputAction.CallbackContext ctx)
    {
        var value = ctx.ReadValue<float>();
        y = (value == 1) ? true : false;
    }

    void OnB(InputAction.CallbackContext ctx)
    {
        var value = ctx.ReadValue<float>();
        b = (value == 1) ? true : false;
    }
}
