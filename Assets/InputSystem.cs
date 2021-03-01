// GENERATED AUTOMATICALLY FROM 'Assets/InputSystem.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputSystem : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputSystem()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputSystem"",
    ""maps"": [
        {
            ""name"": ""Game"",
            ""id"": ""ed03c7f5-0145-49fe-89d2-bd3555b90588"",
            ""actions"": [
                {
                    ""name"": ""Primary"",
                    ""type"": ""Button"",
                    ""id"": ""a4118f9d-7073-4584-aaaa-02266a16b67e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Secondary"",
                    ""type"": ""Button"",
                    ""id"": ""6a8730ea-015c-4f38-be5e-5d63f57fd496"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Ping"",
                    ""type"": ""Button"",
                    ""id"": ""f31d7b45-cb75-4287-86c8-f3e8258b350b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""783f9623-47a9-4bf3-84b5-411e00b2c2b1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Primary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1530a517-ca4c-4e6f-b293-e4d993b1b9bf"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Secondary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53a27e40-c9c8-4357-8d63-b7137ac78359"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Ping"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Game Debug"",
            ""id"": ""9072d1ef-ea4f-4ffc-9ed6-44136781a83e"",
            ""actions"": [
                {
                    ""name"": ""GasUp"",
                    ""type"": ""Button"",
                    ""id"": ""4e6ab874-068e-4ddf-9454-22356ae893d4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LogPos"",
                    ""type"": ""Button"",
                    ""id"": ""f7688bf5-ce82-470f-9532-aed078648c45"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ResetPos"",
                    ""type"": ""Button"",
                    ""id"": ""86059ab5-2e65-408e-bb9b-f6699f3f6a45"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9deabf5a-dde5-4ba9-ae98-126fa135a7a4"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ResetPos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dac1ea2c-de4f-40f6-980e-679af395322f"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""LogPos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b01ffec-7bfb-4c28-a7a9-dafe97477838"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""GasUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Game
        m_Game = asset.FindActionMap("Game", throwIfNotFound: true);
        m_Game_Primary = m_Game.FindAction("Primary", throwIfNotFound: true);
        m_Game_Secondary = m_Game.FindAction("Secondary", throwIfNotFound: true);
        m_Game_Ping = m_Game.FindAction("Ping", throwIfNotFound: true);
        // Game Debug
        m_GameDebug = asset.FindActionMap("Game Debug", throwIfNotFound: true);
        m_GameDebug_GasUp = m_GameDebug.FindAction("GasUp", throwIfNotFound: true);
        m_GameDebug_LogPos = m_GameDebug.FindAction("LogPos", throwIfNotFound: true);
        m_GameDebug_ResetPos = m_GameDebug.FindAction("ResetPos", throwIfNotFound: true);
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
    private readonly InputAction m_Game_Primary;
    private readonly InputAction m_Game_Secondary;
    private readonly InputAction m_Game_Ping;
    public struct GameActions
    {
        private @InputSystem m_Wrapper;
        public GameActions(@InputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @Primary => m_Wrapper.m_Game_Primary;
        public InputAction @Secondary => m_Wrapper.m_Game_Secondary;
        public InputAction @Ping => m_Wrapper.m_Game_Ping;
        public InputActionMap Get() { return m_Wrapper.m_Game; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameActions set) { return set.Get(); }
        public void SetCallbacks(IGameActions instance)
        {
            if (m_Wrapper.m_GameActionsCallbackInterface != null)
            {
                @Primary.started -= m_Wrapper.m_GameActionsCallbackInterface.OnPrimary;
                @Primary.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnPrimary;
                @Primary.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnPrimary;
                @Secondary.started -= m_Wrapper.m_GameActionsCallbackInterface.OnSecondary;
                @Secondary.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnSecondary;
                @Secondary.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnSecondary;
                @Ping.started -= m_Wrapper.m_GameActionsCallbackInterface.OnPing;
                @Ping.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnPing;
                @Ping.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnPing;
            }
            m_Wrapper.m_GameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Primary.started += instance.OnPrimary;
                @Primary.performed += instance.OnPrimary;
                @Primary.canceled += instance.OnPrimary;
                @Secondary.started += instance.OnSecondary;
                @Secondary.performed += instance.OnSecondary;
                @Secondary.canceled += instance.OnSecondary;
                @Ping.started += instance.OnPing;
                @Ping.performed += instance.OnPing;
                @Ping.canceled += instance.OnPing;
            }
        }
    }
    public GameActions @Game => new GameActions(this);

    // Game Debug
    private readonly InputActionMap m_GameDebug;
    private IGameDebugActions m_GameDebugActionsCallbackInterface;
    private readonly InputAction m_GameDebug_GasUp;
    private readonly InputAction m_GameDebug_LogPos;
    private readonly InputAction m_GameDebug_ResetPos;
    public struct GameDebugActions
    {
        private @InputSystem m_Wrapper;
        public GameDebugActions(@InputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @GasUp => m_Wrapper.m_GameDebug_GasUp;
        public InputAction @LogPos => m_Wrapper.m_GameDebug_LogPos;
        public InputAction @ResetPos => m_Wrapper.m_GameDebug_ResetPos;
        public InputActionMap Get() { return m_Wrapper.m_GameDebug; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameDebugActions set) { return set.Get(); }
        public void SetCallbacks(IGameDebugActions instance)
        {
            if (m_Wrapper.m_GameDebugActionsCallbackInterface != null)
            {
                @GasUp.started -= m_Wrapper.m_GameDebugActionsCallbackInterface.OnGasUp;
                @GasUp.performed -= m_Wrapper.m_GameDebugActionsCallbackInterface.OnGasUp;
                @GasUp.canceled -= m_Wrapper.m_GameDebugActionsCallbackInterface.OnGasUp;
                @LogPos.started -= m_Wrapper.m_GameDebugActionsCallbackInterface.OnLogPos;
                @LogPos.performed -= m_Wrapper.m_GameDebugActionsCallbackInterface.OnLogPos;
                @LogPos.canceled -= m_Wrapper.m_GameDebugActionsCallbackInterface.OnLogPos;
                @ResetPos.started -= m_Wrapper.m_GameDebugActionsCallbackInterface.OnResetPos;
                @ResetPos.performed -= m_Wrapper.m_GameDebugActionsCallbackInterface.OnResetPos;
                @ResetPos.canceled -= m_Wrapper.m_GameDebugActionsCallbackInterface.OnResetPos;
            }
            m_Wrapper.m_GameDebugActionsCallbackInterface = instance;
            if (instance != null)
            {
                @GasUp.started += instance.OnGasUp;
                @GasUp.performed += instance.OnGasUp;
                @GasUp.canceled += instance.OnGasUp;
                @LogPos.started += instance.OnLogPos;
                @LogPos.performed += instance.OnLogPos;
                @LogPos.canceled += instance.OnLogPos;
                @ResetPos.started += instance.OnResetPos;
                @ResetPos.performed += instance.OnResetPos;
                @ResetPos.canceled += instance.OnResetPos;
            }
        }
    }
    public GameDebugActions @GameDebug => new GameDebugActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface IGameActions
    {
        void OnPrimary(InputAction.CallbackContext context);
        void OnSecondary(InputAction.CallbackContext context);
        void OnPing(InputAction.CallbackContext context);
    }
    public interface IGameDebugActions
    {
        void OnGasUp(InputAction.CallbackContext context);
        void OnLogPos(InputAction.CallbackContext context);
        void OnResetPos(InputAction.CallbackContext context);
    }
}
