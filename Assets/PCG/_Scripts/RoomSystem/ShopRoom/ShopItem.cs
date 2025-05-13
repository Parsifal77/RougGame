using UnityEngine;
using UnityEngine.Events;

public class ShopItem : MonoBehaviour
{
    [SerializeField]
    private int price;

    public UnityEvent OnPurchased;

    private bool canBePurchased = true;

    public void Initialize(int itemPrice)
    {
        price = itemPrice;

        if (OnPurchased == null)
            OnPurchased = new UnityEvent();
    }

    public int GetPrice()
    {
        return price;
    }

    public void Purchase()
    {
        if (canBePurchased)
        {
            canBePurchased = false;
            OnPurchased?.Invoke();

            // Note: The actual destruction of the object should happen elsewhere
            // For example, in a player interaction script after processing the purchase
        }
    }

    public bool CanBePurchased()
    {
        return canBePurchased;
    }
}