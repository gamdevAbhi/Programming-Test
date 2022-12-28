using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AiInterface
{
    void Initialize(int id);
    void SetTargetIndex(int id);
    List<int> PathFind(int start, int playerIndex);
    bool CanGo(int root, int playerIndex, List<bool> arriveP);
    List<int> GetPath(int root, int playerIndex, List<int> path, List<bool> visitate);
    bool IsPlayerTouch();
    void Turn(int playerId);
}
