using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    private int id;
    private Vector2 gridPos;
    private int distance = 0;

    [Header("Grid Color")]
    [SerializeField] private Color selectColor;
    [SerializeField] private Color NoneSelectColor;
    [SerializeField] private Color ObstacleColor;

    [Header("Neighbour")]
    [SerializeField] private List<int> neighbourIDs;
    
    private bool canMove = true;

    public Vector2 _gridPos
    {
        get
        {
            return gridPos;
        }
    }

    public bool _canMove
    {
        get
        {
            return canMove;
        }
        set
        {
            canMove = value;
        }
    }

    public int _id
    {
        get
        {
            return id;
        }
    }

    public int _distance
    {
        get
        {
            return distance;
        }
    }


    private void GridPos(Vector2 pos)
    {
        gridPos = pos;
    }

    private void SetDistance(Vector2 start)
    {
        Vector2 v = start - gridPos;

        distance = (int)(Mathf.Abs(v.x) * (int)Mathf.Abs(v.x) + Mathf.Abs(v.y) * Mathf.Abs(v.y));
    }

    private void IncreaseDistance(int value)
    {
        distance += value;
    }

    private void ChangeColorSelect()
    {
        Material matt = GetComponent<Renderer>().material;

        matt.SetColor("_Color", selectColor);
    }

    private void ChangeColorNonSelected()
    {
        Material matt = GetComponent<Renderer>().material;
        matt.SetColor("_Color", NoneSelectColor);
    }

    private void ChangeColorObstacle()
    {
        Material matt = GetComponent<Renderer>().material;
        matt.SetColor("_Color", ObstacleColor);
    }

    private void AddNeighbour(int id)
    {
        neighbourIDs.Add(id);
    }

    protected internal List<int> GetNeighbour(int specialGrid)
    {
        List<int> neighbours = new List<int>();

        for(int i = 0; i < neighbourIDs.Count; i++)
        {
            if(transform.parent.GetChild(neighbourIDs[i]).GetComponent<GridScript>()._canMove && transform.parent.GetChild(neighbourIDs[i]).GetComponent<GridScript>()._id != specialGrid)
            {
                neighbours.Add(neighbourIDs[i]);
            }
        }

        return neighbours;
    }

    protected internal void DistanceIncreaseNeighbour()
    {
        List<int> neighbours = new List<int>();

        for(int i = 0; i < neighbourIDs.Count; i++)
        {
            if(transform.parent.GetChild(neighbourIDs[i]).GetComponent<GridScript>()._canMove)
            {
                neighbours.Add(neighbourIDs[i]);
            }
        }

        foreach(int element in neighbours)
        {
            transform.parent.GetChild(element).GetComponent<GridScript>().SendMessage("IncreaseDistance", distance);
        }
    }

    protected internal bool HasNeighbour(int node)
    {
        List<int> neighbours = GetNeighbour(-1);

        if(neighbours.Contains(node))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetID(int _id)
    {
        id = _id;
    }

}
