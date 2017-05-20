using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInput : MonoBehaviour {

    public float speed;             //Floating point variable to store the player's movement speed.
    public float jumpHeight = 5;
    public Vector2 vel;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRender;
    private bool doJump = false;
    private bool isCrouching = false;
    private float pitch = 1;
    public bool selected = true;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (!selected){
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!selected && doJump)
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }
        float moveHorizontal = 0;
        float moveVertical = 0;
        if (Input.GetKey("q")) {
            moveHorizontal = -1;
            changeFacingDirection(moveHorizontal);
        }
        if (Input.GetKey("d")) {
            moveHorizontal = 1;
            changeFacingDirection(moveHorizontal);
        }
        if (Input.GetButton("Jump") && doJump == true) {
            moveVertical = jumpHeight;
            doJump = false;
        }
        if (Input.GetKey("s"))
            crouch();
        else if (Input.GetKeyUp("s"))
            crouch(false);
        moveHorizontal *= speed;
        moveVertical += rb2d.velocity.y;
        vel = new Vector2(moveHorizontal, moveVertical);
        changeFacingDirection(vel.x);
        rb2d.velocity = vel;
        if (rb2d.velocity.y < 0.001f && rb2d.velocity.y > -0.001f)
            doJump = true;
    }

    private void crouch(bool crouching = true)
    {
        if (crouching && !isCrouching)
        {
            isCrouching = true;
            transform.localScale = new Vector3(1f, .5f, 1f);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.6275f, transform.localPosition.z);
        }
        else if (!crouching && isCrouching)
        {
            isCrouching = false;
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.6275f, transform.localPosition.z);
        }
    }

    private void changeFacingDirection(float dir)
    {
        if (Mathf.Sign(dir) == -1 && Mathf.Sign(pitch) == 1) {
            pitch = -pitch;
            spriteRender.flipX = true;
        }
        else if (Mathf.Sign(dir) == 1 && Mathf.Sign(pitch) == -1) {
            pitch = -pitch;
            spriteRender.flipX = false;
        }
    }
}