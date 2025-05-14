using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    [SerializeField] private Image filledHeart;
    [SerializeField] private Image outlinedHeart;

    private PlayerHealth playerHealth;

    private void Awake()
    {
        RoomContentGenerator roomGenerator = FindObjectOfType<RoomContentGenerator>();
        if (roomGenerator != null)
        {
            roomGenerator.DungeonContentGenerated.AddListener(OnDungeonContentGenerated);
        }
        else
        {
            Debug.LogError("RoomContentGenerator not found in scene.");
        }
    }

    private void OnDungeonContentGenerated()
    {
        // Unsubscribe from previous player's health changes
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged.RemoveListener(UpdateHealthDisplay);
            playerHealth = null;
        }

        GameObject player = GameObject.FindWithTag("Player");

        Debug.Log("Player found: " + player);

        if (player != null && player.TryGetComponent(out PlayerHealth health))
        {
            playerHealth = health;
            playerHealth.OnHealthChanged.AddListener(UpdateHealthDisplay);
            UpdateHealthDisplay(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        }
        else
        {
            Debug.LogError("PlayerHealth component not found on the Player GameObject after dungeon generation.");
        }
    }

    private void UpdateHealthDisplay(int currentHealth, int maxHealth)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < maxHealth; i++)
        {
            Image heartImage = Instantiate(i < currentHealth ? filledHeart : outlinedHeart, transform);
            heartImage.transform.localScale = Vector3.one;
            heartImage.transform.localPosition = new Vector3(i * 1.5f, 0, 0);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        RoomContentGenerator roomGenerator = FindObjectOfType<RoomContentGenerator>();
        if (roomGenerator != null)
        {
            roomGenerator.DungeonContentGenerated.RemoveListener(OnDungeonContentGenerated);
        }

        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged.RemoveListener(UpdateHealthDisplay);
        }
    }
}