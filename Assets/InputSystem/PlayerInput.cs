//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/InputSystem/PlayerInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Mouse"",
            ""id"": ""41b78f8e-8697-4802-a6ce-49f6c37a718c"",
            ""actions"": [
                {
                    ""name"": ""MouseMovement"",
                    ""type"": ""Value"",
                    ""id"": ""6bbfc68b-7ef3-4f11-981e-e70ca536dcf4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MouseClick"",
                    ""type"": ""Button"",
                    ""id"": ""d58cbb2b-1b98-4990-aa14-7822155497f6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3eb3c5ec-1140-496e-8643-0de5eef2cd57"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""760c0595-208c-4ebc-b30f-125614c51e41"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""KeyBoard"",
            ""id"": ""8ff69dc7-e0d9-482e-a108-e0adeaa88598"",
            ""actions"": [
                {
                    ""name"": ""SkipTurn"",
                    ""type"": ""Button"",
                    ""id"": ""e0f928f1-a346-40da-bd53-e2766896951b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e310830c-2762-450d-872b-7aee50432e24"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkipTurn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Mouse
        m_Mouse = asset.FindActionMap("Mouse", throwIfNotFound: true);
        m_Mouse_MouseMovement = m_Mouse.FindAction("MouseMovement", throwIfNotFound: true);
        m_Mouse_MouseClick = m_Mouse.FindAction("MouseClick", throwIfNotFound: true);
        // KeyBoard
        m_KeyBoard = asset.FindActionMap("KeyBoard", throwIfNotFound: true);
        m_KeyBoard_SkipTurn = m_KeyBoard.FindAction("SkipTurn", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Mouse
    private readonly InputActionMap m_Mouse;
    private List<IMouseActions> m_MouseActionsCallbackInterfaces = new List<IMouseActions>();
    private readonly InputAction m_Mouse_MouseMovement;
    private readonly InputAction m_Mouse_MouseClick;
    public struct MouseActions
    {
        private @PlayerInput m_Wrapper;
        public MouseActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouseMovement => m_Wrapper.m_Mouse_MouseMovement;
        public InputAction @MouseClick => m_Wrapper.m_Mouse_MouseClick;
        public InputActionMap Get() { return m_Wrapper.m_Mouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseActions set) { return set.Get(); }
        public void AddCallbacks(IMouseActions instance)
        {
            if (instance == null || m_Wrapper.m_MouseActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MouseActionsCallbackInterfaces.Add(instance);
            @MouseMovement.started += instance.OnMouseMovement;
            @MouseMovement.performed += instance.OnMouseMovement;
            @MouseMovement.canceled += instance.OnMouseMovement;
            @MouseClick.started += instance.OnMouseClick;
            @MouseClick.performed += instance.OnMouseClick;
            @MouseClick.canceled += instance.OnMouseClick;
        }

        private void UnregisterCallbacks(IMouseActions instance)
        {
            @MouseMovement.started -= instance.OnMouseMovement;
            @MouseMovement.performed -= instance.OnMouseMovement;
            @MouseMovement.canceled -= instance.OnMouseMovement;
            @MouseClick.started -= instance.OnMouseClick;
            @MouseClick.performed -= instance.OnMouseClick;
            @MouseClick.canceled -= instance.OnMouseClick;
        }

        public void RemoveCallbacks(IMouseActions instance)
        {
            if (m_Wrapper.m_MouseActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMouseActions instance)
        {
            foreach (var item in m_Wrapper.m_MouseActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MouseActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MouseActions @Mouse => new MouseActions(this);

    // KeyBoard
    private readonly InputActionMap m_KeyBoard;
    private List<IKeyBoardActions> m_KeyBoardActionsCallbackInterfaces = new List<IKeyBoardActions>();
    private readonly InputAction m_KeyBoard_SkipTurn;
    public struct KeyBoardActions
    {
        private @PlayerInput m_Wrapper;
        public KeyBoardActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @SkipTurn => m_Wrapper.m_KeyBoard_SkipTurn;
        public InputActionMap Get() { return m_Wrapper.m_KeyBoard; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KeyBoardActions set) { return set.Get(); }
        public void AddCallbacks(IKeyBoardActions instance)
        {
            if (instance == null || m_Wrapper.m_KeyBoardActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_KeyBoardActionsCallbackInterfaces.Add(instance);
            @SkipTurn.started += instance.OnSkipTurn;
            @SkipTurn.performed += instance.OnSkipTurn;
            @SkipTurn.canceled += instance.OnSkipTurn;
        }

        private void UnregisterCallbacks(IKeyBoardActions instance)
        {
            @SkipTurn.started -= instance.OnSkipTurn;
            @SkipTurn.performed -= instance.OnSkipTurn;
            @SkipTurn.canceled -= instance.OnSkipTurn;
        }

        public void RemoveCallbacks(IKeyBoardActions instance)
        {
            if (m_Wrapper.m_KeyBoardActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IKeyBoardActions instance)
        {
            foreach (var item in m_Wrapper.m_KeyBoardActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_KeyBoardActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public KeyBoardActions @KeyBoard => new KeyBoardActions(this);
    public interface IMouseActions
    {
        void OnMouseMovement(InputAction.CallbackContext context);
        void OnMouseClick(InputAction.CallbackContext context);
    }
    public interface IKeyBoardActions
    {
        void OnSkipTurn(InputAction.CallbackContext context);
    }
}
