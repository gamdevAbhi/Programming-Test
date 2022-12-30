using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagerScript : MonoBehaviour
{
    [Header("Variable")]
    [SerializeField] private float hoverSize = 2f;

    [Header("UI Element")]
    [SerializeField] private TextMeshProUGUI currentGridPos;
    [SerializeField] private TextMeshProUGUI gridSize;
    [SerializeField] private Button edit;
    [SerializeField] private Button isomatric;
    [SerializeField] private Button cancel;
    [SerializeField] private Button gridIncrease;
    [SerializeField] private Button gridDecrease;

    [Header("Script")]
    [SerializeField] private TurnControllerScript turnControllerScript;

    private void Update()
    {
        if(turnControllerScript.currentTurn == TurnControllerScript.Turn.None)
        {
            isomatric.interactable = true;
            cancel.interactable = false;
            edit.interactable = true;
            gridIncrease.interactable = true;
            gridDecrease.interactable = true;
        }
        else if(turnControllerScript.currentTurn != TurnControllerScript.Turn.None)
        {
            isomatric.interactable = false;
            cancel.interactable = true;
            edit.interactable = false;
            gridIncrease.interactable = false;
            gridDecrease.interactable = false;
        }
    }

    public void OnHover(Transform _transform)
    {
        _transform.localScale = new Vector3(hoverSize, hoverSize, 1f);
    }

    public void OnHoverEnd(Transform _transform)
    {
        _transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void DisplayCurrentGridPos(string pos)
    {
        currentGridPos.text = pos;
    }

    private void DisplayGridSize(string size)
    {
        gridSize.text = "Grid Size\n" + size;
    }
}
