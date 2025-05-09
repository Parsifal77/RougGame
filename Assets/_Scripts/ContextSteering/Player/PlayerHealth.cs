using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    protected override void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("DeathScene");
    }
}