using UnityEngine;
using System;
using UnityEditor; // Include System for System.Random

public class DefaultEnemyHealth : Health
{
    [Header("Booster Drops")]
    [SerializeField] private GameObject healthBoosterDrop;
    [SerializeField] private GameObject speedBoosterDrop;
    [SerializeField] private GameObject strengthBoosterDrop;
    [SerializeField] private GameObject coinDrop;
    [SerializeField] private float dropSpawnChance = 0.9f;

    private RoomContentGenerator roomContentGenerator;

    private System.Random random;

    protected void Start()
    {
        // Seed each enemy's random with their unique instance ID
        random = new System.Random(GetInstanceID());

        roomContentGenerator = GameObject.Find("RoomContentGenerator").GetComponent<RoomContentGenerator>();

        OnDeathWithReference.AddListener((GameObject sender) => {
            roomContentGenerator.enemiesCount--;
            Debug.Log("Enemies count: " + roomContentGenerator.enemiesCount);
        });
    }

    protected override void Die()
    {
        double randomValue = random.NextDouble();
        if (randomValue < dropSpawnChance)
        {
            GameObject dropToSpawn = coinDrop;
            if (dropToSpawn != null)
            {
                Instantiate(dropToSpawn, transform.position, Quaternion.identity);
            }
        }
        else
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


    public override void GetHit(int amount, GameObject sender)
    {
        if (isDead)
            return;
        if (sender.layer == gameObject.layer)
            return;

        currentHealth -= amount;

        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
            isDead = true;
            Die(); // Call the abstract Die method
        }

    }
}