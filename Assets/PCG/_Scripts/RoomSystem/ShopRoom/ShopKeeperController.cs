using UnityEngine;

public class ShopkeeperController : MonoBehaviour
{
    [SerializeField]
    private string shopkeeperMessage = "Welcome to my shop! Press B near an item to buy it.";
    // This could be expanded with animation, dialog


    private Canvas canvas;

    private void Awake()
    {
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // welcome message
            Debug.Log(shopkeeperMessage);

            // TO DO: UI dialog 
            canvas.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // hide message
            Debug.Log("Goodbye!");
            // TO DO: UI dialog box
            canvas.gameObject.SetActive(false);
        }
    }
}