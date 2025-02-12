using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractDDungeonGenerator), true)]
public class DungeonEditor : Editor
{
   AbstractDDungeonGenerator generator;


    private void Awake()
    {
        generator = (AbstractDDungeonGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Create Dungeon"))
        {
            generator.GenerateDungeon();
        }

    }
}
