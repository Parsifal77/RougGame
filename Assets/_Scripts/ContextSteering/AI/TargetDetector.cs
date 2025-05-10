using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [SerializeField]
    private float targetDetectionRange = 5;

    [SerializeField]
    private LayerMask playerLayerMask;

    [SerializeField]
    private bool showGizmos = true;

    // Gizmo parameters
    private List<Transform> colliders;

    public override void Detect(AIData aiData)
    {
        // Initialize colliders list
        colliders = new List<Transform>();

        // Find player within detection range
        Collider2D playerCollider =
            Physics2D.OverlapCircle(transform.position, targetDetectionRange, playerLayerMask);

        if (playerCollider != null && playerCollider.gameObject.CompareTag("Player"))
        {
            colliders.Add(playerCollider.transform);
            //Debug.Log($"TargetDetector detected: {playerCollider.gameObject.name}");
        }

        aiData.targets = colliders;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);

        if (colliders == null || !Application.isPlaying)
            return;

        Gizmos.color = Color.magenta;
        foreach (var item in colliders)
        {
            Gizmos.DrawSphere(item.position, 0.3f);
        }
    }
}