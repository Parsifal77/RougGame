using System.Collections;
using UnityEngine;

public class PlayerWeapon : Weapon
{
    // Inherits all behavior from Weapon; can override for player-specific changes

    [SerializeField] private int attackDamage = 1;

    public int PlayerAttackDamage => attackDamage;

    public override void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            if (collider.isTrigger)
                continue;
            if (collider.TryGetComponent<Health>(out var health))
            {
                health.GetHit(attackDamage, transform.parent.gameObject);
            }
        }
    }

    internal void ResetDamageMultiplier(int baseAttackDamage)
    {
        attackDamage = baseAttackDamage;
        //Debug.Log("Damage multiplier reset to base value.");
    }

    internal void SetDamageMultiplier(float damageMultiplier, float multiplierDuration, int baseAttackDamage)
    {
        attackDamage = (int)(attackDamage * damageMultiplier);
        //Debug.Log($"Damage multiplier set to {attackDamage} for {multiplierDuration} seconds.");

        StartCoroutine(ResetDamageMultiplierAfterDelay(multiplierDuration, baseAttackDamage));
    }

    private IEnumerator ResetDamageMultiplierAfterDelay(float duration, int baseAttackDamage)
    {
        yield return new WaitForSeconds(duration);
        ResetDamageMultiplier(baseAttackDamage);
    }
}