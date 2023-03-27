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
            onRawMovementInputAxisChange?.Invoke(rawMovementInputAxis);
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
            onRawMouseInputDeltaChange?.Invoke(rawMouseInputDelta);
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
            onJumpInputValueChange?.Invoke(jumpInputValue);
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
            onCrouchInputValueChange?.Invoke(crouchInputValue);
        } 
    }

    public event Action<bool> onToggleCrouchChange;

    private bool _didToggleCrouch = false;

    public bool didToggleCrouch {
        get => _didToggleCrouch;
        private set{
            _didToggleCrouch = value;
            onToggleCrouchChange?.Invoke(didToggleCrouch);
        }
    }


#endregion

#region --- SPRINT ---

    public event Action<float> onSprintInputValueChange;
    private float _sprintInputValue;

    public float sprintInputValue{
        get => _sprintInputValue;
        private set {
            _sprintInputValue = value;
            onSprintInputValueChange?.Invoke(sprintInputValue);
        }
    }
#endregion
    private PlayerInput _playerInput;



    // has to subscribe on Update() as Player Input is doing something OnEnable() and will cause errors if subscribed OnEnable()
    private void PlayerInputInit()
    {
        if (_playerInput.defaultActionMap != "Gameplay") return; //checks the default map first so it won't produce any error

        // ground movement
        _playerInput.actions["Movement"].performed += OnMovement;
        _playerInput.actions["Movement"].canceled += OnMovement;

        // player jump
        _playerInput.actions["Jump"].performed += OnJump;
        _playerInput.actions["Jump"].canceled += OnJump;

        // player crouch
         _playerInput.actions["Crouch"].performed += OnCrouchPressed;
         _playerInput.actions["Crouch"].canceled += OnCrouchRelease;

        // player Look
        _playerInput.actions["Look"].performed += OnLook;
        _playerInput.actions["Look"].canceled += OnLook;

        // player sprinting
        _playerInput.actions["Sprint"].performed += OnSprint;
        _playerInput.actions["Sprint"].canceled += OnSprint;
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

        _playerInput.actions["Jump"].performed -= OnJump;
        _playerInput.actions["Jump"].canceled -= OnJump;

        _playerInput.actions["Crouch"].performed -= OnCrouchPressed;
        _playerInput.actions["Crouch"].canceled -= OnCrouchRelease;
        
        _playerInput.actions["Look"].performed -= OnLook;
        _playerInput.actions["Look"].canceled -= OnLook;

        _playerInput.actions["Sprint"].performed -= OnSprint;
        _playerInput.actions["Sprint"].canceled -= OnSprint;
    }

    void Update()
    {
        if (_playerInput != null){
            PlayerInputInit();
        }
        else{
            Debug.LogError("Cannot detect PlayerInput. Will stop now");
            return;
        }
    }
#endregion


#region -= PLAYEREVENT =- 
    private void OnMovement(InputAction.CallbackContext ctx)
    {
        Vector2 inputAxis = ctx.ReadValue<Vector2>();
        rawMovementInputAxis = new Vector3(inputAxis.x, 0, inputAxis.y);
        
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        rawMouseInputDelta = ctx.ReadValue<Vector2>();

    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        jumpInputValue = ctx.ReadValue<float>();
    }


    private void OnCrouchPressed(InputAction.CallbackContext ctx)
    {
        if (!_didToggleCrouch){
            crouchInputValue = ctx.ReadValue<float>();

            didToggleCrouch = true;
            Debug.Log($"did press {didToggleCrouch}");
        }
    }

    private void OnCrouchRelease(InputAction.CallbackContext ctx)
    {
        didToggleCrouch = false;
    }

    private void OnSprint(InputAction.CallbackContext ctx)
    {
        sprintInputValue = ctx.ReadValue<float>();
    }
#endregion
}
