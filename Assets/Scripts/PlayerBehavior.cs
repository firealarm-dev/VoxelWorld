using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float speed = 3;
    [SerializeField] private float sprintSpeed = 5;
    [SerializeField] private float jumpForce = 100;

    private MovementSystem _movementSystem;
    private CameraSystem _cameraSystem;

    private void Awake()
    {
        _movementSystem = new MovementSystem(gameObject, speed, sprintSpeed, jumpForce);
        _cameraSystem = new CameraSystem(transform, _movementSystem, cameraTransform);
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

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        //Move smb relative to the camera
        if (!(h == 0 && v == 0))
        {
            _cameraSystem.MoveUnderCamera(h, v);
        }
    }
}
