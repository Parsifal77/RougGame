using UnityEngine;
using UnityEngine.UI;

public class DashCooldownUI : MonoBehaviour
{
    [SerializeField]
    private PlayerAgent playerAgent;
    [SerializeField] private Image cooldownImage;

    private void Start()
    {
        // Automatically find the PlayerAgent in the parent
        playerAgent = GetComponentInParent<PlayerAgent>();
        if (playerAgent == null)
        {
            Debug.LogError("PlayerAgent not found in parent!");
        }

        // Ensure cooldownImage is assigned
        if (cooldownImage == null)
        {
            cooldownImage = GetComponent<Image>();
        }
    }

    private void Update()
    {
        if (playerAgent == null || cooldownImage == null) return;

        // Update the fill amount based on the cooldown
        float cooldownProgress = 1 - (playerAgent.CurrentDashCooldown / playerAgent.DashCooldown);
        cooldownImage.fillAmount = Mathf.Clamp01(cooldownProgress);
    }
}