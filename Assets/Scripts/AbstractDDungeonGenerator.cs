using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TileMapVisualize tileMapVisualizer = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;


    public void GenerateDungeon()
    {
        tileMapVisualizer.Clear();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
}
