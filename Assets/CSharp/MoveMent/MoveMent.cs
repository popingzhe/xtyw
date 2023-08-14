using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    //动画使用工具
    private float mouseX;
    private float mouseY;
    private bool useTool;

    bool isMoving;
    Animator[] animators;
    //玩家输入控制
    bool inputDisable;
    private void OnEnable()
    {
        EventHander.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHander.AfterSceneloadEvent += AfterSceneloadEvent;
        EventHander.MoveToPosition += OnMoveToPosition;
        EventHander.MouseClickedEvent += OnMouseClickedEvent;

    }

    private void OnDisable()
    {
        EventHander.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHander.AfterSceneloadEvent -= AfterSceneloadEvent;
        EventHander.MoveToPosition -= OnMoveToPosition;
        EventHander.MouseClickedEvent -= OnMouseClickedEvent;
    }




    private void OnMouseClickedEvent(Vector3 mPos, ItemDetails details)
    {
        //TODO执行动画
        if(details.itemType != ItemType.Seed && details.itemType != ItemType.Commodity && details.itemType != ItemType.Furniture)
        {
            mouseX = mPos.x - transform.position.x;
            mouseY = mPos.y - transform.position.y;

            if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
            {
                mouseY = 0;
            }
            else
            {
                mouseX = 0;
            }
            StartCoroutine(UseToolRoutine(mPos, details));
        }
        else
        {
            EventHander.CallExecuteActionAfterAnimation(mPos, details);
        }


       
    }

    private IEnumerator UseToolRoutine(Vector3 mouseWorldPos,ItemDetails details)
    {
        useTool = true;
        inputDisable = true;
        yield return null;
        foreach (var anim in animators)
        {
            anim.SetTrigger("useTool");
            anim.SetFloat("InputX", mouseX);
            anim.SetFloat("InputY", mouseY);
        }
        //等待动画执行到砍下
        yield return new WaitForSeconds(0.45f);
        EventHander.CallExecuteActionAfterAnimation(mouseWorldPos, details);
        yield return new WaitForSeconds(0.25f);

        useTool = false;
        inputDisable = false;
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

            part.SetFloat("InputX", mouseX);
            part.SetFloat("InputY", mouseY);


            if (isMoving)
            {
                part.SetFloat("InputX", inputX);
                part.SetFloat("InputY", inputY);
            }


            
        }
    }

}
