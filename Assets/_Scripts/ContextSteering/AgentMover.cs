using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMover : MonoBehaviour
{
    private Rigidbody2D rb2d;

    [SerializeField]
    private float maxSpeed = 2, acceleration = 50, deacceleration = 100;
    [SerializeField]
    private float currentSpeed = 0;
    private Vector2 oldMovementInput;
    public Vector2 MovementInput { get; set; }



    [SerializeField] private float dashSpeed = 8f;
    [SerializeField] private float dashDuration = 0.2f;
    private bool isDashing = false;
    private float dashTime;

    [SerializeField] private ParticleSystem dashParticle;
    //private int playerLayer;
    //private int dashingLayer;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        // Cache layer indices for efficiency
        //playerLayer = LayerMask.NameToLayer("Player");
        //dashingLayer = LayerMask.NameToLayer("Dashing");

    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                isDashing = false;
                // Switch back to normal layer when dash ends
                //gameObject.layer = playerLayer;

            }
            rb2d.velocity = oldMovementInput.normalized * dashSpeed;
            return;
        }

        if (MovementInput.magnitude > 0 && currentSpeed >= 0)
        {
            oldMovementInput = MovementInput;
            currentSpeed += acceleration * maxSpeed * Time.deltaTime;
        }
        else
        {
            currentSpeed -= deacceleration * maxSpeed * Time.deltaTime;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        rb2d.velocity = oldMovementInput * currentSpeed;

    }

    public void Dash()
    {
        if (!isDashing)
        {
            isDashing = true;
            dashTime = dashDuration;
            currentSpeed = dashSpeed;

            // switch to the Dashing layer
            //gameObject.layer = dashingLayer;


            // trigger particle effect
            if (dashParticle != null)
                dashParticle.Play();
        }
    }


}
