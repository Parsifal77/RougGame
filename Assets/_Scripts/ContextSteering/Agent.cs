using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    protected AgentAnimations agentAnimations;
    protected AgentMover agentMover;
    protected Weapon weapon;

    protected Vector2 pointerInput, movementInput;

    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

    protected virtual void Awake()
    {
        agentAnimations = GetComponentInChildren<AgentAnimations>();
        weapon = GetComponentInChildren<Weapon>();
        agentMover = GetComponent<AgentMover>();
    }

    protected virtual void Update()
    {
        agentMover.MovementInput = MovementInput;
        weapon.PointerPosition = PointerInput;
        AnimateCharacter();
    }

    public void PerformAttack()
    {
        weapon.Attack();
    }

    protected void AnimateCharacter()
    {
        Vector2 lookDirection = PointerInput - (Vector2)transform.position;
        agentAnimations.RotateToPointer(lookDirection);
        agentAnimations.PlayAnimation(MovementInput);
    }
}