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
        if(currentTurn == Turn.Player)
        {
            currentTurn = Turn.Enemy;
        }
        else if(currentTurn == Turn.Enemy)
        {
            enemyAIScript.Turn(playerScript._currentIndex);
        }
    }
}
