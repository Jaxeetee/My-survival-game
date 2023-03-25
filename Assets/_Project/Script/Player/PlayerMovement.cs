using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float _playerNormalSpeed = 5.0f;

    [SerializeField, Range(0f, 10f)]
    [Tooltip("The closer to 0, the slippier the feel")]
    private float _smoothMovementMultiplier = 7.0f;
    private float _smoothMovementSpeed = 0f;

    public float playerMovementSpeed{
        get => _playerMovementSpeed;
        set => _playerMovementSpeed = value;
    }
    private Vector3 _smoothMovement;

    [Header("Slope Handler Variables")]
    [SerializeField]
    private float _maxSlope; // max slope 
    private RaycastHit _slopeHit; // checks the feet of the player to identify the angle of the player in relations to the ground


    private float _playerMovementSpeed;
    
    private Rigidbody _rb; 
    private Vector3 _movementDirection; //gets the movement direction of the player

#region --- SPRINT ---
    [SerializeField, Tooltip("")]
    private float _sprintSpeedMultiplier;

#endregion

#region --- CROUCH ---

    [SerializeField, Tooltip("Measured in percentage of a player Normal Speed. So if it is set to 25%, crouch speed will be 25% of the normal Speed")]
    [Range(0f,100f)]
    private float _crouchSpeed = 50;
    private float _inputCrouch;

    private bool _isCrouching;
#endregion

#region --- JUMP ---
    [SerializeField, Range(5f,100f)]
    private float _jumpForce;
    private float _inputJump;
    private bool _canJump;
    private bool _toggleCrouch;
#endregion

#region --- INPUT ---
    private Vector3 _inputDirection; // gets the raw input direction

    [SerializeField]
    private PlayerController _playerController;
#endregion   



#region  -= UNITY FUNCTIONS =-
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _playerController.onRawMovementInputAxisChange += UpdateInputMovementValue;
        _playerController.onCrouchInputValueChange += UpdateInputCrouchValue;
        _playerController.onJumpInputValueChange += UpdateInputJumpValue;
    }

    private void OnDisable()
    {
        _playerController.onRawMovementInputAxisChange -= UpdateInputMovementValue;
        _playerController.onCrouchInputValueChange -= UpdateInputCrouchValue;
        _playerController.onJumpInputValueChange -= UpdateInputJumpValue;
    }

    private void Update() {
        Debug.Log($"current speed: {playerMovementSpeed}");
        _canJump = IsGrounded();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        Jump();
        Crouch();

        _rb.useGravity = !OnSlope(); // checks if player is not on a flat surface
    }

    private void OnDrawGizmos() 
    {
        Debug.DrawRay(transform.position, -transform.up * 0.8f, Color.red);
    }
#endregion

#region -= GROUND MOVEMENT =-

    private void MovePlayer()
    {
        
        _movementDirection = FrontDirection(_inputDirection);

        
        if (OnSlope())
        {
            MovePosition(GetSlopeDirection());

            if (_rb.velocity.y > 0)
            {
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else
        {
            MovePosition(_movementDirection);
        }   
    }
    
    private Vector3 FrontDirection(Vector3 direction)
    {
        var forward = transform.forward;
        var right = transform.right;

        forward.y = 0f;
        right.y = 0f;

        return forward * direction.z + right * direction.x;
    }

    private void MovePosition(Vector3 direction)
    {
        _smoothMovement = Vector3.Lerp(_smoothMovement, direction, Time.deltaTime * _smoothMovementMultiplier); // smoothens the 
        _rb.MovePosition(transform.position + _smoothMovement * _playerMovementSpeed * Time.deltaTime);
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, 2f))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < _maxSlope && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeDirection()
    {
        Vector3 angledGround = Vector3.ProjectOnPlane(_movementDirection, _slopeHit.normal);
        return angledGround;
    }


#endregion

#region -= CROUCH & SLIDE =-

    private void Crouch()
    {
        // TODO: Make player movement Slower

        if (!_isCrouching)
        {
            _playerMovementSpeed = _playerNormalSpeed;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 8f);
        }
        else 
        {
            _playerMovementSpeed =  (_crouchSpeed/100) * _playerNormalSpeed;
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 0.5f, 1f), Time.deltaTime * 8f);
        }
            
    }

    public void UpdateToggle(bool value)
    {
        _toggleCrouch = value;

        if (_toggleCrouch)
        {
            _isCrouching = !_isCrouching;
        }
    }
#endregion

#region -= JUMP =-

    private void Jump()
    {
        // TODO: AddForce();
        if (_inputJump == 0) return;

        if (!_canJump) return;

        if (!_isCrouching)
            _rb.AddForce(transform.up * _jumpForce);
        else{
            //make the player stand
            _isCrouching = false;
        }
        
    }

    private bool IsGrounded()
    {
        var onGround = Physics.Raycast(transform.position + new Vector3(0f, 0.5f, 0f), -transform.up,0.8f);
        return onGround;
    }

#endregion

#region -= INPUT =-
    private void UpdateInputMovementValue(Vector3 value)
    {
        _inputDirection = value;
    }

    private void UpdateInputCrouchValue(float value)
    {
        _inputCrouch = value;
    }

    private void UpdateInputJumpValue(float value)
    {
        _inputJump = value;
    }

#endregion

}
