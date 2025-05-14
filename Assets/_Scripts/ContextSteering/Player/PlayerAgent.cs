using UnityEngine;

public class PlayerAgent : Agent
{
    private PlayerInput playerInput;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 8f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private ParticleSystem dashParticle;

    //private float currentDashCooldown;
    public bool isDashing;
    private float dashTimeLeft;

    public static System.Action OnDashStarted;

    public float CurrentDashCooldown { get; private set; }
    public float DashCooldown => dashCooldown;



    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();

        if (playerInput != null)
        {
            playerInput.OnDash.AddListener(HandleDash);
            playerInput.OnAttack.AddListener(PerformAttack);
            playerInput.OnMovementInput.AddListener(input => MovementInput = input);
            playerInput.OnPointerInput.AddListener(input => PointerInput = input);
        }
    }

    protected override void Update()
    {
        base.Update();
        UpdateCooldown();
        UpdateDash();
    }

    private void UpdateCooldown()
    {
        if (CurrentDashCooldown > 0)
        {
            CurrentDashCooldown -= Time.deltaTime;
        }
    }

    private void UpdateDash()
    {
        if (!isDashing) return;

        dashTimeLeft -= Time.deltaTime;
        if (dashTimeLeft <= 0)
        {
            EndDash();
        }
    }

    private void HandleDash()
    {
        if (CurrentDashCooldown > 0 || isDashing) return;

        StartDash();

    }

    private void StartDash()
    {
        OnDashStarted?.Invoke();
        isDashing = true;
        dashTimeLeft = dashDuration;
        CurrentDashCooldown = dashCooldown;

        gameObject.layer = LayerMask.NameToLayer("PlayerDashing");

        Vector2 dashDirection = agentMover.LastMovementDirection;
        Vector2 dashVel = dashDirection * dashSpeed;
        agentMover.IsDashing = true;
        agentMover.DashVelocity = dashVel;
        agentMover.SetVelocityDirectly(dashVel);

        if (dashParticle != null)
            dashParticle.Play();
    }

    private void EndDash()
    {
        isDashing = false;
        agentMover.IsDashing = false;
        gameObject.layer = LayerMask.NameToLayer("Player");

        if (dashParticle != null)
            dashParticle.Stop();
    }
}