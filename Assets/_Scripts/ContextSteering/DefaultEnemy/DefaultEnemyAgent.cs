using UnityEngine;

public class DefaultEnemyAgent : Agent
{
    [SerializeField]
    private float detectionRadius = 5f;

    private GameObject player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < detectionRadius)
            {
                Vector2 direction = (player.transform.position - transform.position).normalized;
                MovementInput = direction;
                PointerInput = player.transform.position;

                if (distanceToPlayer < 1f)
                {
                    PerformAttack();
                }
            }
            else
            {
                MovementInput = Vector2.zero; // Stop moving when player is out of range
            }
        }
        base.Update();
    }

    // Visualize detection radius in the Unity Editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}