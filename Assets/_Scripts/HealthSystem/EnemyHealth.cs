using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : BaseHealth
{
    private static readonly int Die1 = Animator.StringToHash("die");
    private NavMeshAgent _agent;
    private EnemyBehaviour _enemyBehaviour;

    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    protected override void Die()
    {
        _enemyBehaviour.enabled = false;
        _agent.ResetPath();
        _anim.SetTrigger(Die1);
        Destroy(gameObject, deathCooldown);
    }
}
