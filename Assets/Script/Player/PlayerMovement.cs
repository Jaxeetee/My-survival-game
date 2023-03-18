using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _playerMovementSpeed;

    private Rigidbody _rb;
    private Vector3 _movementDirection;

    private Camera _mainCam;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void UpdateInputMovementValue(Vector3 value)
    {
        _movementDirection = value;
    }

    private void MovePlayer()
    {
        Vector3 movement = FrontDirection(_movementDirection) * _playerMovementSpeed * Time.deltaTime;
        _rb.MovePosition(transform.position + movement);
    }
    
    private Vector3 FrontDirection(Vector3 movementDirection)
    {
        var forward = transform.forward;
        var right = transform.right;

        forward.y = 0f;
        right.y = 0f;

        return forward * movementDirection.z + right * movementDirection.x;
    }
}
