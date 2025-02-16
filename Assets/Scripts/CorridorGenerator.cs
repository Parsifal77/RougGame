using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorGenerator : PG_Algorithm_Map
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent;
    [SerializeField]
    public RandomWalkSO roomParametrs;

    protected override void RunProceduralGeneration()
    {
        CorridorGeneration();
    }

    private void CorridorGeneration()
    {
        HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();

        CreateCorridors(floorPosition);

        tileMapVisualizer.PaintFloorTile(floorPosition);
        WallGenerator.CreateWalls(floorPosition, tileMapVisualizer);
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPosition)
    {
        var currentPosition = startPosition;

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = PG_Algorith.RandomWalkCorridor(currentPosition, corridorLength);
            currentPosition = corridor[corridor.Count - 1];
            floorPosition.UnionWith(corridor);
        }
    }
}
