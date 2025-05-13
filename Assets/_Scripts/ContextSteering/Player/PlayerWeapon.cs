using System.Collections;
using UnityEngine;

public class PlayerWeapon : Weapon
{
    [SerializeField] private int attackDamage = 1;

    public int PlayerAttackDamage => attackDamage;

    public override void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            if (collider.isTrigger)
                continue;

            if (collider.TryGetComponent<Item>(out var item))
            {
                item.GetHit(attackDamage, transform.parent.gameObject);
            }
        }
    }

    internal void ResetDamageMultiplier(int baseAttackDamage)
    {
        attackDamage = baseAttackDamage;
    }

    internal void SetDamageMultiplier(float damageMultiplier, float multiplierDuration, int baseAttackDamage)
    {
        attackDamage = (int)(attackDamage * damageMultiplier);
        StartCoroutine(ResetDamageMultiplierAfterDelay(multiplierDuration, baseAttackDamage));
    }

    private IEnumerator ResetDamageMultiplierAfterDelay(float duration, int baseAttackDamage)
    {
        yield return new WaitForSeconds(duration);
        ResetDamageMultiplier(baseAttackDamage);
    }
}