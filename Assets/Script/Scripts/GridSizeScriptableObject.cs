using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridTileScriptableObject", menuName = "ScriptableObject/GridTileScriptableObject", order = 0)]
public class GridSizeScriptableObject : ScriptableObject
{
    public int x;
    public int y;

    public Vector2 gridTile
    {
        get
        {
            return new Vector2(x, y);
        }
        set
        {
            x = (int)value.x;
            y = (int)value.y;
        }
    }
}
