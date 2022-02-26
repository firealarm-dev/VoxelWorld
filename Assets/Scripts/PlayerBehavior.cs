using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{

    private MovementSystem _movementSystem;

    void Awake()
    {
        _movementSystem = new MovementSystem(gameObject, 3, 5, 100);
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
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            _movementSystem.InSprint = true;
        }
        else
        {
            _movementSystem.InSprint = false;
        }
        _movementSystem.Move(h, v);
    }
}
