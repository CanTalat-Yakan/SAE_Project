using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMaster : MonoBehaviour
{
    public static InputMaster Instance { get; private set; }
    public Gamepad_Input input;

    void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        input = new Gamepad_Input();
    }

    void Update()
    {
        //Debug.Log(input.Game.Jump.ReadValue<float>().ToString());
    }

    void OnEnable()
    {
        input.Enable();
    }
    void OnDisable()
    {
        input.Disable();
    }

    void OnInteractButton(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
    }
}
