using UnityEngine;
using System;

using PlayerSystem.InputSystem;

namespace PlayerSystem
{
    public class PlayerMovement : MonoBehaviour
    {

        private InputHandler _inputHandler;

        public MovementState currentState { get; private set; }

        #region === PLAYER MOVEMENT VAR ===
        [SerializeField]
        private float _movementSpeed;

        private Vector3 _direction;

        private Rigidbody _rb;

        public float currentMovementSpeed { get; private set; }

        [Space(20)]
        [Header("Slope Properties")]
        [SerializeField, Range(0f, 90f)]
        private float _maxSlopeAngleWalkable = 45.0f;

        private RaycastHit _slopeHit;
        #endregion


        #region === SPRINT VAR ===
        [Space(20)]
        [Header("Sprint Variables")]
        [Tooltip("Multiplies to _movementSpeed")]
        [SerializeField, Range(0f, 5f)]
        private float _sprintSpeedMultiplier;
        [SerializeField]
        private bool _isSprintToggle;

        #endregion

        #region === STAMINA VAR ===
        [Space(20)]
        [Header("Stamina Variable")]
        [SerializeField]
        private float _maxStamina;
        [Tooltip("Controls how fast the stamina drops")]
        [SerializeField, Range(1.01f, 5f)]
        private float _staminaDropOffMultiplier;

        public float currentStamina { get; private set; }
        #endregion


        #region === CROUCH VAR ===
        [Space(20)]
        [Header("CrouchProperties")]
        [Tooltip("This gets the percentage of the movement Speed")]
        [SerializeField, Range(0, 100)]
        private int _crouchSpeedMultiplier;

        [SerializeField]
        private bool _isCrouchToggle;
        private bool _isCrouching;
        private bool _didToggleCrouch;
        #endregion

        #region === INPUT VARS ===
        private Vector3 _inputMovementDirection;
        private float _inputSprint;
        private float _inputCrouch;
        #endregion


        #region === UNITY FUNCS ===

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _inputHandler = GetComponent<InputHandler>();
        }

        private void Start()
        {
            currentMovementSpeed = _movementSpeed;
        }

        private void OnEnable()
        {
            _inputHandler.onRawGroundMovementInput += UpdateInputMovementDirection;
            _inputHandler.onRawCrouchPressedInput += InputCrouchPressed;
            _inputHandler.OnRawCrouchReleaseInput += InputCrouchReleased;
            _inputHandler.onRawCrouchPressedInput += InputCrouchPressedToggle;
            _inputHandler.OnRawCrouchReleaseInput += InputCrouchReleasedToggle;
            _inputHandler.onRawSprintInput += UpdateInputSprint;
        }

        private void OnDisable()
        {
            _inputHandler.onRawGroundMovementInput -= UpdateInputMovementDirection;
            _inputHandler.onRawCrouchPressedInput -= InputCrouchPressed;
            _inputHandler.OnRawCrouchReleaseInput -= InputCrouchReleased;
            _inputHandler.onRawCrouchPressedInput -= InputCrouchPressedToggle;
            _inputHandler.OnRawCrouchReleaseInput -= InputCrouchReleasedToggle;
            _inputHandler.onRawSprintInput -= UpdateInputSprint;
        }

        private void Update()
        {
            Debug.Log(currentState);
            Debug.Log($"is crouching: {_isCrouching}");

            switch (currentState)
            {
                case MovementState.Crouch:
                    Crouch();
                    break;
                case MovementState.Sprint:

                    break;
                case MovementState.Slide: 

                    break;
                case MovementState.FlatMovement:
                    NormalState();
                    //currentMovementSpeed = _movementSpeed;
                    break;
                case MovementState.SlopeMovement:
                    NormalState();
                    //currentMovementSpeed = _movementSpeed;

                    break;
            }
        }

        private void FixedUpdate()
        {
            _direction = GetMovementDirection();

            _rb.MovePosition(transform.position + _direction * currentMovementSpeed * Time.deltaTime);
        }
        #endregion

        #region === DIRECTION FUNC ===
        private Vector3 GetFrontDirection()
        {
            var forward = transform.forward;
            var right = transform.right;

            forward.y = right.y = 0f; // both forward and right y values will have a value of 0f

            return _inputMovementDirection.x * right + _inputMovementDirection.z * forward;
        }

        //This checks if the Player is on a valid slope
        private bool OnWalkableSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, 2f))
            {
                float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
                return angle < _maxSlopeAngleWalkable && angle != 0f;
            }
            return false;
        }

        private Vector3 GetSlopeDirection()
        {
            Vector3 angledGround = Vector3.ProjectOnPlane(GetFrontDirection(), _slopeHit.normal);
            return angledGround;
        }

        //this determines when to use the normal direction and the slope direction
        private Vector3 GetMovementDirection()
        {
            if (OnWalkableSlope())
            {
                if (!_isCrouching) currentState = MovementState.SlopeMovement;
                //To remove unecessary jump off of the player involving ramps especially when going down.
                if (_rb.velocity.y > 0f)
                {
                    _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
                return GetSlopeDirection();
            }
            else
            {
                if (!_isCrouching) currentState = MovementState.FlatMovement;
                return GetFrontDirection();
            }
        }

        #endregion

        #region === NORMAL GROUND MOVEMENT ===
        private void NormalState()
        {
            if (_isCrouching) return; 

            currentMovementSpeed = _movementSpeed;
        }
        #endregion

        #region === CROUCH FUNC ===
        private void Crouch()
        {
            currentMovementSpeed = _crouchSpeedMultiplier / 100f * _movementSpeed;
        }
        #endregion

        #region === SPRINT FUNC ===

        #endregion

        #region === INPUT GETTER ===
        private void UpdateInputMovementDirection(Vector3 inputMovementDirection)
        {
            _inputMovementDirection = inputMovementDirection;
        }

        #region ==== CROUCH ====
        #region ===== HOLD KEY =====
        private void InputCrouchPressed()
        {
            if (_isCrouchToggle) return;

            currentState = MovementState.Crouch;
            _isCrouching = true;
        }

        private void InputCrouchReleased()
        {
            if (_isCrouchToggle) return;

            _isCrouching = false;
        }
        #endregion

        #region ===== TOGGLE KEY =====
        private void InputCrouchPressedToggle()
        {
            if (!_isCrouchToggle) return; // this just says if the player decided not to use toggle for crouching

            if (!_didToggleCrouch)
            {
                // on key down
                _isCrouching = !_isCrouching; // this determines if the player is crouching or not
                currentState = MovementState.Crouch;

                _didToggleCrouch = true;
            }
            Debug.Log($"did toggle crouch: {_didToggleCrouch}");
        }

        private void InputCrouchReleasedToggle()
        {
            if (!_isCrouchToggle) return;

            if (_didToggleCrouch) _didToggleCrouch = false;
        }
        #endregion
        #endregion

        private void UpdateInputSprint(float inputValue)
        {
            _inputSprint = inputValue;
        }
        #endregion

    }

}


//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerMovement : MonoBehaviour
//{

//#region --- PLAYER MOVEMENT ---

//    [SerializeField]
//    private float _playerNormalSpeed = 5.0f;

//    [SerializeField, Range(0f, 10f)]
//    [Tooltip("The closer to 0, the slippier the feel")]
//    private float _smoothMovementMultiplier = 7.0f;
//    private float _smoothMovementSpeed = 0f;

//    public float playerMovementSpeed{
//        get => _playerMovementSpeed;
//        set => _playerMovementSpeed = value;
//    }
//    private Vector3 _smoothMovement;

//    [Header("Slope Handler Variables")]
//    [SerializeField]
//    private float _maxSlope; // max slope 
//    private RaycastHit _slopeHit; // checks the feet of the player to identify the angle of the player in relations to the ground


//    private float _playerMovementSpeed;

//    private Rigidbody _rb; 
//    private Vector3 _movementDirection; //gets the movement direction of the player

//#endregion


//#region --- SPRINT ---
//    [SerializeField, Tooltip("when player is sprinting.")]
//    private float _sprintSpeedMultiplier;
//    private float _sprintInput;

//#endregion

//#region --- CROUCH ---

//    [SerializeField, Tooltip("Measured in percentage of a player Normal Speed. So if it is set to 25%, crouch speed will be 25% of the normal Speed")]
//    [Range(0f,100f)]
//    private float _crouchSpeed = 50;
//    private float _inputCrouch;

//    private bool _isCrouching;
//#endregion

//#region --- JUMP ---
//    private bool _toggleCrouch;
//#endregion

//#region --- INPUT ---
//    private Vector3 _inputDirection; // gets the raw input direction

//    [SerializeField]
//    private PlayerController _playerController;
//#endregion   



//#region  -= UNITY FUNCTIONS =-
//    private void Awake()
//    {
//        _rb = GetComponent<Rigidbody>();
//    }

//    private void OnEnable()
//    {
//#region -= SUBSCRIBE TO PLAYER CONTROLLER =-
//        _playerController.onRawMovementInputAxisChange += UpdateInputMovementValue;
//        _playerController.onCrouchInputValueChange += UpdateInputCrouchValue;
//        _playerController.onToggleCrouchChange += UpdateCrouchToggle;
//        _playerController.onSprintInputValueChange += UpdateInputSpringValue;
//#endregion
//    }

//    private void Update() {
//        Debug.Log($"current speed: {playerMovementSpeed}");
//    }

//    private void FixedUpdate()
//    {
//        MovePlayer();
//        Crouch();

//        _rb.useGravity = !OnSlope(); // checks if player is not on a flat surface
//    }

//    private void OnDrawGizmos() 
//    {
//        Debug.DrawRay(transform.position, -transform.up * 0.8f, Color.red);
//    }
//#endregion

//#region -= GROUND MOVEMENT =-


//    private bool OnSlope()
//    {
//        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, 2f)) {
//            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
//            return angle < _maxSlope && angle != 0;
//        }
//        return false;
//    }

//    private Vector3 GetSlopeDirection()
//    {
//        Vector3 angledGround = Vector3.ProjectOnPlane(_movementDirection, _slopeHit.normal);
//        return angledGround;
//    }


//    private delegate bool SpeedCondition();
//    // changes speed depending on the movement state of the player.
//    private float ChangePlayerMovementValue(SpeedCondition condition, float changeTo) 
//    {
//        float speed = 0f;
//        if (condition.Invoke()){
//            speed = changeTo;
//        }
//        return speed;
//    }


//#endregion

//#region -= CROUCH & SLIDE =-

//    private void Crouch()
//    {

//        if (!_isCrouching)
//        {
//            _playerMovementSpeed = _playerNormalSpeed;
//            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 8f);
//        }
//        else 
//        {
//            float crouchSpeed = (_crouchSpeed/100) * _playerNormalSpeed;
//            _playerMovementSpeed = crouchSpeed;
//            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 0.5f, 1f), Time.deltaTime * 8f);
//        }

//    }


//#endregion

//#region -= JUMP =-

//    private bool IsGrounded()
//    {
//        var onGround = Physics.Raycast(transform.position + new Vector3(0f, 0.5f, 0f), -transform.up,0.8f);
//        return onGround;
//    }

//#endregion

//#region -= SPRINT =-
//    private void UpdateInputSpringValue(float value)
//    {
//        _sprintInput = value;
//    }

//    private bool IsSprinting()
//    {
//        return _sprintInput > 0;
//    }
//#endregion    

//#region -= INPUT =-
//    private void UpdateInputMovementValue(Vector3 value)
//    {
//        _inputDirection = value;
//    }

//    private void UpdateInputCrouchValue(float value)
//    {
//        _inputCrouch = value;
//    }

//    private void UpdateCrouchToggle(bool value)
//    {
//        _toggleCrouch = value;

//        if (_toggleCrouch) {
//            _isCrouching = !_isCrouching;
//        }
//    }

//#endregion

//}
