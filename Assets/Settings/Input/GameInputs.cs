//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Settings/Input/GameInputs.inputactions
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

namespace SolitaireSettlement
{
    public partial class @GameInputs: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @GameInputs()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInputs"",
    ""maps"": [
        {
            ""name"": ""Interactions"",
            ""id"": ""35cd6b52-e6dc-44a3-9b27-13dae7ffa821"",
            ""actions"": [
                {
                    ""name"": ""Press"",
                    ""type"": ""Button"",
                    ""id"": ""0f30b899-fee2-43f5-be68-398a05067c6f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interaction Point"",
                    ""type"": ""Value"",
                    ""id"": ""f9b779d9-4592-4f86-b1aa-aae76f3e6dd4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f8b12d0b-4f19-4987-a60f-edaff029b627"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b6a560ea-4edc-4659-b048-98805b675384"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""Interaction Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Camera Controls"",
            ""id"": ""454dfe6c-3227-4896-9b23-38782bf8774a"",
            ""actions"": [
                {
                    ""name"": ""Pan"",
                    ""type"": ""PassThrough"",
                    ""id"": ""11e84537-3494-46c2-ab1c-40cfd15adb59"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""PassThrough"",
                    ""id"": ""abf83c65-df77-49ba-a689-86f7806877c6"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Drag"",
                    ""type"": ""Button"",
                    ""id"": ""ba6b71b1-9647-48d8-9242-bb40237e195d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""17f16dd5-fc72-4aef-b554-4dbbd22b2a36"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""Pan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""80e8dd02-d5c2-4d91-9cc1-5e2770283311"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": ""Normalize(min=-1,max=1)"",
                    ""groups"": ""Mouse"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""56c39a1a-adef-4ada-8696-bdf075eecc65"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""Drag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mouse"",
            ""bindingGroup"": ""Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Interactions
            m_Interactions = asset.FindActionMap("Interactions", throwIfNotFound: true);
            m_Interactions_Press = m_Interactions.FindAction("Press", throwIfNotFound: true);
            m_Interactions_InteractionPoint = m_Interactions.FindAction("Interaction Point", throwIfNotFound: true);
            // Camera Controls
            m_CameraControls = asset.FindActionMap("Camera Controls", throwIfNotFound: true);
            m_CameraControls_Pan = m_CameraControls.FindAction("Pan", throwIfNotFound: true);
            m_CameraControls_Zoom = m_CameraControls.FindAction("Zoom", throwIfNotFound: true);
            m_CameraControls_Drag = m_CameraControls.FindAction("Drag", throwIfNotFound: true);
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

        // Interactions
        private readonly InputActionMap m_Interactions;
        private List<IInteractionsActions> m_InteractionsActionsCallbackInterfaces = new List<IInteractionsActions>();
        private readonly InputAction m_Interactions_Press;
        private readonly InputAction m_Interactions_InteractionPoint;
        public struct InteractionsActions
        {
            private @GameInputs m_Wrapper;
            public InteractionsActions(@GameInputs wrapper) { m_Wrapper = wrapper; }
            public InputAction @Press => m_Wrapper.m_Interactions_Press;
            public InputAction @InteractionPoint => m_Wrapper.m_Interactions_InteractionPoint;
            public InputActionMap Get() { return m_Wrapper.m_Interactions; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(InteractionsActions set) { return set.Get(); }
            public void AddCallbacks(IInteractionsActions instance)
            {
                if (instance == null || m_Wrapper.m_InteractionsActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_InteractionsActionsCallbackInterfaces.Add(instance);
                @Press.started += instance.OnPress;
                @Press.performed += instance.OnPress;
                @Press.canceled += instance.OnPress;
                @InteractionPoint.started += instance.OnInteractionPoint;
                @InteractionPoint.performed += instance.OnInteractionPoint;
                @InteractionPoint.canceled += instance.OnInteractionPoint;
            }

            private void UnregisterCallbacks(IInteractionsActions instance)
            {
                @Press.started -= instance.OnPress;
                @Press.performed -= instance.OnPress;
                @Press.canceled -= instance.OnPress;
                @InteractionPoint.started -= instance.OnInteractionPoint;
                @InteractionPoint.performed -= instance.OnInteractionPoint;
                @InteractionPoint.canceled -= instance.OnInteractionPoint;
            }

            public void RemoveCallbacks(IInteractionsActions instance)
            {
                if (m_Wrapper.m_InteractionsActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IInteractionsActions instance)
            {
                foreach (var item in m_Wrapper.m_InteractionsActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_InteractionsActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public InteractionsActions @Interactions => new InteractionsActions(this);

        // Camera Controls
        private readonly InputActionMap m_CameraControls;
        private List<ICameraControlsActions> m_CameraControlsActionsCallbackInterfaces = new List<ICameraControlsActions>();
        private readonly InputAction m_CameraControls_Pan;
        private readonly InputAction m_CameraControls_Zoom;
        private readonly InputAction m_CameraControls_Drag;
        public struct CameraControlsActions
        {
            private @GameInputs m_Wrapper;
            public CameraControlsActions(@GameInputs wrapper) { m_Wrapper = wrapper; }
            public InputAction @Pan => m_Wrapper.m_CameraControls_Pan;
            public InputAction @Zoom => m_Wrapper.m_CameraControls_Zoom;
            public InputAction @Drag => m_Wrapper.m_CameraControls_Drag;
            public InputActionMap Get() { return m_Wrapper.m_CameraControls; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CameraControlsActions set) { return set.Get(); }
            public void AddCallbacks(ICameraControlsActions instance)
            {
                if (instance == null || m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Add(instance);
                @Pan.started += instance.OnPan;
                @Pan.performed += instance.OnPan;
                @Pan.canceled += instance.OnPan;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
                @Drag.started += instance.OnDrag;
                @Drag.performed += instance.OnDrag;
                @Drag.canceled += instance.OnDrag;
            }

            private void UnregisterCallbacks(ICameraControlsActions instance)
            {
                @Pan.started -= instance.OnPan;
                @Pan.performed -= instance.OnPan;
                @Pan.canceled -= instance.OnPan;
                @Zoom.started -= instance.OnZoom;
                @Zoom.performed -= instance.OnZoom;
                @Zoom.canceled -= instance.OnZoom;
                @Drag.started -= instance.OnDrag;
                @Drag.performed -= instance.OnDrag;
                @Drag.canceled -= instance.OnDrag;
            }

            public void RemoveCallbacks(ICameraControlsActions instance)
            {
                if (m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(ICameraControlsActions instance)
            {
                foreach (var item in m_Wrapper.m_CameraControlsActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public CameraControlsActions @CameraControls => new CameraControlsActions(this);
        private int m_MouseSchemeIndex = -1;
        public InputControlScheme MouseScheme
        {
            get
            {
                if (m_MouseSchemeIndex == -1) m_MouseSchemeIndex = asset.FindControlSchemeIndex("Mouse");
                return asset.controlSchemes[m_MouseSchemeIndex];
            }
        }
        public interface IInteractionsActions
        {
            void OnPress(InputAction.CallbackContext context);
            void OnInteractionPoint(InputAction.CallbackContext context);
        }
        public interface ICameraControlsActions
        {
            void OnPan(InputAction.CallbackContext context);
            void OnZoom(InputAction.CallbackContext context);
            void OnDrag(InputAction.CallbackContext context);
        }
    }
}
