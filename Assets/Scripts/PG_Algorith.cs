using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class PG_Algorith
{
    public static HashSet<Vector2Int> RandomWalk(Vector2Int startposition, int walklength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startposition);

        var previous_pos = startposition;

        for (int i = 0; i < walklength; i++)
        {
            var new_pos = previous_pos + Direction2D.RandomDirect();
            path.Add(new_pos);
            previous_pos = new_pos;
        }
        return path;
    }
}

public static class Direction2D
{
    public static List<Vector2Int> DirectioList = new List<Vector2Int>
    {
        new Vector2Int(1,0),
        new Vector2Int(0,1),
        new Vector2Int(-1,0),
        new Vector2Int(0,-1)
    };

    public static Vector2Int RandomDirect()
    {
        return DirectioList[Random.Range(0, DirectioList.Count)];
    }
}
