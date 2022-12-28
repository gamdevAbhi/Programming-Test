using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    private int id;
    private Vector2 gridPos;

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


    private void GridPos(Vector2 pos)
    {
        gridPos = pos;
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

    protected internal List<int> GetNeighbour()
    {
        return neighbourIDs;
    }

    private void SetID(int _id)
    {
        id = _id;
    }

}
