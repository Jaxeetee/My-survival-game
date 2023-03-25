using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


//TODO Jump, Crouch, Look, Sprint 

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
#region --- MOVEMENT ---
    [Header("Movement")]

    [SerializeField, Range(1f,10f)]
    private float _movementSmoothingSpeed = 2f;


    private Vector3 _rawMovementInputAxis;

    public event Action<Vector3> onRawMovementInputAxisChange;
    public Vector3 rawMovementInputAxis { 
        get => _rawMovementInputAxis; 
        private set {
            _rawMovementInputAxis = value;
        } 
    }

    private Vector3 _smoothInputMovement;
#endregion

#region --- LOOK ---
    private Vector2 _rawMouseInputDelta;
    public event Action<Vector2> onRawMouseInputDeltaChange;
    public Vector2 rawMouseInputDelta{
        get => _rawMouseInputDelta;
        private set {
            _rawMouseInputDelta = value;
        }
    }
#endregion

#region --- JUMP ---
    private float _jumpInputValue;

    public event Action<float> onJumpInputValueChange;
    public float jumpInputValue{
        get => _jumpInputValue;
        private set {
            _jumpInputValue = value;
        }
    }
#endregion

#region --- CROUCH ---
    private float _crouchInputValue;

    public event Action<float> onCrouchInputValueChange;
    public float crouchInputValue { 
        get => _crouchInputValue; 
        private set {
            _crouchInputValue = value;
        } 
    }
    private bool _didPress = false;


#endregion

#region --- REFERENCE PLAYER SCRIPTS ---
    [Space]
    [Header("Reference Player Scripts")]

    [SerializeField]
    private PlayerMovement _playerMovementScript;

    [SerializeField]
    private PlayerLook _playerLookScript;
#endregion

    private PlayerInput _playerInput;



    // has to subscribe on Update() as Player Input is doing something OnEnable() and will cause errors if subscribed OnEnable()
    private void PlayerInputInit()
    {
        if (_playerInput.defaultActionMap != "Gameplay") return;
        // ground movement
        _playerInput.actions["Movement"].performed += OnMovement;
        _playerInput.actions["Movement"].canceled += OnMovement;

        // player jump
        _playerInput.actions["Jump"].started += OnJump;
        _playerInput.actions["Jump"].canceled += OnJump;

        // player crouch
         _playerInput.actions["Crouch"].started += OnCrouchPressed;
         _playerInput.actions["Crouch"].canceled += OnCrouchRelease;

        // player Look
        _playerInput.actions["Look"].performed += OnLook;
        _playerInput.actions["Look"].canceled += OnLook;
    }

    private void CalculateMovementInputSmoothing()
    {
        _smoothInputMovement = Vector3.Lerp(_smoothInputMovement, _rawMovementInputAxis, Time.deltaTime * _movementSmoothingSpeed);
    }


    private void UpdateToggle(bool value)
    {
        _playerMovementScript.UpdateToggle(value);
    }

    #region -= Unity Functions =- 
    private void Awake() 
    {
        _playerInput = GetComponent<PlayerInput>();
    }


    private void OnDisable() 
    {
        _playerInput.actions["Movement"].performed -= OnMovement;
        _playerInput.actions["Movement"].canceled -= OnMovement;

        _playerInput.actions["Jump"].started -= OnJump;
        _playerInput.actions["Jump"].canceled -= OnJump;

        _playerInput.actions["Crouch"].performed -= OnCrouchPressed;
        _playerInput.actions["Crouch"].canceled -= OnCrouchRelease;
        
        _playerInput.actions["Look"].performed -= OnLook;
        _playerInput.actions["Look"].canceled -= OnLook;
    }

    void Update()
    {
        if (_playerInput != null)
        {
            PlayerInputInit();
        }else{
            Debug.LogError("Cannot detect PlayerInput. Will stop now");
            return;
        }

        #region -= PLAYER INPUT UPDATE =-
        //? Contemplating to change to remove this since this will be restricted to a Player Input Component anyways
        #endregion
    }
    #endregion


    #region -= PlayerInput Events =- 

    private void OnMovement(InputAction.CallbackContext ctx)
    {
        Vector2 inputAxis = ctx.ReadValue<Vector2>();
        rawMovementInputAxis = new Vector3(inputAxis.x, 0, inputAxis.y);
        onRawMovementInputAxisChange?.Invoke(rawMovementInputAxis);
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        _rawMouseInputDelta = ctx.ReadValue<Vector2>();
        onRawMouseInputDeltaChange?.Invoke(_rawMouseInputDelta);
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        _jumpInputValue = ctx.ReadValue<float>();
        onJumpInputValueChange?.Invoke(_jumpInputValue);
    }


    private void OnCrouchPressed(InputAction.CallbackContext ctx)
    {
        if (!_didPress)
        {
            _crouchInputValue = ctx.ReadValue<float>();
            onCrouchInputValueChange?.Invoke(_crouchInputValue);
            _didPress = true;
            Debug.Log($"did press {_didPress}");
            UpdateToggle(_didPress);
        }
        
    }

    private void OnCrouchRelease(InputAction.CallbackContext ctx)
    {
        _didPress = false;
    }

    #endregion
}
