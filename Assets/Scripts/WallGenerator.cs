using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPosition, TileMapVisualize tileMapVisualize)
    {
        var basicWallPosition = FindWallPosition(floorPosition, Direction2D.DirectioList);
        foreach (var position in basicWallPosition)
        {
            tileMapVisualize.PaintSingleBasicWall(position);
        }
    }

    private static HashSet<Vector2Int> FindWallPosition(HashSet<Vector2Int> floorPosition, List<Vector2Int> DirectionList)
    {
        HashSet<Vector2Int> wallPosition = new HashSet<Vector2Int>();
        foreach (var position in floorPosition)
        {
            foreach (var direction in DirectionList)
            {
                var neighbourPosition = position + direction;
                if(floorPosition.Contains(neighbourPosition) == false)
                    wallPosition.Add(neighbourPosition);
            }
            
        }
        return wallPosition;
    }
}
