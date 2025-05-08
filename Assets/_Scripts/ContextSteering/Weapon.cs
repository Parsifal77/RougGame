using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public SpriteRenderer characterRenderer, weaponRenderer;
    public Vector2 PointerPosition { get; set; }

    public Animator animator;
    public float delay = 0.3f;
    private bool attackBlocked;

    public bool IsAttacking { get; protected set; }

    public Transform circleOrigin;
    public float radius;

    //protected virtual void Awake()
    //{
    //    weaponRenderer = GetComponent<SpriteRenderer>();
    //    characterRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
    //    animator = GetComponent<Animator>();
    //}

    protected virtual void Update()
    {
        if (IsAttacking)
            return;
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        scale.y = direction.x < 0 ? -1 : 1;
        transform.localScale = scale;

        weaponRenderer.sortingOrder = transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180
            ? characterRenderer.sortingOrder - 1
            : characterRenderer.sortingOrder + 1;
    }

    public void Attack()
    {
        if (attackBlocked)
            return;
        animator.SetTrigger("Attack");
        IsAttacking = true;
        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    private System.Collections.IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            if (collider.isTrigger)
                continue;
            if (collider.TryGetComponent<Health>(out var health))
            {
                health.GetHit(1, transform.parent.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }
}