using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _playerMovementSpeed = 5.0f;

    [SerializeField, Range(0f, 10f)]
    [Tooltip("The closer to 0, the slippier the feel")]
    private float _smoothMovementMultiplier = 7.0f;
    private float _smoothMovementSpeed = 0f;
    private Vector3 _smoothMovement;

    [Header("Slope Handler Variables")]
    [SerializeField]
    private float _maxSlope; // max slope 
    private RaycastHit _slopeHit; // checks the feet of the player to identify the angle of the player in relations to the ground

    private Rigidbody _rb; 
    private Vector3 _movementDirection; //gets the movement direction of the player

    #region --- CROUCH ---

    [SerializeField]
    private float _crouchSpeed;

    #endregion

    #region --- JUMP ---
    [SerializeField]
    private float _jumpForce;
    #endregion

    #region --- INPUT ---
    private Vector3 _inputDirection; // gets the raw input direction
    #endregion   



#region  -= UNITY FUNCTIONS =-
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MovePlayer();

        _rb.useGravity = !OnSlope(); // checks if player is not on a flat surface
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
        Debug.Log($"Angled ground: {angledGround}");
        return angledGround;
    }


#endregion

#region -= CROUCH & SLIDE =-

    private void Crouch()
    {
        // TODO: Make player movement Slower
        
    }
#endregion

#region -= JUMP =-

#endregion

#region -= INPUT =-
    public void UpdateInputMovementValue(Vector3 value)
    {
        _inputDirection = value;
    }

#endregion

}
