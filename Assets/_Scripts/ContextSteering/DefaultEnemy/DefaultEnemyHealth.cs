using UnityEngine;
using System; // Include System for System.Random

public class DefaultEnemyHealth : Health
{
    [Header("Booster Drops")]
    [SerializeField] private GameObject healthBoosterDrop;
    [SerializeField] private GameObject speedBoosterDrop;
    [SerializeField] private GameObject strengthBoosterDrop;
    [SerializeField] private float dropSpawnChance = 0.1f;

    private System.Random random;

    protected void Start()
    {
        // Seed each enemy's random with their unique instance ID
        random = new System.Random(GetInstanceID());
    }

    protected override void Die()
    {
        double randomValue = random.NextDouble();
        if (randomValue < dropSpawnChance)
        {
            int randomDrop = random.Next(0, 3); // 0, 1, or 2
            GameObject dropToSpawn = GetDropPrefab(randomDrop);
            if (dropToSpawn != null)
            {
                Instantiate(dropToSpawn, transform.position, Quaternion.identity);
            }
        }
        Destroy(gameObject);
    }

    private GameObject GetDropPrefab(int dropIndex)
    {
        switch (dropIndex)
        {
            case 0: return healthBoosterDrop;
            case 1: return speedBoosterDrop;
            case 2: return strengthBoosterDrop;
            default: return null;
        }
    }
}