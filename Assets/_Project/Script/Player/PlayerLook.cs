using UnityEngine;

using InputSystem;

namespace PlayerSystem
{
    public class PlayerLook : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 5.0f)] private float _mouseSensitivityX;
        private float _mouseInputX;

        [SerializeField, Range(0.1f, 5.0f)] private float _mouseSensitivityY;
        private float _mouseInputY;

        [SerializeField] private Transform _player;

        //max Angle for the player to look up and down
        [SerializeField, Range(0f, 90f)] private float _maxAngleAlongYAxis = 75f;

        private float _xRotation = 0f;


        #region === MOUSE VARIABLES ===

        [SerializeField, Range(0.1f, 10f)]
        private float _mouseSensitivity;

        [SerializeField, Range(0.01f, 0.5f)]
        private float _smoothness = 0.05f;

        [SerializeField, Range(10f, 90f)]
        private float _maxVerticalAngle = 75f;

        [SerializeField, Range(-90f, -10f)]
        private float _minVerticalAngle = -75f;

        private Vector2 _lookInput;
        private Vector2 _smoothLook;

        private float _verticalClamp;

        private InputHandler _inputHandler;

        #endregion


        private void Awake()
        {
            _inputHandler = GetComponentInParent<InputHandler>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void OnEnable()
        {
            _inputHandler.onRawMouseLookInput += GetMouseDelta;
        }

        private void OnDisable()
        {
            _inputHandler.onRawMouseLookInput -= GetMouseDelta;
        }


        // had to change to fixedupdate to prevent from Jitter-ness of mouse look
        void FixedUpdate()
        {
            _lookInput *= _mouseSensitivity * Time.deltaTime;
            _lookInput.y = Mathf.Clamp(_lookInput.y, -_maxAngleAlongYAxis, _maxAngleAlongYAxis);


            _smoothLook = Vector2.Lerp(_smoothLook, _lookInput, _smoothness);

            _verticalClamp = Mathf.Clamp(_verticalClamp - _smoothLook.y, -_maxAngleAlongYAxis, _maxAngleAlongYAxis);
            transform.localRotation = Quaternion.Euler(_verticalClamp, 0f, 0f);


            _player.Rotate(Vector3.up * _smoothLook.x);
        }

        private void GetMouseDelta(Vector2 delta)
        {
            _lookInput = delta;
        }
    }
}