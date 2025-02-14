using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Parametrs", menuName = "PCG/Data")]

public class RandomWalkSO : ScriptableObject
{
    public int interations = 10, walklength = 10;
    public bool StartRandomIteration = false;
}
