using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    // Using the C# wrapper version. Won't use the PlayerInput Component since I find it lacking
    // Must be attached to a GO where player inputs are needed.
    // This means that it is not limited to the player character.

    public class InputHandler : MonoBehaviour
    {
        private PlayerActionControls _playerInputs;

        //This identifies what action map will be activated.
        [SerializeField]
        private ActionMapsType _currentActionMapActive;

        #region === UNITY FUNC ===

        private void Awake()
        {
            _playerInputs = new PlayerActionControls();
        }

        private void Start()
        {
            SwitchCurrentActionMap(_currentActionMapActive.ToString());
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

        //This basically chooses an actionmap to be enabled and disables the rest.
        public void SwitchCurrentActionMap(string mapName)
        {
            foreach(var actionMap in _playerInputs.asset.actionMaps)
            {
                actionMap.Disable();
            }
            _playerInputs.asset.FindActionMap(mapName).Enable();
        }
        #endregion

        #region === INPUT EVENTS ===

        #region ==== GAMEPLAY ====
        //LOOK
        public event Action<Vector2> onRawMouseLookInput;
        //GROUND MOVEMENT
        public event Action<Vector3> onRawGroundMovementInput;

        // CROUCH / SLIDE
        public event Action OnCrouchStartedInput;
        public event Action<bool> OnCrouchHoldInput;
        public event Action<bool> OnCrouchReleasedInput;

        // SPRINT
        public event Action OnSprintStartedInput;
        public event Action<bool> OnSprintHoldInput;
        public event Action<bool> OnSprintReleasedInput;

        private void SubscribeGameplay()
        {
            InputActionMap inputActions = _playerInputs.Gameplay;

            inputActions.FindAction(StringManager.MOVEMENT).performed += OnMovement;
            inputActions.FindAction(StringManager.MOVEMENT).canceled += OnMovement;

            inputActions.FindAction(StringManager.LOOK).performed += OnLook;
            inputActions.FindAction(StringManager.LOOK).canceled += OnLook;

            inputActions.FindAction(StringManager.SPRINT).started += OnSprintStart;
            inputActions.FindAction(StringManager.SPRINT).performed += OnSprintPress; 
            inputActions.FindAction(StringManager.SPRINT).canceled += OnSprintRelease;

            inputActions.FindAction(StringManager.CROUCH).started += OnCrouchStart;
            inputActions.FindAction(StringManager.CROUCH).performed += OnCrouchPress;
            inputActions.FindAction(StringManager.CROUCH).canceled += OnCrouchRelease;
        }

        private void UnsubscribeGameplay()
        {
            InputActionMap inputActions = _playerInputs.Gameplay;

            inputActions.FindAction(StringManager.MOVEMENT).performed -= OnMovement;
            inputActions.FindAction(StringManager.MOVEMENT).canceled -= OnMovement;

            inputActions.FindAction(StringManager.LOOK).performed -= OnLook;
            inputActions.FindAction(StringManager.LOOK).canceled -= OnLook;

            inputActions.FindAction(StringManager.SPRINT).started -= OnSprintStart;
            inputActions.FindAction(StringManager.SPRINT).performed -= OnSprintPress;
            inputActions.FindAction(StringManager.SPRINT).canceled -= OnSprintRelease;

            inputActions.FindAction(StringManager.CROUCH).started -= OnCrouchStart;
            inputActions.FindAction(StringManager.CROUCH).performed -= OnCrouchPress;
            inputActions.FindAction(StringManager.CROUCH).canceled -= OnCrouchRelease;
        }

        #region ==== MOVEMENT FUNC ====
        private void OnMovement(InputAction.CallbackContext obj)
        {
            Vector2 inputAxis = obj.ReadValue<Vector2>();
            Vector3 vector3InputAxis = new Vector3(inputAxis.x, 0, inputAxis.y);
            onRawGroundMovementInput(vector3InputAxis);
        }

        private void OnLook(InputAction.CallbackContext obj)
        {
            Vector2 inputAxis = obj.ReadValue<Vector2>();
            onRawMouseLookInput(inputAxis);
        }

        #region ==== SPRINT ====
        private void OnSprintStart(InputAction.CallbackContext obj)
        {
            OnSprintStartedInput?.Invoke();
        }

        private void OnSprintPress(InputAction.CallbackContext obj)
        {
            bool sprinting = obj.ReadValueAsButton();
            OnSprintHoldInput?.Invoke(sprinting);
        }

        private void OnSprintRelease(InputAction.CallbackContext obj)
        {
            bool sprinting = obj.ReadValueAsButton();
            OnSprintReleasedInput?.Invoke(sprinting);
        }
        #endregion

        #region ==== CROUCH ====
        private void OnCrouchStart(InputAction.CallbackContext obj)
        {
            OnCrouchStartedInput?.Invoke();
        }

        private void OnCrouchPress(InputAction.CallbackContext obj)
        {
            bool crouching = obj.ReadValueAsButton();
            OnCrouchHoldInput?.Invoke(crouching);
        }

        private void OnCrouchRelease(InputAction.CallbackContext obj)
        {
            bool crouching = obj.ReadValueAsButton();
            OnCrouchReleasedInput?.Invoke(crouching);
        }
        #endregion


        #endregion

        #region ==== COMBAT AND INTERACTION FUNC ====

        #endregion

        #endregion

        #region ==== MENU ====
        private void SubscribeMenu()
        {
            InputActionMap inputActions = _playerInputs.Menu;
        }

        private void UnsubscribeMenu()
        {
            InputActionMap inputActions = _playerInputs.Menu;

        }
        #endregion

        #endregion
    }
}