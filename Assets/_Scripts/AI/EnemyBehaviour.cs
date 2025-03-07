using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyState
{
    Wander,
    FollowTarget,
    Patrol,
    Attack
}

public class EnemyBehaviour : MonoBehaviour
{
    [field:SerializeField]
    public Transform target {get; private set;}

    [Header("Current State")] 
    [SerializeField] private EnemyState state;
    
    [SerializeField] private float detectionRadius;

    [SerializeField] private AIBase[] states;

    private void Start()
    {
        states = GetComponents<AIBase>();
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyState.Wander:
                UpdateWander();
                return;
            case EnemyState.Patrol:
                UpdatePatrol();
                return;
            case EnemyState.FollowTarget:
                UpdateFollowTarget();
                return;
            case EnemyState.Attack:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }

    private void UpdatePatrol()
    {
        if (!PlayerIsOnRange(detectionRadius)) return;

        ChangeState(EnemyState.FollowTarget);
    }
    
    private void UpdateWander()
    {
        if (!PlayerIsOnRange(detectionRadius)) return;

        ChangeState(EnemyState.FollowTarget);
    }

    private void UpdateFollowTarget()
    {
        if (PlayerIsOnRange(detectionRadius)) return;
        
        var dice = Random.Range(0, 100);
        ChangeState(dice >= 50 ? EnemyState.Wander : EnemyState.Patrol);
    }

    private void ChangeState(EnemyState newState)
    {
        state = newState;

        for (int i = 0; i < states.Length; i++)
        {
            states[i].enabled = i == (int)state;
            
            /*if (i == (int)state)
            {
                states[i].enabled = true;
            }
            else
            {
                states[i].enabled = false;
            }*/
        }
    }

    private bool PlayerIsOnRange(float detectionRange)
    {
        var sqrDistance = (target.position - transform.position).sqrMagnitude;
        return sqrDistance <= Mathf.Pow(detectionRange, 2);
    }
}
