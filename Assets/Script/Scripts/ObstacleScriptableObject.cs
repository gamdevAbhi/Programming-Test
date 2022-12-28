using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleScriptableObject", menuName = "ScriptableObject/ObstacleScriptableObject", order = 0)]
public class ObstacleScriptableObject : ScriptableObject 
{
    public bool isChanged;
    public bool runTimeChange;
    public List<bool> obstacleGrid;
}