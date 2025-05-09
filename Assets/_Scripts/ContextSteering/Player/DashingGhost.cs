using UnityEngine;

public class DashingGhost : MonoBehaviour
{
    private PlayerAgent player;

    [SerializeField] private float ghostDelay = 1f;
    private float ghostDelayTimer;
    [SerializeField] private GameObject ghostPrefab;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerAgent>();
        ghostDelayTimer = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDashing)
        {
            if (ghostDelayTimer > 0)
            {
                ghostDelayTimer -= Time.deltaTime;
            }
            else
            {
                // generate a ghost
                GameObject currentGhost = Instantiate(ghostPrefab, transform.position, Quaternion.identity);
                Sprite currentSprite = GetComponentInChildren<SpriteRenderer>().sprite;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                currentGhost.transform.localScale = transform.Find("Sprite").localScale;
                ghostDelayTimer = ghostDelay; // Reset the timer
                Destroy(currentGhost, 1f); // Destroy the ghost after 1 second
            }
        }
    }
}
