using UnityEngine;

public class PriceDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject priceTextPrefab; // TextMesh prefab: for customizing the text appearance

    private TextMesh priceTextMesh;
    private int price;

    public void SetPrice(int price)
    {
        this.price = price;
        CreatePriceText();
        UpdatePriceDisplay();
    }

    private void CreatePriceText()
    {
        GameObject textObject = new GameObject("PriceText");
        textObject.transform.SetParent(transform);
        textObject.transform.localPosition = new Vector3(0, 0.7f, 0); // Position above the shop item

        priceTextMesh = textObject.AddComponent<TextMesh>();
        priceTextMesh.alignment = TextAlignment.Center;
        priceTextMesh.anchor = TextAnchor.LowerCenter;
        priceTextMesh.fontSize = 32;
        priceTextMesh.characterSize = 0.1f;
        priceTextMesh.color = Color.yellow;
    }

    private void UpdatePriceDisplay()
    {
        priceTextMesh.text = price.ToString() + " G";
    }

    // Make the price text always face the camera
    private void LateUpdate()
    {
        if (priceTextMesh != null && Camera.main != null)
        {
            priceTextMesh.transform.forward = Camera.main.transform.forward;
        }
    }
}