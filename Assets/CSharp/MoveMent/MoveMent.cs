using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMent : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed;
    public float wSpeed;
    public float rSpeed;

    private float inputX;

    private float inputY;

    private Vector2 movementInput;

    bool isMoving;
    Animator[] animators;
    //ÕÊº“ ‰»Îøÿ÷∆
    bool inputDisable;
    private void OnEnable()
    {
        EventHander.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHander.AfterSceneloadEvent += AfterSceneloadEvent;
        EventHander.MoveToPosition += OnMoveToPosition;
    }

    private void OnDisable()
    {
        EventHander.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHander.AfterSceneloadEvent -= AfterSceneloadEvent;
        EventHander.MoveToPosition -= OnMoveToPosition;
    }

    private void OnMoveToPosition(Vector3 targetPos)
    {
       transform.position = targetPos;
    }

    private void AfterSceneloadEvent()
    {
        inputDisable = false;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        inputDisable = true; 
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!inputDisable)
            PlayerInput();
        else
            isMoving = false;
        SwitchAnimation();
    }

    private void FixedUpdate()
        
    {
        if (!inputDisable)
            Move();
    }

    private void PlayerInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(KeyCode.LeftShift) )
        {
            inputX = inputX * 0.5f;
            inputY = inputY * 0.5f;
            speed = wSpeed;
        }
        else
        {
            speed = rSpeed;
        }
        
        movementInput = new Vector2 (inputX, inputY);
        movementInput = movementInput.normalized;
        isMoving = movementInput.normalized != Vector2.zero;
    }

    private void Move() 
    {
        rb.MovePosition(rb.position + movementInput*speed*Time.smoothDeltaTime);
    }

    private void SwitchAnimation()
    {
        foreach (var part in animators)
        {
            part.SetBool("isMoving", isMoving);
            if (isMoving)
            {
                part.SetFloat("InputX", inputX);
                part.SetFloat("InputY", inputY);
            }


            
        }
    }

}
