using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CCTest : MonoBehaviour
{
    [SerializeField] private bool isSimpleMove;
    
    [SerializeField] private float moveSpeed;
    
    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        var inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        var moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        if (moveDirection.sqrMagnitude > 1)
        {
            moveDirection.Normalize();
        }

        if (isSimpleMove)
        {
            _characterController.SimpleMove(moveDirection * moveSpeed);
        }
        else
        {
            _characterController.Move(moveDirection * (moveSpeed * Time.deltaTime));
        }
    }
}
