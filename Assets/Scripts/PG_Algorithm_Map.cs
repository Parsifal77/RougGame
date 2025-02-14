using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PG_Algorithm_Map : AbstractDDungeonGenerator
{

    [SerializeField]
    private RandomWalkSO parametrs;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPosition = RunRandomWalk();
        tileMapVisualizer.Clear();
        tileMapVisualizer.PaintFloorTile(floorPosition);
    }

    protected HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPosition = startPosition;

        HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();

        for (int i = 0;i < parametrs.interations; i++)
        {
            var path = PG_Algorith.RandomWalk(currentPosition, parametrs.walklength);
            floorPosition.UnionWith(path);
            if(parametrs.StartRandomIteration)
            {
                currentPosition = floorPosition.ElementAt(Random.Range(0, floorPosition.Count));
            }
        }
        return floorPosition;
    }
}
