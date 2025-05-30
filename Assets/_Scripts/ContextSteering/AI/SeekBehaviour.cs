using System.Linq;
using UnityEngine;

public class SeekBehaviour : SteeringBehaviour
{
    [SerializeField]
    private float targetRechedThreshold = 0.5f;

    [SerializeField]
    private bool showGizmo = true;

    bool reachedLastTarget = true;

    //gizmo parameters
    private Vector2 targetPositionCached;
    private float[] interestsTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        // If we don't have a target, stop seeking
        // Else set a new target
        if (reachedLastTarget)
        {
            if (aiData.targets == null || aiData.targets.Count <= 0 || !aiData.targets.Any(target => target != null && target.gameObject.CompareTag("Player")))
            {
                aiData.currentTarget = null;
                reachedLastTarget = true;
                return (danger, interest);
            }
            else
            {
                reachedLastTarget = false;
                aiData.currentTarget = aiData.targets
                    .Where(target => target != null && target.gameObject.CompareTag("Player"))
                    .OrderBy(target => Vector2.Distance(target.position, transform.position))
                    .FirstOrDefault();
            }
        }

        // Cache the last position only if we still see the target
        if (aiData.currentTarget != null && aiData.targets != null && aiData.targets.Contains(aiData.currentTarget) && aiData.currentTarget.gameObject.CompareTag("Player"))
        {
            targetPositionCached = aiData.currentTarget.position;
        }
        else
        {
            reachedLastTarget = true;
            aiData.currentTarget = null;
            return (danger, interest);
        }

        // Check if we have reached the target
        if (Vector2.Distance(transform.position, targetPositionCached) < targetRechedThreshold)
        {
            reachedLastTarget = true;
            aiData.currentTarget = null;
            return (danger, interest);
        }

        // Compute interest directions toward the target
        Vector2 directionToTarget = (targetPositionCached - (Vector2)transform.position);
        for (int i = 0; i < interest.Length; i++)
        {
            float result = Vector2.Dot(directionToTarget.normalized, Directions.eightDirections[i]);

            // Accept only directions at less than 90 degrees to the target direction
            if (result > 0)
            {
                float valueToPutIn = result;
                if (valueToPutIn > interest[i])
                {
                    interest[i] = valueToPutIn;
                }
            }
        }
        interestsTemp = interest;
        return (danger, interest);
    }

    private void OnDrawGizmos()
    {
        if (showGizmo == false)
            return;
        Gizmos.DrawSphere(targetPositionCached, 0.2f);

        if (Application.isPlaying && interestsTemp != null)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < interestsTemp.Length; i++)
            {
                Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestsTemp[i]);
            }
            if (reachedLastTarget == false)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(targetPositionCached, 0.1f);
            }
        }
    }
}