using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class RoomContentGenerator : MonoBehaviour
{
    [SerializeField]
    private RoomGenerator playerRoom, defaultRoom;

    [SerializeField]
    private ShopRoom shopRoom;

    List<GameObject> spawnedObjects = new List<GameObject>();

    [SerializeField]
    private GraphTest graphTest;

    public Transform itemParent;

    [SerializeField]
    private CinemachineVirtualCamera cinemachineCamera;

    public UnityEvent RegenerateDungeon;

    public UnityEvent DungeonContentGenerated;

    private bool shopRoomGenerated = false; // Track if the shop room was generated


    public int enemiesCount = int.MaxValue;


    private void Start()
    {
        DungeonContentGenerated.AddListener(() =>
        {
            // Get enemies count
            enemiesCount = spawnedObjects.Count(item => item.CompareTag("Enemy"));
            Debug.Log($"Enemies count: {enemiesCount}");
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            foreach (var item in spawnedObjects)
            {
                Destroy(item);
            }
            RegenerateDungeon?.Invoke();
        }
    }

    public void GenerateRoomContent(DungeonData dungeonData)
    {
        foreach (GameObject item in spawnedObjects)
        {
            DestroyImmediate(item);
        }
        spawnedObjects.Clear();

        // Reset shop room flag
        shopRoomGenerated = false;

        SelectPlayerSpawnPoint(dungeonData);

        // Add shop room into the generation
        SelectShopRoom(dungeonData);

        SelectEnemySpawnPoints(dungeonData);

        foreach (GameObject item in spawnedObjects)
        {
            if (item != null)
                item.transform.SetParent(itemParent, false);
        }

        DungeonContentGenerated?.Invoke();
    }

    private void SelectPlayerSpawnPoint(DungeonData dungeonData)
    {
        int randomRoomIndex = UnityEngine.Random.Range(0, dungeonData.roomsDictionary.Count);
        Vector2Int playerSpawnPoint = dungeonData.roomsDictionary.Keys.ElementAt(randomRoomIndex);

        graphTest.RunDijkstraAlgorithm(playerSpawnPoint, dungeonData.floorPositions);

        Vector2Int roomIndex = dungeonData.roomsDictionary.Keys.ElementAt(randomRoomIndex);

        List<GameObject> placedPrefabs = playerRoom.ProcessRoom(
            playerSpawnPoint,
            dungeonData.roomsDictionary.Values.ElementAt(randomRoomIndex),
            dungeonData.GetRoomFloorWithoutCorridors(roomIndex)
            );

        FocusCameraOnThePlayer(placedPrefabs[placedPrefabs.Count - 1].transform);

        spawnedObjects.AddRange(placedPrefabs);

        dungeonData.roomsDictionary.Remove(playerSpawnPoint);
    }

    private void SelectShopRoom(DungeonData dungeonData)
    {
        // Only add a shop room if there are rooms left and it is not already generated
        if (!shopRoomGenerated && dungeonData.roomsDictionary.Count > 0 && shopRoom != null)
        {
            int shopRoomIndex = UnityEngine.Random.Range(0, dungeonData.roomsDictionary.Count);
            Vector2Int shopRoomPosition = dungeonData.roomsDictionary.Keys.ElementAt(shopRoomIndex);

            List<GameObject> shopObjects = shopRoom.ProcessRoom(
                shopRoomPosition,
                dungeonData.roomsDictionary.Values.ElementAt(shopRoomIndex),
                dungeonData.GetRoomFloorWithoutCorridors(shopRoomPosition)
            );

            spawnedObjects.AddRange(shopObjects);

            // Remove the room from available rooms
            dungeonData.roomsDictionary.Remove(shopRoomPosition);
            shopRoomGenerated = true;
        }
    }

    private void FocusCameraOnThePlayer(Transform playerTransform)
    {
        cinemachineCamera.LookAt = playerTransform;
        cinemachineCamera.Follow = playerTransform;
    }

    private void SelectEnemySpawnPoints(DungeonData dungeonData)
    {
        foreach (KeyValuePair<Vector2Int, HashSet<Vector2Int>> roomData in dungeonData.roomsDictionary)
        {
            spawnedObjects.AddRange(
                defaultRoom.ProcessRoom(
                    roomData.Key,
                    roomData.Value,
                    dungeonData.GetRoomFloorWithoutCorridors(roomData.Key)
                    )
            );
        }
    }
}