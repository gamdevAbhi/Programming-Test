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
            if(currentPath[0] == playerScript._currentIndex)
            {
                currentPath = new List<int>();
            }
            else
            {
                transform.position = new Vector3(gridSystem.GetChild(currentPath[0]).position.x, height, gridSystem.GetChild(currentPath[0]).position.z);
                currentIndex = currentPath[0];
                currentPath = new List<int>();
            }

            turnControllerScript.currentTurn = TurnControllerScript.Turn.None;
            time = 0f;
        }
        else if(currentPath.Count > 0 && time <= timeMove && turnControllerScript.currentTurn == TurnControllerScript.Turn.Enemy)
        {
            time += Time.deltaTime;
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
        possiblePath = pathFinder.CanGo(currentIndex, id, -1);

        currentPath = pathFinder.FindPath(currentIndex, id, possiblePath, -1);
    }
}
