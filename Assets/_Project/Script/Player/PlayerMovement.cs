using UnityEngine;
using System;

using InputSystem;

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
        [SerializeField, Range(0f, 100f)]
        private float _sprintSpeedMultiplier;

        [SerializeField]
        private bool _isSprintToggle;

        #endregion

        #region === CROUCH VAR ===
        [Space(20)]
        [Header("CrouchProperties")]
        [Tooltip("This gets the percentage of the movement Speed")]
        [SerializeField, Range(0, 100)]
        private int _crouchSpeedMultiplier;

        [SerializeField]
        private bool _isCrouchToggle;
        private bool _didToggleCrouch;
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

        #region === INPUT VARS ===
        private Vector3 _inputMovementDirection;
        private bool _isCrouching;
        private bool _isSprinting;
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

            _inputHandler.OnCrouchStartedInput += InputCrouchStarted;
            _inputHandler.OnCrouchHoldInput += InputCrouchOnHold;
            _inputHandler.OnCrouchReleasedInput += InputCrouchReleased;

            _inputHandler.OnSprintStartedInput += InputSprintStarted;
            _inputHandler.OnSprintHoldInput += InputSprintOnHold;
            _inputHandler.OnSprintReleasedInput -= InputSprintReleased;
        }

        private void OnDisable()
        {
            _inputHandler.onRawGroundMovementInput -= UpdateInputMovementDirection;

            _inputHandler.OnCrouchStartedInput -= InputCrouchStarted;
            _inputHandler.OnCrouchHoldInput -= InputCrouchOnHold;
            _inputHandler.OnCrouchReleasedInput -= InputCrouchReleased;

            _inputHandler.OnSprintStartedInput -= InputSprintStarted;
            _inputHandler.OnSprintHoldInput -= InputSprintOnHold;
            _inputHandler.OnSprintReleasedInput -= InputSprintReleased;
        }

        private void Update()
        {
            Debug.Log($"current State: {currentState}");
            switch (currentState)
            {
                case MovementState.Crouch:
                    Crouch();
                    break;
                case MovementState.Sprint:
                    Sprint();
                    break;
                case MovementState.Slide:

                    break;
                case MovementState.Default:
                    NormalState();
                    break;
                default:
                    NormalState();
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
                //To remove unecessary jump off of the player involving ramps especially when going down.
                if (_rb.velocity.y > 0f)
                {
                    _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
                return GetSlopeDirection();
            }
            else
            {
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
        private void Sprint()
        {
            currentMovementSpeed = (_sprintSpeedMultiplier / 100f * _movementSpeed) + _movementSpeed;
        }
        #endregion

        private void SwitchMovementState(MovementState state)
        {
            Debug.Log($"Current state: {state}");
            currentState = state;

        }

        #region === INPUT GETTER ===
        private void UpdateInputMovementDirection(Vector3 inputMovementDirection)
        {
            _inputMovementDirection = inputMovementDirection;
        }

        #region ==== CROUCH ====
        private void InputCrouchStarted()
        {
            if (!_isCrouchToggle) return; // this just says if the player decided not to use toggle for crouching

            // on key down
            _isCrouching = !_isCrouching; // this determines if the player is crouching or not

            if (_isCrouching)
                currentState = MovementState.Crouch;
            else
                currentState = MovementState.Default;
        }

        private void InputCrouchOnHold(bool value)
        {
            if (_isCrouchToggle) return;

            currentState = MovementState.Crouch;
            _isCrouching = value;
        }

        private void InputCrouchReleased(bool value)
        {
            if (_isCrouchToggle) return;
            currentState = MovementState.Default;
            _isCrouching = value;
        }

        #endregion

        #region ==== SPRINT ====

        private void InputSprintStarted()
        {
            if (!_isSprintToggle) return;

            _isSprinting = !_isSprinting;

            if (_isSprinting)
                currentState = MovementState.Sprint;
            else
                currentState = MovementState.Default;
        }

        private void InputSprintOnHold(bool value)
        {
            if (_isSprintToggle) return;

            currentState = MovementState.Sprint;
            _isSprinting = value;
        }

        private void InputSprintReleased(bool value)
        {
            if (_isSprintToggle) return;

            currentState = MovementState.Default;
            _isSprinting = value;
        }

        #endregion

        #endregion
    }

}