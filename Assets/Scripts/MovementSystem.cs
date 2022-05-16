using System;
using UnityEngine;

public class MovementSystem
{
    private readonly float _speed;
    private readonly float _sprintSpeed;
    private readonly float _jumpForce;
    private bool _inAir = true;
    private readonly Rigidbody _rigidbody;
    private readonly GameObject _gameObject;

    private Action<float, float> _move;

    public MovementSystem(GameObject gameObject, float speed, float sprintSpeed, float jumpForce)
    {
        _gameObject = gameObject;
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _speed = speed;
        _sprintSpeed = sprintSpeed;
        _jumpForce = jumpForce;
    }

    public bool InSprint { get; set; }

    public bool InAir => _inAir;

    public void Jump()
    {
        if (!InAir)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce);
            OnLeaveGround();
        }
    }

    private void MoveOnGround(float horizontalPart, float verticalPart)
    {
        var speed = InSprint ? _sprintSpeed : _speed;

        _rigidbody.velocity = (_gameObject.transform.right * horizontalPart + _gameObject.transform.forward * verticalPart) * speed;
    }

    private void MoveAtAir(float horizontalPart, float verticalPart)
    {
        _rigidbody.AddForce((_gameObject.transform.right * horizontalPart + _gameObject.transform.forward * verticalPart) * _speed);
    }

    public void Move(float horizontalPart, float verticalPart)
    {
        _move?.Invoke(horizontalPart, verticalPart);
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
