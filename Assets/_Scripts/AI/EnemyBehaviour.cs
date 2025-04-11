using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyState
{
    Wander,
    FollowTarget,
    Patrol,
    Death,
    Attack
}

public class EnemyBehaviour : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("speed");

    [field:SerializeField]
    public Transform target {get; private set;}

    [Header("Current State")] 
    [SerializeField] private EnemyState state;
    
    [SerializeField] private float detectionRadius;

    [SerializeField] private AIBase[] states;

    public float speed;

    private SphereCollider _collider ;
    private Animator _animator;
    private EnemyHealth _enemyHealth;

    private void Start()
    {
        states = GetComponents<AIBase>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<SphereCollider>();
        _enemyHealth = GetComponent<EnemyHealth>();
        _collider.radius = detectionRadius;
        _animator.SetFloat(Speed, speed);
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
            case EnemyState.Death:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }

    private void UpdatePatrol()
    {
        if (!PlayerIsOnRange(detectionRadius) && _enemyHealth.CurrentHealth > 0) return;

        if (_enemyHealth.CurrentHealth <= 0)
        {
            ChangeState(EnemyState.Death);
            speed = 0;
            return;
        }
        
        speed = 7;
        _animator.SetFloat(Speed, speed);
        ChangeState(EnemyState.FollowTarget);
    }
    
    private void UpdateWander()
    {
        if (!PlayerIsOnRange(detectionRadius) && _enemyHealth.CurrentHealth > 0) return;

        if (_enemyHealth.CurrentHealth <= 0)
        {
            ChangeState(EnemyState.Death);
            speed = 0;
            return;
        }
            
        
        speed = 7;
        _animator.SetFloat(Speed, speed);
        ChangeState(EnemyState.FollowTarget);
    }

    private void UpdateFollowTarget()
    {
        if (PlayerIsOnRange(detectionRadius) && _enemyHealth.CurrentHealth > 0) return;

        if (_enemyHealth.CurrentHealth <= 0)
        {
            ChangeState(EnemyState.Death);
            speed = 0;
            return;
        }
        speed = 3.5f;
        _animator.SetFloat(Speed, speed);
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
        if (!target) return false;
        var sqrDistance = (target.position - transform.position).sqrMagnitude;
        return sqrDistance <= Mathf.Pow(detectionRange, 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ITargeteableByAI>() == null) return;
        
        target = other.GetComponent<ITargeteableByAI>().GetTarget();
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<ITargeteableByAI>() == null) return;
    }
}
