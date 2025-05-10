using System.Collections;
using UnityEngine;

public class AgentMover : MonoBehaviour
{
    private Rigidbody2D rb2d;
    [SerializeField] private float maxSpeed = 2, acceleration = 50, deacceleration = 100;
    [SerializeField] private float currentSpeed = 0;
    private Vector2 oldMovementInput;
    public Vector2 MovementInput { get; set; }
    public Vector2 LastMovementDirection => oldMovementInput.normalized;

    public bool IsDashing { get; set; }
    public Vector2 DashVelocity { get; set; }

    public float MaxSpeed => maxSpeed;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (IsDashing)
        {
            rb2d.velocity = DashVelocity;
        }
        else
        {
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
    }

    public void SetVelocityDirectly(Vector2 velocity)
    {
        rb2d.velocity = velocity;
    }

    public void SetSpeedMultiplier(float speedBoosterMultiplier, float speedBoosterDuration, float baseMaxSpeed)
    {
        this.maxSpeed = maxSpeed * speedBoosterMultiplier;
        //Debug.Log($"Speed multiplier set to {this.maxSpeed} for {speedBoosterDuration} seconds.");
        StartCoroutine(ResetSpeedMultiplierAfterDelay(speedBoosterDuration, baseMaxSpeed));
    }

    private IEnumerator ResetSpeedMultiplierAfterDelay(float speedBoosterDuration, float baseMaxSpeed)
    {
        yield return new WaitForSeconds(speedBoosterDuration);
        maxSpeed = baseMaxSpeed;
        //Debug.Log("Speed multiplier reset to base value.");
    }
}