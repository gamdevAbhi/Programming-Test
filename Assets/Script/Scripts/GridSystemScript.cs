using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemScript : MonoBehaviour
{
    [Header("Grid Variables")]
    [SerializeField] private Vector2 gridTile;
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private GridSizeScriptableObject gridSizeObj;

    private Vector2 gridTileSize = new Vector2(1f, 1f);
    private Transform selectGrid;

    public Transform _selectGrid
    {
        get
        {
            return selectGrid;
        }
    }

    public int _gridTileSize
    {
        get
        {
            return (int)gridTile.x * (int)gridTile.y;
        }
    }

    public Vector2 _gridTile
    {
        get
        {
            return gridTile;
        }
    }

    [Header("Scripts")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private UIManagerScript uIManagerScript;
    [SerializeField] private ToggleObstacleScript toggleObstacleScript;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private ObstacleManager obstacleManager;
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private EnemyAIScript enemyAIScript;

    private void Start()
    {
        ChangeGridTile(gridSizeObj.gridTile);
        uIManagerScript.SendMessage("DisplayGridSize", gridTile.x.ToString() + " X " + gridTile.y.ToString());
        CreateGridSystem();
        obstacleManager.SendMessage("Initialized", gridTile.x * gridTile.y - 1);
        playerScript.SendMessage("Initialize");
        enemyAIScript.Initialize((int)gridTile.x * (int)gridTile.y - 1);
    }

    private void Update()
    {
        selectGrid = inputManager.GetCurrentGrid(gridLayer);

        string pos = "";

        if(selectGrid != null)
        {
            GridScript script = selectGrid.GetComponent<GridScript>();
            pos = "X = " + script._gridPos.x + " Y = " + script._gridPos.y;
        }

        uIManagerScript.SendMessage("DisplayCurrentGridPos", pos);

        ChangeGridColors();
    }

    protected internal void ChangeGridTile(Vector2 NewGridTile)
    {
        gridTile.x = NewGridTile.x;
        gridTile.y = NewGridTile.y;
    }

    private void ChangeGridColors()
    {
        foreach(Transform child in transform)
        {
            if(selectGrid == child)
            {
                child.SendMessage("ChangeColorSelect");
            }
            else if(child.GetComponent<GridScript>()._canMove == true)
            {
                child.SendMessage("ChangeColorNonSelected");
            }
            else
            {
                child.SendMessage("ChangeColorObstacle");
            }
        }
    }

    private void CreateGridSystem()
    {
        float x = 0;
        float y = 0;
        int id = 0;

        for(int i = 0; i < gridTile.y; i++)
        {
            for(int j = 0; j < gridTile.x; j++)
            {
                GameObject gridObj = Instantiate(gridPrefab, new Vector3(x, 0, y), Quaternion.identity, transform);

                gridObj.SendMessage("GridPos", new Vector2(x / gridTileSize.x, y / gridTileSize.y));
                gridObj.SendMessage("SetID", id);

                GridScript currentGridScript = gridObj.GetComponent<GridScript>();

                if(currentGridScript._gridPos.x != 0)
                {
                    currentGridScript.SendMessage("AddNeighbour", id - 1);
                }
                if(currentGridScript._gridPos.x != gridTile.x - 1)
                {
                    currentGridScript.SendMessage("AddNeighbour", id + 1);
                }
                if(currentGridScript._gridPos.y != 0)
                {
                    currentGridScript.SendMessage("AddNeighbour", id - gridTile.x);
                }
                if(currentGridScript._gridPos.y != gridTile.y - 1)
                {
                    currentGridScript.SendMessage("AddNeighbour", id + gridTile.x);
                }

                x += gridTileSize.x;
                id++;
            }

            y += gridTileSize.y;
            x = 0;
        }
        
        cameraManager.SendMessage("ChangeCamera", false);
        toggleObstacleScript.SendMessage("CreateToggle", (int)gridTile.x * (int)gridTile.y);
        toggleObstacleScript.SendMessage("SetPosition", transform);
        cameraManager.SendMessage("ChangeCamera", true);
    }
}
