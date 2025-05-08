using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private AgentAnimations agentAnimations;
    private AgentMover agentMover;

    private WeaponParent weaponParent;

    private Vector2 pointerInput, movementInput;

    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

    private PlayerInput playerInput;

    private void Update()
    {
        agentMover.MovementInput = MovementInput;
        weaponParent.PointerPosition = pointerInput;
        AnimateCharacter();
    }

    public void Dash()
    {
        agentMover.Dash();
    }

    public void PerformAttack()
    {
        weaponParent.Attack();
    }

    private void Awake()
    {
        agentAnimations = GetComponentInChildren<AgentAnimations>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        agentMover = GetComponent<AgentMover>();

        // only subscribe dash for the player
        if (gameObject.CompareTag("Player"))
        {
            playerInput = FindObjectOfType<PlayerInput>();
            if (playerInput != null)
                playerInput.OnDash.AddListener(Dash);
        }
        
    }

    private void AnimateCharacter()
    {
        Vector2 lookDirection = pointerInput - (Vector2)transform.position;
        agentAnimations.RotateToPointer(lookDirection);
        agentAnimations.PlayAnimation(MovementInput);
    }

}
