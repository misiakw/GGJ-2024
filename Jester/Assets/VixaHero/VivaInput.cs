//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/VixaHero/VivaInput.inputactions
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

public partial class @VivaInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @VivaInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""VivaInput"",
    ""maps"": [
        {
            ""name"": ""Viixa"",
            ""id"": ""1f0054f8-6664-4a28-8b2c-99085b8bc5cd"",
            ""actions"": [
                {
                    ""name"": ""Key1"",
                    ""type"": ""Button"",
                    ""id"": ""1c80c437-a663-4470-b209-7c2587385206"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Key2"",
                    ""type"": ""Button"",
                    ""id"": ""46c3783e-a3c1-498d-905a-8b1c71782654"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Key3"",
                    ""type"": ""Button"",
                    ""id"": ""08ee9a90-d3fd-4abb-a7ba-ccb8e366e344"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Key4"",
                    ""type"": ""Button"",
                    ""id"": ""5e8a2639-ea07-4e70-8244-461ebb0fcea9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a28a031f-ac2d-4e0f-a823-39e2891fe6a6"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Key1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3514e0e-4df6-48d1-95dc-feef2346432e"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Key2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""076f2451-4ae4-44b8-ad3c-a73c917ebd8a"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Key3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c940a8a-b7b0-4b99-b5fd-99f911ffcd40"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Key4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Viixa
        m_Viixa = asset.FindActionMap("Viixa", throwIfNotFound: true);
        m_Viixa_Key1 = m_Viixa.FindAction("Key1", throwIfNotFound: true);
        m_Viixa_Key2 = m_Viixa.FindAction("Key2", throwIfNotFound: true);
        m_Viixa_Key3 = m_Viixa.FindAction("Key3", throwIfNotFound: true);
        m_Viixa_Key4 = m_Viixa.FindAction("Key4", throwIfNotFound: true);
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

    // Viixa
    private readonly InputActionMap m_Viixa;
    private List<IViixaActions> m_ViixaActionsCallbackInterfaces = new List<IViixaActions>();
    private readonly InputAction m_Viixa_Key1;
    private readonly InputAction m_Viixa_Key2;
    private readonly InputAction m_Viixa_Key3;
    private readonly InputAction m_Viixa_Key4;
    public struct ViixaActions
    {
        private @VivaInput m_Wrapper;
        public ViixaActions(@VivaInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Key1 => m_Wrapper.m_Viixa_Key1;
        public InputAction @Key2 => m_Wrapper.m_Viixa_Key2;
        public InputAction @Key3 => m_Wrapper.m_Viixa_Key3;
        public InputAction @Key4 => m_Wrapper.m_Viixa_Key4;
        public InputActionMap Get() { return m_Wrapper.m_Viixa; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ViixaActions set) { return set.Get(); }
        public void AddCallbacks(IViixaActions instance)
        {
            if (instance == null || m_Wrapper.m_ViixaActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ViixaActionsCallbackInterfaces.Add(instance);
            @Key1.started += instance.OnKey1;
            @Key1.performed += instance.OnKey1;
            @Key1.canceled += instance.OnKey1;
            @Key2.started += instance.OnKey2;
            @Key2.performed += instance.OnKey2;
            @Key2.canceled += instance.OnKey2;
            @Key3.started += instance.OnKey3;
            @Key3.performed += instance.OnKey3;
            @Key3.canceled += instance.OnKey3;
            @Key4.started += instance.OnKey4;
            @Key4.performed += instance.OnKey4;
            @Key4.canceled += instance.OnKey4;
        }

        private void UnregisterCallbacks(IViixaActions instance)
        {
            @Key1.started -= instance.OnKey1;
            @Key1.performed -= instance.OnKey1;
            @Key1.canceled -= instance.OnKey1;
            @Key2.started -= instance.OnKey2;
            @Key2.performed -= instance.OnKey2;
            @Key2.canceled -= instance.OnKey2;
            @Key3.started -= instance.OnKey3;
            @Key3.performed -= instance.OnKey3;
            @Key3.canceled -= instance.OnKey3;
            @Key4.started -= instance.OnKey4;
            @Key4.performed -= instance.OnKey4;
            @Key4.canceled -= instance.OnKey4;
        }

        public void RemoveCallbacks(IViixaActions instance)
        {
            if (m_Wrapper.m_ViixaActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IViixaActions instance)
        {
            foreach (var item in m_Wrapper.m_ViixaActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ViixaActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ViixaActions @Viixa => new ViixaActions(this);
    public interface IViixaActions
    {
        void OnKey1(InputAction.CallbackContext context);
        void OnKey2(InputAction.CallbackContext context);
        void OnKey3(InputAction.CallbackContext context);
        void OnKey4(InputAction.CallbackContext context);
    }
}