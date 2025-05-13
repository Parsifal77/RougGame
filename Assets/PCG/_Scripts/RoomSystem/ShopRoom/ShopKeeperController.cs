using UnityEngine;

public class ShopkeeperController : MonoBehaviour
{
    [SerializeField]
    private string shopkeeperMessage = "Welcome to my shop! Press B near an item to buy it.";

    // This could be expanded with animation, dialog

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // welcome message
            Debug.Log(shopkeeperMessage);

            // TO DO: UI dialog box
        }
    }
}