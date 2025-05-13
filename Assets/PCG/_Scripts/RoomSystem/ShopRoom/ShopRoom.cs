using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopRoom : RoomGenerator
{
    [Header("Shop Components")]
    [SerializeField]
    private GameObject shopkeeperPrefab;

    [SerializeField]
    private List<ItemPlacementData> decorationItems;

    [Header("Booster Settings")]
    [SerializeField]
    private List<GameObject> boosterPrefabs;

    [SerializeField]
    private List<int> boosterPrices; // Prices corresponding to each booster prefab

    [SerializeField]
    private int numberOfBoostersToDisplay = 3;

    [SerializeField]
    private PrefabPlacer prefabPlacer;

    // Track placed boosters for interaction
    private List<GameObject> placedBoosters = new List<GameObject>();

    public override List<GameObject> ProcessRoom(
        Vector2Int roomCenter,
        HashSet<Vector2Int> roomFloor,
        HashSet<Vector2Int> roomFloorNoCorridors)
    {
        ItemPlacementHelper itemPlacementHelper =
            new ItemPlacementHelper(roomFloor, roomFloorNoCorridors);

        // Place decoration items
        List<GameObject> placedObjects =
            prefabPlacer.PlaceAllItems(decorationItems, itemPlacementHelper);

        // Place shopkeeper
        Vector2Int shopkeeperPosition = FindGoodPositionForShopkeeper(roomCenter, roomFloorNoCorridors);
        GameObject shopkeeperObject =
            prefabPlacer.CreateObject(shopkeeperPrefab, shopkeeperPosition + new Vector2(0.5f, 0.5f));

        // Add ShopkeeperController component if needed
        if (!shopkeeperObject.GetComponent<ShopkeeperController>())
        {
            shopkeeperObject.AddComponent<ShopkeeperController>();
        }

        placedObjects.Add(shopkeeperObject);

        // Place boosters
        placedBoosters = PlaceBoosters(itemPlacementHelper);
        placedObjects.AddRange(placedBoosters);

        return placedObjects;
    }

    private List<GameObject> PlaceBoosters(ItemPlacementHelper itemPlacementHelper)
    {
        List<GameObject> boosters = new List<GameObject>();

        // Select random boosters
        List<int> selectedBoosterIndices = SelectRandomBoosterIndices();

        // Define positions for boosters - maybe in a semicircle around the shopkeeper
        // For simplicity, we'll just use wall placements first
        foreach (int boosterIndex in selectedBoosterIndices)
        {
            // We need to know the size of the booster - assuming it's 1x1 for now
            // If your booster prefabs have different sizes, you might need to extract that information
            Vector2Int size = new Vector2Int(1, 1);

            Vector2? position = itemPlacementHelper.GetItemPlacementPosition(
                PlacementType.NearWall,
                100,
                size,
                false);

            if (position.HasValue)
            {
                // Create booster object
                GameObject boosterObject = prefabPlacer.CreateObject(
                    boosterPrefabs[boosterIndex],
                    position.Value + new Vector2(0.5f, 0.5f));

                // Add price component
                PriceDisplay priceDisplay = boosterObject.AddComponent<PriceDisplay>();
                priceDisplay.SetPrice(boosterPrices[boosterIndex]);

                // Add shop item component to handle purchase logic
                ShopItem shopItem = boosterObject.AddComponent<ShopItem>();
                shopItem.Initialize(boosterPrices[boosterIndex]);

                boosters.Add(boosterObject);
            }
        }

        return boosters;
    }

    private List<int> SelectRandomBoosterIndices()
    {
        // Create a list of indices
        List<int> indices = new List<int>();
        for (int i = 0; i < boosterPrefabs.Count; i++)
        {
            indices.Add(i);
        }

        // Shuffle the indices
        for (int i = 0; i < indices.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, indices.Count);
            int temp = indices[i];
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        // Take only the number we need
        int count = Mathf.Min(numberOfBoostersToDisplay, indices.Count);
        return indices.Take(count).ToList();
    }

    private Vector2Int FindGoodPositionForShopkeeper(Vector2Int roomCenter, HashSet<Vector2Int> floorPositions)
    {
        // Place the shopkeeper in the center
        return roomCenter;
    }
}