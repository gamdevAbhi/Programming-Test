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

    private List<bool> arrive;
    private bool possiblePath = false;
    private int currentIndex = 0;
    private bool isCancel = false;

    private List<int> currentPath = new List<int>();
    private List<int> movableGrid = new List<int>();

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
        
        if(time > timeMove && currentPath.Count > 0 && turnControllerScript.currentTurn == TurnControllerScript.Turn.None)
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
        else if(turnControllerScript.currentTurn == TurnControllerScript.Turn.None)
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

        currentPath = FindPath(currentIndex, id);
    }
    
    private List<int> FindPath(int start, int destination)
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
            path = GetPath(start, destination, path, visitate);

            return path;
        }
        else
        {
            return new List<int>();
        }
    }


    private bool CanGo(int root, int destination, List<bool> arriveP)
    {
        Queue breadth = new Queue();

        breadth.Enqueue(root);

        arriveP[root] = true;
        
        while(breadth.Count > 0)
        {
            int currentRoot = (int)breadth.Dequeue();

            if(currentRoot == destination)
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

    private List<int> GetPath(int root, int destination, List<int> path, List<bool> visitate)
    {
        visitate = new List<bool>();

        for(int i = 0; i < movableGrid.Count; i++)
        {
            visitate.Add(false);
        }

        int currentRoot = root;

        visitate[movableGrid.IndexOf(currentRoot)] = false;

        while(currentRoot != destination)
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

                Vector2 vector = (gridSystem.GetChild(destination).GetComponent<GridScript>()._gridPos - gridSystem.GetChild(movableNeighbour[i]).GetComponent<GridScript>()._gridPos);
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

            if(movableNeighbour.Count == 0 && currentRoot != destination)
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
}
