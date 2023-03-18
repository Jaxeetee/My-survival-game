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

    [SerializeField]
    private PlayerMovement _playerMovementScript;
    private Vector3 _rawMovementInputAxis;
    private Vector3 _smoothInputMovement;
    #endregion


    #region --- LOOK ---
    [Header("Look")]
    private Vector2 _rawMouseInputDelta;

    [SerializeField]
    private PlayerLook _playerLookScript;

    #endregion



    private PlayerInput _playerInput;



    // has to subscribe on Update() as Player Input is doing something OnEnable() and will cause errors if subscribed OnEnable()
    private void PlayerInputInit()
    {
        _playerInput.actions["Movement"].performed += OnMovement;
        _playerInput.actions["Movement"].canceled += OnMovement;


        _playerInput.actions["Look"].performed += OnLook;
        _playerInput.actions["Look"].canceled += OnLook;
    }

    private void CalculateMovementInputSmoothing()
    {
        _smoothInputMovement = Vector3.Lerp(_smoothInputMovement, _rawMovementInputAxis, Time.deltaTime * _movementSmoothingSpeed);
    }

    private void UpdatePlayerMovement()
    {
        _playerMovementScript.UpdateInputMovementValue(_smoothInputMovement);
    }

    private void UpdatePlayerLook()
    {
        _playerLookScript.GetMouseDelta(_rawMouseInputDelta);
    }

    #region -= Unity Functions =- 
    private void Awake() 
    {
        _playerInput = GetComponent<PlayerInput>();
    }


    private void OnEnable() 
    {
        
    }

    private void OnDisable() 
    {
        _playerInput.actions["Movement"].performed -= OnMovement;
        _playerInput.actions["Movement"].canceled -= OnMovement;

        _playerInput.actions["Look"].performed -= OnLook;
        _playerInput.actions["Look"].canceled -= OnLook;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInput != null)
        {
            PlayerInputInit();
        }else{
            Debug.LogError("Cannot detect PlayerInput. Will stop now");
            return;
        }


        CalculateMovementInputSmoothing();
        UpdatePlayerMovement();

        UpdatePlayerLook();
    }

    private void LateUpdate() {
        
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

    #endregion
}
