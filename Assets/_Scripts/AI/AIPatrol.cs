
using UnityEngine;

public class AIPatrol : AIBase
{
    [SerializeField] private Transform[] wayPoints;
    private int _currentWayPointIndex;

    protected override void Start()
    {
        base.Start();
        GoToNextPoint();
    }

    private void Update()
    {
        if(agent.remainingDistance < agent.stoppingDistance && !agent.pathPending)
            GoToNextPoint();
    }

    private void GoToNextPoint()
    {
        if(_currentWayPointIndex >= wayPoints.Length) _currentWayPointIndex = 0;

        agent.SetDestination(wayPoints[_currentWayPointIndex++].position);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }
}
