using UnityEngine;
using UnityEngine.InputSystem;


    public class PlayerLook : MonoBehaviour
    {
        [SerializeField, Range (0.1f, 5.0f)] private float _mouseSensitivityX;
        private float _mouseInputX;

        [SerializeField, Range(0.1f, 5.0f)] private float _mouseSensitivityY;
        private float _mouseInputY;

        [SerializeField] private Transform _player;

        //max Angle for the player to look up and down
        [SerializeField, Range(0f, 90f)] private float _maxAngleAlongYAxis = 75f;

        private float _xRotation = 0f;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void OnEnable()
        {
 
        }

        private void OnDisable()
        {

        }

        void Update()
        {
           

            _xRotation -= _mouseInputY * _mouseSensitivityY * Time.deltaTime;
            _xRotation = Mathf.Clamp(_xRotation, -_maxAngleAlongYAxis, _maxAngleAlongYAxis);
            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

            //player body rotation
            _player.Rotate(Vector3.up * _mouseInputX * _mouseSensitivityX * Time.deltaTime);
        }

        public void GetMouseDelta(Vector2 delta)
        {
            _mouseInputX = delta.x;
            _mouseInputY = delta.y;
        }
    }


