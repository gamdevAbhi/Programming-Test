using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [Header("Grid Manager")]
    [SerializeField] private Transform gridSystem;

    protected internal List<int> FindPath(int start, int destination, bool possiblePath, int specialGrid)
    {
        if(possiblePath)
        {
            List<int> path = NewGetPath(start, destination, specialGrid);

            return path;
        }
        else
        {
            return new List<int>();
        }
    }


    protected internal bool CanGo(int root, int destination, int specialGrid)
    {
        List<bool> arriveP = new List<bool>();

        foreach(Transform child in gridSystem)
        {
            if(child.GetComponent<GridScript>()._canMove)
            {
                arriveP.Add(false);
            }
            else
            {
                arriveP.Add(true);
            }
        }

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

            foreach(int id in gridSystem.GetChild(currentRoot).GetComponent<GridScript>().GetNeighbour(specialGrid))
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
    
    private List<int> NewGetPath(int start, int destination, int specialGrid)
    {
        List<int> visited = new List<int>();
        List<int> beforeNeighbour = new List<int>();
        List<int> NonSP = new List<int>();
        List<int> SP = new List<int>();


        SetDistance(start);

        int currentNode = start;
        
        NonSP.Add(currentNode);
        visited.Add(currentNode);

        while(currentNode != destination)
        {

            List<int> newNeighbour = new List<int>();
            
            newNeighbour = gridSystem.GetChild(currentNode).GetComponent<GridScript>().GetNeighbour(specialGrid);
            

            newNeighbour = RemoveSameElement(newNeighbour, visited);
            newNeighbour = RemoveSameElement(newNeighbour, beforeNeighbour);

            beforeNeighbour = MargeTwoList(newNeighbour, beforeNeighbour);

            int lowestDistance = GetLowestDistance(beforeNeighbour);
            

            beforeNeighbour.Remove(lowestDistance);

            currentNode = lowestDistance;
            
            gridSystem.GetChild(currentNode).GetComponent<GridScript>().DistanceIncreaseNeighbour();

            NonSP.Add(currentNode);
            visited.Add(currentNode);
        }

        NonSP.Reverse();

        SP.Add(NonSP[0]);

        int index = 0;

        for(int i = 1; i < NonSP.Count - 1; i++)
        {
            if(gridSystem.GetChild(NonSP[index]).GetComponent<GridScript>().HasNeighbour(NonSP[i]))
            {
                index = i;
                SP.Add(NonSP[i]);
            }
        }

        SP.Reverse();

        return SP;
    }

    private void SetDistance(int id)
    {
        foreach(Transform trans in gridSystem)
        {
            trans.SendMessage("SetDistance", gridSystem.GetChild(id).GetComponent<GridScript>()._gridPos);
        }
    }

    private int GetLowestDistance(List<int> nodes)
    {
        int lowest = 0;

        for(int i = 0; i < nodes.Count; i++)
        {
            if(gridSystem.GetChild(nodes[lowest]).GetComponent<GridScript>()._distance > gridSystem.GetChild(nodes[i]).GetComponent<GridScript>()._distance)
            {
                lowest = i;
            }
        }

        return nodes[lowest];
    }

    private List<int> MargeTwoList(List<int> firstList, List<int> secondList)
    {
        List<int> newList = new List<int>();

        foreach(int element in firstList)
        {
            newList.Add(element);
        }

        foreach(int element in secondList)
        {
            newList.Add(element);
        }

        return newList;
    }

    private List<int> RemoveSameElement(List<int> getRemoved, List<int> checker)
    {
        List<int> newList = new List<int>();

        foreach(int element in getRemoved)
        {
            if(!checker.Contains(element))
            {
                newList.Add(element);
            }
        }

        return newList;
    }

    private void DebugList(List<int> list, string message, int max)
    {
        Debug.Log(message);

        foreach(int element in list)
        {
            Debug.Log(element + " " + max);
        }
    }

}
