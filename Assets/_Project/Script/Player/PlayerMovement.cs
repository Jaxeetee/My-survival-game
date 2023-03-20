using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _playerMovementSpeed;

    [SerializeField, Tooltip("Sets the multiplier")]
    private float _smoothMultiplier = 1.0f;

    [Header("Slope Handler Variables")]
    [SerializeField]
    private float _maxSlope; // max slope 
    private RaycastHit _slopeHit; // checks the feet of the player to identify the angle of the player in relations to the ground




    private Rigidbody _rb;
    private Vector3 _movementDirection; //gets the movement direction of the player
    private Vector3 _inputDirection; // gets the raw input direction

    private Vector3 _smoothMovement;


#region  -= UNITY FUNCTIONS =-
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        SmoothInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();

        _rb.useGravity = !OnSlope(); // checks if player is not on a flat surface
    }
#endregion



    public void UpdateInputMovementValue(Vector3 value)
    {
        _inputDirection = value;
    }

    private void MovePlayer()
    {
        Vector3 vector3 = (transform.forward * _smoothMovement.z) + (transform.right * _smoothMovement.x);
        _movementDirection = vector3;
        
        if (OnSlope())
        {
            _rb.MovePosition(transform.position + GetSlopeDirection() * _playerMovementSpeed * Time.deltaTime);

            if (_rb.velocity.y > 0)
            {
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else
        {
            _rb.MovePosition(transform.position + _movementDirection * _playerMovementSpeed * Time.deltaTime);
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
        return Vector3.ProjectOnPlane(_movementDirection, _slopeHit.normal).normalized;
    }

    private void SmoothInput()
    {
        if (!OnSlope())
            _smoothMovement = Vector3.Lerp(_smoothMovement, _inputDirection, Time.deltaTime * _smoothMultiplier);
        else
            _smoothMovement = _inputDirection;
    }
}
