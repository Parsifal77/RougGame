using UnityEngine;

public class DashStunEffect : MonoBehaviour
{
    [SerializeField] private float stunDuration = 0.5f;
    private DefaultEnemyAgent enemyAgent;

    private void Start()
    {
        enemyAgent = GetComponent<DefaultEnemyAgent>();
        PlayerAgent.OnDashStarted += HandleDash;
    }

    private void HandleDash()
    {
        if (enemyAgent != null)
            StartCoroutine(StunEnemy());
    }

    private System.Collections.IEnumerator StunEnemy()
    {
        enemyAgent.IsStunned = true;
        yield return new WaitForSeconds(stunDuration);
        enemyAgent.IsStunned = false;
    }

    private void OnDestroy()
    {
        PlayerAgent.OnDashStarted -= HandleDash;
    }
}