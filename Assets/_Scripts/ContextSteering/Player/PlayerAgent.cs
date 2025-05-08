using UnityEngine;

public class PlayerAgent : Agent
{
    private PlayerInput playerInput;

    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>(); // Assumes PlayerInput is on the same GameObject
        if (playerInput != null)
        {
            playerInput.OnDash.AddListener(Dash);
            playerInput.OnAttack.AddListener(PerformAttack);
            playerInput.OnMovementInput.AddListener(input => MovementInput = input);
            playerInput.OnPointerInput.AddListener(input => PointerInput = input);
        }
    }

    public void Dash()
    {
        agentMover.Dash();
    }
}