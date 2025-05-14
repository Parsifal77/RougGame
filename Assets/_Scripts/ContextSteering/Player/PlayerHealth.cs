using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    public UnityEvent<int, int> OnHealthChanged;

    public override void GetHit(int amount, GameObject sender)
    {
        // Store health before potential change
        int previousHealth = currentHealth;

        // Call base functionality (health reduction and death check)
        base.GetHit(amount, sender);

        // Only trigger event if health actually changed
        if (currentHealth != previousHealth)
        {
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }

    protected override void Die()
    {
        OnHealthChanged?.Invoke(0, maxHealth); // Notify UI about zero health
        Destroy(gameObject);
        SceneManager.LoadScene("DeathScene");
    }

    public void HealPlayer(int amount)
    {
        if (isDead) return;
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}