// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input/Gamepad_Input.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Gamepad_Input : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Gamepad_Input()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Gamepad_Input"",
    ""maps"": [
        {
            ""name"": ""Game"",
            ""id"": ""35dd0c91-2dd5-4a4f-ac8c-6f77133e7cec"",
            ""actions"": [
                {
                    ""name"": ""L-Stick"",
                    ""type"": ""Value"",
                    ""id"": ""57f30d21-0501-4698-83fa-0587bf56b31a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""D-Pad"",
                    ""type"": ""Value"",
                    ""id"": ""153f9eda-6172-481c-b4fc-3bbfcccdc4bc"",
                    ""expectedControlType"": ""Dpad"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Light"",
                    ""type"": ""Button"",
                    ""id"": ""8e2c57aa-8b4f-48c0-b406-d562403151e8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Heavy"",
                    ""type"": ""Button"",
                    ""id"": ""e60bcf93-744c-4a58-a448-795d1ca37ba6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Block"",
                    ""type"": ""Button"",
                    ""id"": ""50cc7379-018b-4667-9e97-58002cbf47f7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Low"",
                    ""type"": ""Button"",
                    ""id"": ""2a99424b-52ed-4b6d-9ea6-8aba460887a2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f9d4ca19-9b08-4b5a-a41d-c583a94beedc"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""D-Pad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a63419f4-5c5a-43dd-98b2-5950b72f3755"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""L-Stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d38e195-6d78-4897-967e-7b8b1e2a0942"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Light"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dc256dee-269b-4480-a5d2-e2715bd77a1a"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Heavy"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b09e79bf-c349-419e-8f5b-3afa83d1fb51"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cf79dbc0-deb1-4dfd-8ed9-56ea455ba953"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Low"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""94f37f89-3efc-47f3-9956-4e461993508a"",
            ""actions"": [
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""723d612b-cf0c-41f9-9afe-829dfc21243e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Button"",
                    ""id"": ""8ed76f5a-312e-4076-a8e1-d936b52cbb28"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""1babed90-e564-432a-9a13-eb39d6e010b9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Start"",
                    ""type"": ""Button"",
                    ""id"": ""e3381d98-9981-487a-abd3-5cc28bcd0947"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""67b04009-8615-4a64-9dcb-7b5f7e2cd74d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ee072a47-f8d2-44ab-8eed-3b1e3cff1aba"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""a97011a4-ac5d-4cf3-841a-604677d8c99c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8fb6ce03-28a3-468d-9a32-0062e1b5667b"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3ab34571-ce53-44ee-98ec-082881c1a737"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""42300bfb-d0e4-4f26-9149-040d4829e123"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""adb0dc1e-7cfd-45d9-806e-d1e47c3ea757"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""3a4c70fa-d345-4fd4-abd7-bcd59e432584"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9a5a9971-c8e0-4f4c-866a-d840b812511c"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4d7e4c66-186f-4765-8306-e92675b2141f"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d9e4dbc9-cfa9-4f6c-8e2d-845a165ef1f1"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""459b09ad-d295-4042-b33a-33f205ccadba"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9e9fdb55-c982-4625-9f04-abc832bfde23"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""46b86db6-58cf-42d6-9a43-d64816d7fd7a"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9a98e81f-bbb4-444a-b1e8-85f41aebaa26"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Game
        m_Game = asset.FindActionMap("Game", throwIfNotFound: true);
        m_Game_LStick = m_Game.FindAction("L-Stick", throwIfNotFound: true);
        m_Game_DPad = m_Game.FindAction("D-Pad", throwIfNotFound: true);
        m_Game_Light = m_Game.FindAction("Light", throwIfNotFound: true);
        m_Game_Heavy = m_Game.FindAction("Heavy", throwIfNotFound: true);
        m_Game_Block = m_Game.FindAction("Block", throwIfNotFound: true);
        m_Game_Low = m_Game.FindAction("Low", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_Submit = m_Menu.FindAction("Submit", throwIfNotFound: true);
        m_Menu_Navigate = m_Menu.FindAction("Navigate", throwIfNotFound: true);
        m_Menu_Cancel = m_Menu.FindAction("Cancel", throwIfNotFound: true);
        m_Menu_Start = m_Menu.FindAction("Start", throwIfNotFound: true);
        m_Menu_Select = m_Menu.FindAction("Select", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Game
    private readonly InputActionMap m_Game;
    private IGameActions m_GameActionsCallbackInterface;
    private readonly InputAction m_Game_LStick;
    private readonly InputAction m_Game_DPad;
    private readonly InputAction m_Game_Light;
    private readonly InputAction m_Game_Heavy;
    private readonly InputAction m_Game_Block;
    private readonly InputAction m_Game_Low;
    public struct GameActions
    {
        private @Gamepad_Input m_Wrapper;
        public GameActions(@Gamepad_Input wrapper) { m_Wrapper = wrapper; }
        public InputAction @LStick => m_Wrapper.m_Game_LStick;
        public InputAction @DPad => m_Wrapper.m_Game_DPad;
        public InputAction @Light => m_Wrapper.m_Game_Light;
        public InputAction @Heavy => m_Wrapper.m_Game_Heavy;
        public InputAction @Block => m_Wrapper.m_Game_Block;
        public InputAction @Low => m_Wrapper.m_Game_Low;
        public InputActionMap Get() { return m_Wrapper.m_Game; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameActions set) { return set.Get(); }
        public void SetCallbacks(IGameActions instance)
        {
            if (m_Wrapper.m_GameActionsCallbackInterface != null)
            {
                @LStick.started -= m_Wrapper.m_GameActionsCallbackInterface.OnLStick;
                @LStick.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnLStick;
                @LStick.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnLStick;
                @DPad.started -= m_Wrapper.m_GameActionsCallbackInterface.OnDPad;
                @DPad.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnDPad;
                @DPad.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnDPad;
                @Light.started -= m_Wrapper.m_GameActionsCallbackInterface.OnLight;
                @Light.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnLight;
                @Light.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnLight;
                @Heavy.started -= m_Wrapper.m_GameActionsCallbackInterface.OnHeavy;
                @Heavy.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnHeavy;
                @Heavy.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnHeavy;
                @Block.started -= m_Wrapper.m_GameActionsCallbackInterface.OnBlock;
                @Block.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnBlock;
                @Block.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnBlock;
                @Low.started -= m_Wrapper.m_GameActionsCallbackInterface.OnLow;
                @Low.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnLow;
                @Low.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnLow;
            }
            m_Wrapper.m_GameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LStick.started += instance.OnLStick;
                @LStick.performed += instance.OnLStick;
                @LStick.canceled += instance.OnLStick;
                @DPad.started += instance.OnDPad;
                @DPad.performed += instance.OnDPad;
                @DPad.canceled += instance.OnDPad;
                @Light.started += instance.OnLight;
                @Light.performed += instance.OnLight;
                @Light.canceled += instance.OnLight;
                @Heavy.started += instance.OnHeavy;
                @Heavy.performed += instance.OnHeavy;
                @Heavy.canceled += instance.OnHeavy;
                @Block.started += instance.OnBlock;
                @Block.performed += instance.OnBlock;
                @Block.canceled += instance.OnBlock;
                @Low.started += instance.OnLow;
                @Low.performed += instance.OnLow;
                @Low.canceled += instance.OnLow;
            }
        }
    }
    public GameActions @Game => new GameActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_Submit;
    private readonly InputAction m_Menu_Navigate;
    private readonly InputAction m_Menu_Cancel;
    private readonly InputAction m_Menu_Start;
    private readonly InputAction m_Menu_Select;
    public struct MenuActions
    {
        private @Gamepad_Input m_Wrapper;
        public MenuActions(@Gamepad_Input wrapper) { m_Wrapper = wrapper; }
        public InputAction @Submit => m_Wrapper.m_Menu_Submit;
        public InputAction @Navigate => m_Wrapper.m_Menu_Navigate;
        public InputAction @Cancel => m_Wrapper.m_Menu_Cancel;
        public InputAction @Start => m_Wrapper.m_Menu_Start;
        public InputAction @Select => m_Wrapper.m_Menu_Select;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                @Submit.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnSubmit;
                @Navigate.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnNavigate;
                @Cancel.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnCancel;
                @Start.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnStart;
                @Start.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnStart;
                @Start.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnStart;
                @Select.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Start.started += instance.OnStart;
                @Start.performed += instance.OnStart;
                @Start.canceled += instance.OnStart;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);
    public interface IGameActions
    {
        void OnLStick(InputAction.CallbackContext context);
        void OnDPad(InputAction.CallbackContext context);
        void OnLight(InputAction.CallbackContext context);
        void OnHeavy(InputAction.CallbackContext context);
        void OnBlock(InputAction.CallbackContext context);
        void OnLow(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnSubmit(InputAction.CallbackContext context);
        void OnNavigate(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnStart(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
    }
}
