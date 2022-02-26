using UnityEngine;

public class MovementSystem
{
    private readonly float _speed;
    private readonly float _sprintSpeed;
    private readonly float _jumpForce;
    private bool _inAir = true;
    private readonly GameObject _gameObject;

    private delegate void MoveVar(float h, float v);

    private MoveVar _move;

    public MovementSystem(GameObject gameObject, float speed, float sprintSpeed, float jumpForce)
    {
        _gameObject = gameObject;
        _speed = speed;
        _sprintSpeed = sprintSpeed;
        _jumpForce = jumpForce;
    }

    public bool InSprint { get; set; } = false;

    public bool InAir => _inAir;

    public void Jump()
    {
        if (!InAir)
        {
            _gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * _jumpForce);
            OnLeaveGround();
        }
    }

    private void MoveOnGround(float h, float v)
    {
        var speed = _speed;
        if (InSprint)
        {
            speed = _sprintSpeed;
        }

        _gameObject.GetComponent<Rigidbody>().velocity =
            (_gameObject.transform.right * h + _gameObject.transform.forward * v) * speed;
    }

    private void MoveAtAir(float h, float v)
    {
        _gameObject.GetComponent<Rigidbody>().AddForce((_gameObject.transform.right * h + _gameObject.transform.forward * v) * _speed);
    }

    public void Move(float h, float v)
    {
        _move?.Invoke(h, v);
    }

    public void OnTouchGround()
    {
        _inAir = false;
        _move = MoveOnGround;
    }

    public void OnLeaveGround()
    {
        _inAir = true;
        _move = MoveAtAir;
    }
}
