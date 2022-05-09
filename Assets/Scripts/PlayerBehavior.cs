using System;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private MovementSystem _movementSystem;
    private CameraSystem _cameraSystem;

    private void Awake()
    {
        _movementSystem = new MovementSystem(gameObject, 3, 5, 100);
        _cameraSystem = new CameraSystem(gameObject, _movementSystem);
    }
    private void OnTriggerEnter(Collider other)
    {
        _movementSystem.OnTouchGround();
    }
    private void OnTriggerExit(Collider other)
    {
        _movementSystem.OnLeaveGround();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _movementSystem.Jump();
        }
    }

    private void FixedUpdate()
    {
        _movementSystem.InSprint = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift);
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //Move smb relative to the camera
        _cameraSystem.MoveUnderCamera(h, v);
    }

}
