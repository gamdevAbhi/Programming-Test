using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ObstacleManager : MonoBehaviour
{
    [SerializeField] [HideInInspector] private List<bool> toggleData;
    [SerializeField] [HideInInspector] private int[] gridSize = new int[2];
    [SerializeField] [HideInInspector] private int maxGrid = 0;
    [SerializeField] [HideInInspector] private bool isPressed = false;
    [SerializeField] [HideInInspector] private bool takeData = false;
    [SerializeField] [HideInInspector] private bool toggleChange = false;
    [HideInInspector] public int playerIndex = 0;
    [HideInInspector] public int enemyIndex = 0;

    [Header("Scripts")]
    [SerializeField] private ObstacleScriptableObject obstacleData;
    [SerializeField] private EnemyAIScript enemyAIScript;
    [SerializeField] private PlayerScript playerScript;

    [Header("Obstacle Variable")]
    [SerializeField] private float height = 1f;
    [SerializeField] private Color obstacleColor;
    [SerializeField] private GameObject obstaclePrefab;

    [Header("Grid Manager")]
    [SerializeField] private Transform gridSystem;

    private void Initialized(int maxId)
    {
        for(int i = 0; i < obstacleData.obstacleGrid.Count; i++)
        {   
            if(obstacleData.obstacleGrid[i] == true && i != 0 && i != maxId)
            {
                SpawnObstacle(i);
            }
            else
            {
                obstacleData.obstacleGrid[0] = false;
                obstacleData.obstacleGrid[obstacleData.obstacleGrid.Count - 1] = false;
            }
        }
    }

    protected internal void ChangeToggle(int id, bool value)
    {
        toggleData[id] = value;
        ObstacleEditSave();
    }

    private void Update()
    {
        if(isPressed)
        {
            if(Application.isPlaying == false)
            {
                ObstacleEditSave();
            }
            else
            {
                if(playerScript._turnFinish && !enemyAIScript._isTurning)
                {
                    ObstacleEditSave();
                }
            }
        }

        if(takeData)
        {
            for(int i = 0; i < maxGrid; i++)
            {
                toggleData[i] = obstacleData.obstacleGrid[i];
            }
        }

        if(Application.isPlaying == false)
        {
            playerIndex = 0;
            enemyIndex = maxGrid - 1;
        }

        ExecuteEdit();

        if(Application.isPlaying == true)
        {
            ObstacleEdit();
        }

        #if UNITY_EDITOR
        EditorUtility.SetDirty(obstacleData);
        #endif
    }
    private void ObstacleEditSave()
    {
        for(int i = 0; i < maxGrid; i++)
        {
            obstacleData.obstacleGrid[i] = toggleData[i];
        }

        if(Application.isPlaying)
        {
            obstacleData.isChanged = true;
        }
    }
    private void ExecuteEdit()
    {
        toggleChange = false;
        maxGrid = gridSystem.GetComponent<GridSystemScript>()._gridTileSize;

        gridSize[0] = (int)gridSystem.GetComponent<GridSystemScript>()._gridTile.x;
        gridSize[1] = (int)gridSystem.GetComponent<GridSystemScript>()._gridTile.y;

        if(toggleData == null)
        {
            toggleData = new List<bool>();
        }

        if(obstacleData.obstacleGrid == null)
        {
            obstacleData.obstacleGrid = new List<bool>();
        }

        if(toggleData.Count < maxGrid)
        {
            for(int i = toggleData.Count - 1; i < maxGrid; i++)
            {
                toggleData.Add(false);
            }
        }
        else if(toggleData.Count > maxGrid)
        {
            for(int i = toggleData.Count - 1; toggleData.Count > maxGrid; i--)
            {
                toggleData.RemoveAt(i);
            }
        }

        if(obstacleData.obstacleGrid.Count < maxGrid)
        {
            for(int i = obstacleData.obstacleGrid.Count - 1; i < maxGrid; i++)
            {
                obstacleData.obstacleGrid.Add(false);
            }
        }
        else if(obstacleData.obstacleGrid.Count > maxGrid)
        {
            for(int i = obstacleData.obstacleGrid.Count - 1; obstacleData.obstacleGrid.Count > maxGrid; i--)
            {
                obstacleData.obstacleGrid.RemoveAt(i);
            }
        }

        if(obstacleData.runTimeChange == true && Application.isPlaying == false)
        {
            for(int i = 0; i < maxGrid; i++)
            {
                toggleData[i] = obstacleData.obstacleGrid[i];
            }

            obstacleData.runTimeChange = false;
        }
        
        for(int i = 0; i < maxGrid; i++)
        {
            if(toggleData[i] != obstacleData.obstacleGrid[i])
            {
                toggleChange = true;
            }
        }
    }

    private void ObstacleEdit()
    {
        if(obstacleData.isChanged == true)
        {
            for(int i = 0; i < obstacleData.obstacleGrid.Count; i++)
            {   
                if(obstacleData.obstacleGrid[i] == true)
                {
                    SpawnObstacle(i);
                }
                else
                {
                    DestroyObstalce(i);
                }
            }

            obstacleData.isChanged = false;
        }
    }

    private void SpawnObstacle(int id)
    {
        if(gridSystem.GetChild(id).GetComponent<GridScript>()._canMove == true)
        {
            gridSystem.GetChild(id).GetComponent<GridScript>()._canMove = false;

            GameObject obj = Instantiate(obstaclePrefab, gridSystem.GetChild(id).position + new Vector3(0f, height, 0f), Quaternion.identity, gridSystem.GetChild(id));
            obj.GetComponent<Renderer>().material.SetColor("_Color", obstacleColor);
        }
    }

    private void DestroyObstalce(int id)
    {
        if(gridSystem.GetChild(id).GetComponent<GridScript>()._canMove == false)
        {
            gridSystem.GetChild(id).GetComponent<GridScript>()._canMove = true;
            Destroy(gridSystem.GetChild(id).GetChild(0).gameObject);
        }
    }
}
