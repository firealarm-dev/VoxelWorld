using UnityEngine;

namespace Player
{
    public class PlayerBehavior : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float speed = 3;
        [SerializeField] private float sprintSpeed = 5;
        [SerializeField] private float jumpForce = 1000;

        private MovementSystem _movementSystem;
        private CameraSystem _cameraSystem;
        private CapsuleCollider _collider;

        private void Awake()
        {
            _movementSystem = new MovementSystem(gameObject, speed, sprintSpeed, jumpForce);
            _cameraSystem = new CameraSystem(transform, _movementSystem, cameraTransform);
            _collider = GetComponent<CapsuleCollider>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
            // if (Physics.Raycast(transform.position, Vector3.down, _collider.bounds.extents.y + 0.5f))
            if (Physics.SphereCast(new Ray(_collider.bounds.center, Vector3.down), _collider.radius, 0.9f))
            {
                Debug.Log("Ground");
                _movementSystem.OnTouchGround();
            }
            else
            {
                Debug.Log("Air");
                _movementSystem.OnLeaveGround();
            }
            
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
}
