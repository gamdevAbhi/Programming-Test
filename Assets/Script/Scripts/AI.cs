using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AI
{
    void Initialize(int id);
    void SetTargetIndex(int id);
    void Turn(int playerId);
}
