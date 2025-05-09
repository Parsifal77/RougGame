public class DefaultEnemyHealth : Health
{
    protected override void Die()
    {
        Destroy(gameObject);
    }
}