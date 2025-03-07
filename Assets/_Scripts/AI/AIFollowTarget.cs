using System;
using UnityEngine;

public class AIFollowTarget : AIBase
{
    private void Update()
    {
        agent.SetDestination(enemyBehaviour.target.position);
    }
}
