using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnControllerScript : MonoBehaviour
{
    public enum Turn {None, Player, Enemy};

    [Header("Turn")]
    [SerializeField] protected internal Turn currentTurn = Turn.Player;
    
    [Header("Scripts")]
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private EnemyAIScript enemyAIScript;
    [SerializeField] private InputManager inputManager;

    private void Update()
    {
        if(currentTurn == Turn.Player && playerScript._turnFinish)
        {
            currentTurn = Turn.Enemy;
            enemyAIScript.Turn(playerScript._currentIndex);
        }
    }
    protected internal bool isAvailable(int id)
    {
        if(id == enemyAIScript._currentIndex)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
