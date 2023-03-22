using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
#region --- MOVEMENT ---
    [Header("Movement")]

    [SerializeField, Range(1f,10f)]
    private float _movementSmoothingSpeed = 2f;


    private Vector3 _rawMovementInputAxis;
    private Vector3 _smoothInputMovement;
#endregion

#region --- LOOK ---
    [Header("Look")]
    private Vector2 _rawMouseInputDelta;
#endregion

#region --- JUMP ---
    private float _jumpInputValue;
#endregion

#region --- CROUCH ---
    private float _crouchInputValue;
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
        _playerInput.actions["Jump"].performed -= OnJump;

        // player crouch
         _playerInput.actions["Crouch"].performed += OnCrouch;

        // player Look
        _playerInput.actions["Look"].performed += OnLook;
        _playerInput.actions["Look"].canceled += OnLook;
    }

    private void CalculateMovementInputSmoothing()
    {
        _smoothInputMovement = Vector3.Lerp(_smoothInputMovement, _rawMovementInputAxis, Time.deltaTime * _movementSmoothingSpeed);
    }

    private void UpdatePlayerJumpInput()
    {
        _playerMovementScript.UpdateInputJumpValue(_jumpInputValue);
    }

    private void UpdatePlayerCrouchInput()
    {
        _playerMovementScript.UpdateInputCrouchValue(_crouchInputValue);
    }

    private void UpdatePlayerMovementInputs()
    {
        _playerMovementScript.UpdateInputMovementValue(_rawMovementInputAxis);
    }

    private void UpdatePlayerLookInputs()
    {
        _playerLookScript.GetMouseDelta(_rawMouseInputDelta);
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

        _playerInput.actions["Crouch"].performed -= OnCrouch;
        
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
        UpdatePlayerMovementInputs();
        UpdatePlayerCrouchInput();
        UpdatePlayerJumpInput();
        UpdatePlayerLookInputs();
        #endregion
    }
    #endregion


    #region -= PlayerInput Events =- 

    private void OnMovement(InputAction.CallbackContext ctx)
    {
        Vector2 inputAxis = ctx.ReadValue<Vector2>();
        _rawMovementInputAxis = new Vector3(inputAxis.x, 0, inputAxis.y);
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        _rawMouseInputDelta = ctx.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        _jumpInputValue = ctx.ReadValue<float>();
    }

    private void OnCrouch(InputAction.CallbackContext ctx)
    {
        _crouchInputValue = ctx.ReadValue<float>();
    }

    #endregion
}
