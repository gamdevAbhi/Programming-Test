using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIScript : MonoBehaviour, AiInterface
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

    private List<bool> arrive;
    private bool possiblePath = false;
    private int currentIndex = 0;
    private float time = 0f;

    private List<int> currentPath = new List<int>();
    private List<int> movableGrid = new List<int>();

    public bool _isTurning
    {
        get
        {
            return (currentPath.Count > 1);
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
            currentPath = new List<int>();
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
        arrive = new List<bool>();

        foreach(Transform child in gridSystem)
        {
            if(child.GetComponent<GridScript>()._canMove)
            {
                arrive.Add(false);
            }
            else
            {
                arrive.Add(true);
            }
        }

        possiblePath = CanGo(currentIndex, id, arrive);

        arrive = new List<bool>();

        foreach(Transform child in gridSystem)
        {
            if(child.GetComponent<GridScript>()._canMove)
            {
                arrive.Add(false);
            }
            else
            {
                arrive.Add(true);
            }
        }

        currentPath = PathFind(currentIndex, id);
    }

    public List<int> PathFind(int start, int playerIndex)
    {
        if(possiblePath)
        {
            movableGrid = new List<int>();
        
            foreach(Transform child in gridSystem)
            {
                if(child.GetComponent<GridScript>()._canMove == true)
                {
                    movableGrid.Add(child.GetComponent<GridScript>()._id);
                }
            }
            
            List<int> path = new List<int>();
            List<bool> visitate = new List<bool>();

            path = GetPath(start, playerIndex, path, visitate);

            return path;
        }
        else
        {
            turnControllerScript.currentTurn = TurnControllerScript.Turn.None;
            return new List<int>();
        }
    }


    public bool CanGo(int root, int playerIndex, List<bool> arriveP)
    {
        Queue breadth = new Queue();

        breadth.Enqueue(root);

        arriveP[root] = true;
        
        while(breadth.Count > 0)
        {
            int currentRoot = (int)breadth.Dequeue();

            if(currentRoot == playerIndex)
            {

                return true;
            }

            foreach(int id in gridSystem.GetChild(currentRoot).GetComponent<GridScript>().GetNeighbour())
            {
               if(arriveP[id] == false)
               {
                   breadth.Enqueue(id);
                   arriveP[id] = true;
               }
            }
        }

        return false;
    }

    public List<int> GetPath(int root, int playerIndex, List<int> path, List<bool> visitate)
    {
        visitate = new List<bool>();

        for(int i = 0; i < movableGrid.Count; i++)
        {
            visitate.Add(false);
        }

        int currentRoot = root;

        visitate[movableGrid.IndexOf(currentRoot)] = false;

        while(currentRoot != playerIndex)
        {
            List<int> movableNeighbour = new List<int>();

            foreach(int id in gridSystem.GetChild(currentRoot).GetComponent<GridScript>().GetNeighbour())
            {
                if(movableGrid.Contains(id) && visitate[movableGrid.IndexOf(id)] == false)
                {
                    movableNeighbour.Add(id);
                }
            }

            List<int> neighbourH = new List<int>();
            List<int> neighbourG = new List<int>();
            List<int> neighbourF = new List<int>();

            for(int i = 0; i < movableNeighbour.Count; i++)
            {

                Vector2 vector = (gridSystem.GetChild(playerIndex).GetComponent<GridScript>()._gridPos - gridSystem.GetChild(movableNeighbour[i]).GetComponent<GridScript>()._gridPos);
                neighbourH.Add(Mathf.Abs((int)vector.x) + Mathf.Abs((int)vector.y));

                vector = (gridSystem.GetChild(root).GetComponent<GridScript>()._gridPos - gridSystem.GetChild(movableNeighbour[i]).GetComponent<GridScript>()._gridPos);
                neighbourG.Add(Mathf.Abs((int)vector.x) + Mathf.Abs((int)vector.y));

                neighbourF.Add(neighbourH[i] + neighbourG[i]);
            }

            int lowest = 0;

            for(int i = 0; i < movableNeighbour.Count; i++)
            {
                if(neighbourF[lowest] > neighbourF[i])
                {
                    lowest = i;
                }
                else if(neighbourF[lowest] == neighbourF[i])
                {
                    lowest = (neighbourH[lowest] < neighbourH[i])? lowest : i;
                }
            }

            if(movableNeighbour.Count == 0 && currentRoot != playerIndex)
            {
                path.Remove(currentRoot);
                visitate[movableGrid.IndexOf(currentRoot)] = true;
                currentRoot = path[path.Count - 1];
            }
            else
            {
                currentRoot = movableNeighbour[lowest];
                visitate[movableGrid.IndexOf(currentRoot)] = true;

                path.Add(currentRoot);
            }
        }

        return path;
    }

    public bool IsPlayerTouch()
    {
        return false;
    }
}
