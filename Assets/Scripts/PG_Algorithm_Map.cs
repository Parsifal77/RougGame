using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PG_Algorithm_Map : AbstractDDungeonGenerator
{

    [SerializeField]
    private int interation = 10;
    [SerializeField]
    public int walklenght = 10;
    [SerializeField]
    public bool satrtRandomInter = true;




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

        for (int i = 0;i < interation; i++)
        {
            var path = PG_Algorith.RandomWalk(currentPosition, walklenght);
            floorPosition.UnionWith(path);
            if(satrtRandomInter)
            {
                currentPosition = floorPosition.ElementAt(Random.Range(0, floorPosition.Count));
            }
        }
        return floorPosition;
    }
}
