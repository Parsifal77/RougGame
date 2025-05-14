using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour//, IDamageable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private BoxCollider2D itemCollider;

    [SerializeField]
    private int health = 3;
    [SerializeField]
    private bool nonDestructible;

    [SerializeField]
    private GameObject hitFeedback, destoyFeedback; // Note: Fix typo to "destroyFeedback" if desired

    private bool isDestroyed = false;

    public UnityEvent OnGetHit => throw new System.NotImplementedException(); // Placeholder

    public void Initialize(ItemData itemData)
    {
        spriteRenderer.sprite = itemData.sprite;
        spriteRenderer.transform.localPosition = new Vector2(0.5f * itemData.size.x, 0.5f * itemData.size.y);
        itemCollider.size = itemData.size;
        itemCollider.offset = spriteRenderer.transform.localPosition;

        if (itemData.nonDestructible)
            nonDestructible = true;

        this.health = itemData.health;
    }

    public void GetHit(int damage, GameObject damageDealer)
    {
        if (nonDestructible || isDestroyed)
            return;

        health -= damage;

        if (health > 0)
        {
            Instantiate(hitFeedback, spriteRenderer.transform.position, Quaternion.identity);
            spriteRenderer.transform.DOShakePosition(0.2f, 0.3f, 75, 1, false, true);
        }
        else
        {
            isDestroyed = true;
            Instantiate(destoyFeedback, spriteRenderer.transform.position, Quaternion.identity);
            spriteRenderer.transform.DOShakePosition(0.2f, 0.3f, 75, 1, false, true)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}