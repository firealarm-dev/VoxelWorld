using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speedPlayer;
    [SerializeField] private float jumpPower;
    [SerializeField] private float gravityForce;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Animator animator;
    
    
    private CharacterController _controller;
    private Vector3 _playerVelocity;
    
    private void Awake()
    {
        _controller = gameObject.GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MoveCharacter();
        UseGravity();
    }

    private void MoveCharacter()
    {
        _playerVelocity = Vector3.zero;
        _playerVelocity.x = Input.GetAxis("Horizontal") * speedPlayer;
        _playerVelocity.z = Input.GetAxis("Vertical") * speedPlayer;
        
        var relativeDirection = mainCamera.TransformDirection(_playerVelocity);

        relativeDirection = new Vector3(relativeDirection.x, 0, relativeDirection.z);
        

        
        if (_playerVelocity != Vector3.zero)
        {
            transform.forward = relativeDirection;
            animator.SetBool("isStanding", false);
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isStanding", true);
            animator.SetBool("isRunning", false);
        }
        
        Vector3 rightPlayerVelocity = transform.forward * _playerVelocity.magnitude;
        
        rightPlayerVelocity.y = gravityForce;
        _controller.Move( rightPlayerVelocity * Time.deltaTime);
    }

    private void UseGravity()
    {
        if (!_controller.isGrounded)
        {
            gravityForce -= 30f * Time.deltaTime;
            animator.SetBool("isRunning", false);
            animator.SetBool("isStanding", false);
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isFalling", false);
            gravityForce = -1f;
        }
        
        
        if (Input.GetKeyDown(KeyCode.Space) && _controller.isGrounded)
        {
            gravityForce = jumpPower;
            animator.SetBool("isRunning", false);
            animator.SetBool("isStanding", false);
            
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Jump", true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("Jump", false);
        }
    }
}
