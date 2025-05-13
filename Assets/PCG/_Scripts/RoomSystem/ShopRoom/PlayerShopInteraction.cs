using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShopInteraction : MonoBehaviour
{
    [SerializeField]
    private int playerCoinCount = 10; // Starting gold amount TO DO: retrieve from player data (BoosterHandler.cs)

    [SerializeField]
    private List<ShopItem> itemsInRange = new List<ShopItem>();

    [SerializeField]
    private KeyCode purchaseKey = KeyCode.B;

    private ShopItem currentSelectedItem = null;

    private BoostersHandler playerBoostersHandler;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerBoostersHandler = player.GetComponent<BoostersHandler>();
            if (playerBoostersHandler != null)
            {
                playerCoinCount = playerBoostersHandler.GetPlayerCoinCount;
                Debug.Log("Player has " + playerCoinCount + " gold.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found.");
        }
    }

    private void Start()
    {
        playerBoostersHandler.OnCoinCountChanged.AddListener(UpdateCoinCount);
    }

    private void UpdateCoinCount(int updatedCoinCount)
    {
        playerCoinCount = updatedCoinCount;
    }

    private void Update()
    {
        // Check if any items are in range
        if (itemsInRange.Count > 0)
        {
            // Select the first valid item
            currentSelectedItem = GetClosestItem();

            // Check for purchase input
            if (Input.GetKeyDown(purchaseKey) && currentSelectedItem != null)
            {
                TryPurchaseItem(currentSelectedItem);
            }
        }
        else
        {
            currentSelectedItem = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShopItem shopItem = collision.GetComponent<ShopItem>();
        if (shopItem != null && shopItem.CanBePurchased() && !itemsInRange.Contains(shopItem))
        {
            itemsInRange.Add(shopItem);

            // Highlight the item
            HighlightItem(shopItem, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ShopItem shopItem = collision.GetComponent<ShopItem>();
        if (shopItem != null)
        {
            itemsInRange.Remove(shopItem);

            // Remove highlight
            HighlightItem(shopItem, false);

            // deselect
            if (currentSelectedItem == shopItem)
            {
                currentSelectedItem = null;
            }
        }
    }

    private ShopItem GetClosestItem()
    {
        ShopItem closest = null;
        float closestDistance = float.MaxValue;

        foreach (ShopItem item in itemsInRange)
        {
            if (item.CanBePurchased())
            {
                float distance = Vector2.Distance(transform.position, item.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = item;
                }
            }
        }

        return closest;
    }

    private void TryPurchaseItem(ShopItem item)
    {
        int price = item.GetPrice();

        if (playerCoinCount >= price)
        {
            // Deduct gold
            playerCoinCount -= price;
            playerBoostersHandler.SetPlayerCoinCount = playerCoinCount;

            // Process purchase
            item.Purchase();

            // TO DO: Update player inventory here
            playerBoostersHandler.OnCoinCountChanged?.Invoke(playerCoinCount);

            // Remove the shop item from available items
            itemsInRange.Remove(item);

            // Destroy the object after a short delay (for effects/animations)
            Destroy(item.gameObject, 0.5f);

            Debug.Log($"Purchased item. Gold remaining: {playerCoinCount}");
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

    private void HighlightItem(ShopItem item, bool highlight)
    {
        // Visual feedback for highlighting: glow in this case
        SpriteRenderer renderer = item.GetComponentInChildren<SpriteRenderer>();
        if (renderer != null)
        {
            if (highlight)
            {
                renderer.color = new Color(1f, 1f, 1f, 1f); // Full brightness
            }
            else
            {
                renderer.color = new Color(0.7f, 0.7f, 0.7f, 1f); // Dimmed
            }
        }
    }
}