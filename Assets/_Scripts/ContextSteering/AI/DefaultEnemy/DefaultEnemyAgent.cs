using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DefaultEnemyAgent : Agent
{
    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;

    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    private AIData aiData;

    [SerializeField]
    private float aiUpdateDelay = 0.06f, attackDelay = 1f;

    [SerializeField]
    private float detectionDelay = 0.05f;

    [SerializeField]
    private float attackDistance = 0.5f;

    [SerializeField]
    private float detectionRadius = 5f;

    [SerializeField]
    private ContextSolver movementDirectionSolver;

    private Health health;
    private KnockbackFeedback knockbackFeedback;
    private GameObject player;
    private bool following = false;

    public bool IsStunned { get; set; }

    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
        health = GetComponent<Health>();
        knockbackFeedback = GetComponent<KnockbackFeedback>();

        // Subscribe to the OnHitWithReference event
        if (health != null && knockbackFeedback != null)
        {
            health.OnHitWithReference.AddListener(knockbackFeedback.PlayFeedback);
        }
        else
        {
            Debug.LogWarning("Health or KnockbackFeedback component missing on " + gameObject.name);
        }
    }

    private void Start()
    {
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    protected override void Update()
    {
        if (IsStunned)
        {
            agentMover.MovementInput = Vector2.zero; // Stop moving when stunned
            return;
        }

        // Perform detection every frame
        //PerformDetection();

        // Enemy AI movement based on target availability
        if (aiData.currentTarget != null && aiData.currentTarget.gameObject.CompareTag("Player"))
        {
            // Look at the target
            OnPointerInput?.Invoke(aiData.currentTarget.position);
            PointerInput = aiData.currentTarget.position;

            if (!following)
            {
                following = true;
                StartCoroutine(ChaseAndAttack());
            }
        }
        else if (aiData.GetTargetsCount() > 0 && aiData.targets[0].gameObject.CompareTag("Player"))
        {
            // Target acquisition logic, ensuring it's the player
            aiData.currentTarget = aiData.targets[0];
            //Debug.Log($"Target acquired: {aiData.currentTarget.gameObject.name}");
        }
        else
        {
            // No valid player target
            aiData.currentTarget = null;
            MovementInput = Vector2.zero;
            following = false;
        }

        // Update movement input
        OnMovementInput?.Invoke(MovementInput);
        base.Update();
    }

    private void PerformDetection()
    {
        if (aiData.targets == null)
        {
            aiData.targets = new List<Transform>();
        }
        else
        {
            aiData.targets.Clear(); // Clear existing targets to avoid stale data
        }

        foreach (Detector detector in detectors)
        {
            detector.Detect(aiData);
        }

        // Fallback: Ensure player is detected if within radius
        if (player != null && Vector2.Distance(transform.position, player.transform.position) <= detectionRadius)
        {
            if (!aiData.targets.Contains(player.transform) && player.CompareTag("Player"))
            {
                aiData.targets.Add(player.transform);
                //Debug.Log($"Player detected via fallback: {player.name}");
            }
        }

        // Remove non-player targets
        if (aiData.targets != null)
        {
            aiData.targets.RemoveAll(target => target == null || !target.gameObject.CompareTag("Player"));
        }

        //if (aiData.targets.Count > 0)
        //{
        //    Debug.Log($"Detected targets: {string.Join(", ", aiData.targets.Select(t => t.gameObject.name))}");
        //}
    }

    private IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null || !aiData.currentTarget.gameObject.CompareTag("Player"))
        {
            // Stopping logic
            //Debug.Log("Stopping: No valid player target");
            MovementInput = Vector2.zero;
            following = false;
            yield break;
        }

        float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);

        if (distance < attackDistance)
        {
            // Attack logic
            MovementInput = Vector2.zero;
            OnAttackPressed?.Invoke();
            PerformAttack();
            yield return new WaitForSeconds(attackDelay);
            StartCoroutine(ChaseAndAttack());
        }
        else
        {
            // Chase logic
            MovementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
            //if (MovementInput.magnitude < 0.1f)
            //{
            //    Debug.Log("No movement: Steering direction too weak");
            //}
            yield return new WaitForSeconds(aiUpdateDelay);
            StartCoroutine(ChaseAndAttack());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}