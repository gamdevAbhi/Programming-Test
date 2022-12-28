using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class GridTileInitiater : MonoBehaviour
{
    [Header("Script")]
    [SerializeField] private GridSystemScript gridSystem;
    [SerializeField] private GridSizeScriptableObject gridScriptObj;

    private void Update()
    {
        if(gridSystem._gridTile != gridScriptObj.gridTile && Application.isPlaying == false)
        {
            gridScriptObj.gridTile = gridSystem._gridTile;
        }
        if(gridSystem._gridTile.x < 2 && gridSystem._gridTile.y < 2)
        {
            gridSystem.ChangeGridTile(new Vector2(2f, 2f));
        }

        #if UNITY_EDITOR
        EditorUtility.SetDirty(gridScriptObj);
        #endif
    }
}
