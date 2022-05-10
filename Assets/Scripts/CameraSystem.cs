using System;
using UnityEngine;

public class CameraSystem
{
    private readonly Transform _targetTransform;

    private readonly MovementSystem _movementSystem;

    private Transform CameraTransform => GameObject.FindWithTag("MainCamera").transform;

    public CameraSystem(Transform targetTransform, MovementSystem movementSystem)
    {
        //transform of the player
        _targetTransform = targetTransform;
        
        _movementSystem = movementSystem;
    }

    public void MoveUnderCamera(float horizontalPart, float verticalPart)
    {
        var currentDirection = new Vector3(horizontalPart, 0, verticalPart).normalized;
        
        //Direction relative to camera direction
        var relativeDirection = CameraTransform.TransformDirection(currentDirection);
        
        //removing the vertical part
        relativeDirection = new Vector3(relativeDirection.x, 0, relativeDirection.z);
        
        //changing player direction
        _targetTransform.forward = relativeDirection;
        
        //using MovementSystem and making it move the player only forward
        var normalizedDirection = NormalizeDirection(horizontalPart, verticalPart);
        _movementSystem.Move(0, normalizedDirection);
        
    }
    
    //converting current direction of the player from any direction to forward and avoiding sums of vectors
    private float NormalizeDirection(float hPart, float vPart)
    {
        float direction = 0;
        if (hPart != 0)
        {
            direction = Math.Abs(hPart);
        }else if (vPart != 0)
        {
            direction = Math.Abs(vPart);
        }

        return direction;
    }
}
