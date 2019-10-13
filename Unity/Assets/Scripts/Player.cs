﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 14f;
    [SerializeField] float climbSpeed = 5f;

    Rigidbody2D playerRigidBody;
    Animator playerAnimator;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;

    bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
        Jump();
    }

    private void ClimbLadder()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            playerAnimator.SetBool("Climbing", false);
            return;
        }

        float inputAxis = Input.GetAxis("Vertical");
        Vector2 ladderVelocity = new Vector2(playerRigidBody.velocity.x, inputAxis * climbSpeed);
        playerRigidBody.velocity = ladderVelocity;

        bool hasVerticalSpeed = Mathf.Abs(playerRigidBody.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("Climbing", hasVerticalSpeed);
    }

    private void Jump()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return; // don't allow player to jump
        }

        if (Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocity = new Vector2(0f, jumpSpeed);
            playerRigidBody.velocity += jumpVelocity;
        }
    }

    private void Run()
    {
        float inputAxis = Input.GetAxis("Horizontal");
        Vector2 moveVelocity = new Vector2(inputAxis * moveSpeed, playerRigidBody.velocity.y);
        playerRigidBody.velocity = moveVelocity;

        bool hasHorizontalMovement = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("Running", hasHorizontalMovement);
    }

    private void FlipSprite()
    {
        bool hasHorizontalMovement = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;
        if (hasHorizontalMovement)
        {
            transform.localScale =  new Vector2 (Mathf.Sign(playerRigidBody.velocity.x), 1f);
        }
    }
}