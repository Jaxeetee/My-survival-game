using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerSystem
{
    // Using the C# wrapper version. Won't use the PlayerInput Component since I find it lacking
    // I'm more used to the C# wrapper version. 
    // Must be attached to the player
    public class InputHandler : MonoBehaviour
    {
        private PlayerControls _playerControls;

        #region === UNITY FUNC ===
        private void Awake()
        {
            _playerControls = new PlayerControls();
        }

        private void OnEnable()
        {
            SubscribeGameplay();
            SubscribeMenu();
        }

        private void OnDisable()
        {
            UnsubscribeGameplay();
            UnsubscribeMenu();
        }
        #endregion

        #region === ACTION MAP FUNC ===
        public Dictionary<string, InputActionMap> actionMaps { get; private set; }

        // adds all mapNames in a Dictionary
        private void UpdateActionMapList()
        {
            actionMaps.Clear();
            foreach(var map in _playerControls.asset.actionMaps)
            {
                actionMaps.Add(map.name, map);
            }
        }

        //This basically chooses an actionmap to be enabled and disables the rest.
        public void SwitchCurrentActionMap(string mapName)
        {
            if (actionMaps.Count == 0 || actionMaps == null)
            {
                Debug.LogError("There are currently no Action maps added or action maps are null");
                return;
            }

            foreach (var map in actionMaps)
            {
                if (mapName != map.Key)
                {
                    map.Value.Disable();
                }
                else
                {
                    if (map.Value.enabled)
                    {
                        Debug.LogWarning("Action map is already Enabled!");
                        return;
                    }
                    map.Value.Enable();
                }
            }
        }
        #endregion

        #region === INPUT EVENTS ===

        #region ==== GAMEPLAY ====
        //LOOK
        public static event Action<Vector2> onRawMouseLookInput;
        //GROUND MOVEMENT
        public static event Action<Vector2> onRawGroundMovementInput;
        // CROUCH / SLIDE
        public static event Action<float> onRawCrouchInput;
        // SPRINT
        public static event Action<float> onRawSprintInput;

        private void SubscribeGameplay()
        {
            InputActionMap inputActions = _playerControls.Gameplay;

            inputActions.FindAction(StringManager.MOVEMENT).performed += OnMovement;
            inputActions.FindAction(StringManager.MOVEMENT).canceled += OnMovement;


            inputActions.FindAction(StringManager.LOOK).performed += OnLook;
            inputActions.FindAction(StringManager.LOOK).canceled += OnLook;

            inputActions.FindAction(StringManager.SPRINT).performed += OnSprint;
            inputActions.FindAction(StringManager.SPRINT).canceled += OnSprint;

            inputActions.FindAction(StringManager.CROUCH).performed += OnCrouch;
            inputActions.FindAction(StringManager.CROUCH).canceled += OnCrouch;
        }


        private void UnsubscribeGameplay()
        {
            InputActionMap inputActions = _playerControls.Gameplay;

            inputActions.FindAction(StringManager.MOVEMENT).performed -= OnMovement;
            inputActions.FindAction(StringManager.MOVEMENT).canceled -= OnMovement;

            inputActions.FindAction(StringManager.LOOK).performed -= OnLook;
            inputActions.FindAction(StringManager.LOOK).canceled -= OnLook;

            inputActions.FindAction(StringManager.SPRINT).performed -= OnSprint;
            inputActions.FindAction(StringManager.SPRINT).canceled -= OnSprint;

            inputActions.FindAction(StringManager.CROUCH).performed += OnCrouch;
            inputActions.FindAction(StringManager.CROUCH).canceled += OnCrouch;
        }

        #region ==== MOVEMENT FUNC ====
        private void OnMovement(InputAction.CallbackContext obj)
        {
            Vector2 inputAxis = obj.ReadValue<Vector2>();
            onRawGroundMovementInput(inputAxis);
        }

        private void OnLook(InputAction.CallbackContext obj)
        {
            Vector2 inputAxis = obj.ReadValue<Vector2>();
            onRawMouseLookInput(inputAxis);
        }

        private void OnSprint(InputAction.CallbackContext obj)
        {
            float inputPressed = obj.ReadValue<float>();
            onRawSprintInput?.Invoke(inputPressed);
        }

        private void OnCrouch(InputAction.CallbackContext obj)
        {
            float inputPressed = obj.ReadValue<float>();
            onRawCrouchInput?.Invoke(inputPressed);
        }


        #endregion

        #region ==== COMBAT AND INTERACTION FUNC ====

        #endregion

        #endregion

        #region ==== MENU ====
        private void SubscribeMenu()
        {
            InputActionMap inputActions = _playerControls.Menu;
        }

        private void UnsubscribeMenu()
        {
            InputActionMap inputActions = _playerControls.Menu;
            
        }
        #endregion

        #endregion
    }
}