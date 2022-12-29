using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Grid Manager")]
    [SerializeField] private Transform gridSystem;

    [Header("Player Variable")]
    [SerializeField] private float height = 1f;
    [SerializeField] private float timeMove = 0.5f;
    [SerializeField] private Color playerColor;

    [Header("Script")]
    [SerializeField] private TurnControllerScript turnControllerScript;
    [SerializeField] private ObstacleManager obstacleManager;
    [SerializeField] private EnemyAIScript enemyAIScript;
    [SerializeField] private PathFinder pathFinder;

    private bool possiblePath = false;
    private int currentIndex = 0;
    private bool isCancel = false;

    private List<int> currentPath = new List<int>();

    float time = 0f;

    public int _currentIndex
    {
        get
        {
            return currentIndex;
        }
    }

    public bool _turnFinish
    {
        get
        {
            return (currentPath.Count == 0);
        }
    }

    private void Initialize()
    {
        transform.position = new Vector3(gridSystem.GetChild(0).position.x, height, gridSystem.GetChild(0).position.z);
        GetComponent<Renderer>().material.SetColor("_EmissionColor", playerColor);
    }

    private void Update()
    {
        obstacleManager.playerIndex = currentIndex;
        
        if(time > timeMove && currentPath.Count > 0 && turnControllerScript.currentTurn != TurnControllerScript.Turn.Enemy)
        {
            currentIndex = currentPath[0];

            transform.position = gridSystem.GetChild(currentIndex).transform.position;
            transform.position = new Vector3 (transform.position.x, 1, transform.position.z);
            time = 0f;
            currentPath.Remove(currentIndex);
                
            turnControllerScript.currentTurn = TurnControllerScript.Turn.Player;

            if(isCancel == true)
            {
                currentPath = new List<int>();
                isCancel = false;
            }
            else if(currentPath.Count > 0)
            {
                currentIndex = currentPath[0];
            }
        }
        else if(turnControllerScript.currentTurn != TurnControllerScript.Turn.Enemy && currentPath.Count > 0)
        {
            time += Time.deltaTime;
        }
    }

    protected internal bool GetStatusMoving()
    {
        if(currentPath.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Cancel()
    {
        isCancel = true;
    }

    private void SetTargetIndex(int id)
    {
        possiblePath = pathFinder.CanGo(currentIndex, id, enemyAIScript._currentIndex);

        currentPath = pathFinder.FindPath(currentIndex, id, possiblePath, enemyAIScript._currentIndex);
    }
    
}
