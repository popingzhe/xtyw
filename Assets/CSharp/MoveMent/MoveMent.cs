using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMent : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed;

    private float inputX;

    private float inputY;

    private Vector2 movementInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void PlayerInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        movementInput = new Vector2 (inputX, inputY);
        movementInput = movementInput.normalized;
    }

    private void Move() 
    {
        rb.MovePosition(rb.position + movementInput*speed*Time.smoothDeltaTime);
    }

}
