using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIScript : MonoBehaviour, AI
{
    [Header("Grid Manager")]
    [SerializeField] private Transform gridSystem;
    [SerializeField] private Transform player;

    [Header("Player Variable")]
    [SerializeField] private float height = 1f;
    [SerializeField] private float timeMove = 0.5f;
    [SerializeField] private Color enemyColor;

    [Header("Script")]
    [SerializeField] private TurnControllerScript turnControllerScript;
    [SerializeField] private ObstacleManager obstacleManager;
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private PathFinder pathFinder;

    private bool possiblePath = false;
    private int currentIndex = 0;
    private float time = 0f;

    private List<int> currentPath = new List<int>();

    public bool _isTurning
    {
        get
        {
            return (currentPath.Count > 1);
        }
    }

    public int _currentIndex
    {
        get
        {
            return currentIndex;
        }
    }

    public void Initialize(int id)
    {
        transform.position = new Vector3(gridSystem.GetChild(id).position.x, height, gridSystem.GetChild(id).position.z);
        GetComponent<Renderer>().material.SetColor("_EmissionColor", enemyColor);
        currentIndex = id;
    }

    private void Update()
    {
        obstacleManager.enemyIndex = currentIndex;
        
        if(currentPath.Count > 0 && time >= timeMove)
        {
            transform.position = new Vector3(gridSystem.GetChild(currentPath[0]).position.x, height, gridSystem.GetChild(currentPath[0]).position.z);
            currentIndex = currentPath[0];
            currentPath.Remove(currentPath[0]);
            time = 0f;
        }
        else if(currentPath.Count > 0 && time <= timeMove && turnControllerScript.currentTurn == TurnControllerScript.Turn.Enemy)
        {
            time += Time.deltaTime;
        }
        else if(currentPath.Count == 0 && turnControllerScript.currentTurn == TurnControllerScript.Turn.Enemy)
        {
            turnControllerScript.currentTurn = TurnControllerScript.Turn.None;
        }
    }

    public void Turn(int playerId)
    {
        if(playerId != currentIndex)
        {
            SetTargetIndex(playerId);
        }
        else
        {
            turnControllerScript.currentTurn = TurnControllerScript.Turn.None;
        }
    }

    public void SetTargetIndex(int id)
    {
        List<List<int>> adjacentPath = new List<List<int>>();
        List<int> adjacentTile = gridSystem.GetChild(id).GetComponent<GridScript>().GetNeighbour(-1);
        List<int> nearPath = new List<int>();
        
        foreach(int tile in adjacentTile)
        {
            possiblePath = pathFinder.CanGo(currentIndex, tile, id);

            if(possiblePath)
            {
                adjacentPath.Add(pathFinder.FindPath(currentIndex, tile, possiblePath, id));
            }
        }

        if(adjacentPath.Count > 0)
        {
            int nearPathIndex = 0;

            for(int i = 0; i < adjacentPath.Count; i++)
            {
                if(adjacentPath[i].Count < adjacentPath[nearPathIndex].Count)
                {
                    nearPathIndex = i;
                }
            }

            nearPath = adjacentPath[nearPathIndex];
        }

        currentPath = nearPath;
    }
}
