using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState<T> : State<T>
{
    #region Properties

    private iNode _root;
    private Func<bool> _isInSight;
    private Transform[] _patrolPoints;
    private Transform _currpatrolPoint =null;
    private Transform _transform;
    private Action<Vector3> _onPatrol;
    private Action<bool> _setIdleCommand;
    private ObstacleAvoidance _obstacleAvoidance;


    private HashSet<Transform> visitedWP= new HashSet<Transform>();
    private float _midDist;



    #endregion

    public EnemyPatrolState(Transform transform, Action<bool> setIdleCommand,Func<bool> isInSight,Transform[] waypoints,ObstacleAvoidance obstacleAvoidance, 
         Action<Vector3> onPatrol , float minDist,iNode root)
    {
 
        _root = root;
        _patrolPoints = waypoints;
        _isInSight = isInSight;
        _transform = transform;
        _midDist = minDist;
        _obstacleAvoidance = obstacleAvoidance;
        _setIdleCommand = setIdleCommand;
        _onPatrol = onPatrol;

    }

    public override void Awake()
    {
        // Start point to patrol
        if (_currpatrolPoint == null)
        {
            _currpatrolPoint = NearestPatPoint();
        }
        _obstacleAvoidance.SetNewBehaviour(ObstacleAvoidance.DesiredBehaviour.Seek);
        _obstacleAvoidance.SetNewTarget(_currpatrolPoint);
        _setIdleCommand?.Invoke(false);
    }

    public override void Execute()
    {
      //  Debug.Log("Patrol");
       
        if (_isInSight()) { _root.Execute(); _setIdleCommand?.Invoke(false); return; }

       // Debug.Log("Eh visto un enano " + _isInSight());
        var dir = _obstacleAvoidance.GetDir();
        _onPatrol?.Invoke(dir);

        var distance = Vector3.Distance(_transform.position, _currpatrolPoint.position);

        if (distance>_midDist) return;
        
        ResetPatrolPoint();
        
        _setIdleCommand?.Invoke(true);
        
        _root.Execute();




    }
    //Look up the nearest patrolPoint
    private Transform NearestPatPoint()
    {
        float minDist= float.MaxValue;
        float currDist;
        Transform nearestPatrolpt= null;
        
        for (int i = 0; i < _patrolPoints.Length; i++)
        {
            currDist = Vector3.Distance(_transform.position, _patrolPoints[i].position);
            var currPatrol = _patrolPoints[i];
    
            if (currDist>minDist || visitedWP.Contains(currPatrol)) continue;
            
                minDist = currDist;
                nearestPatrolpt = currPatrol;
            
        }
  
        visitedWP.Add(nearestPatrolpt);
        if (visitedWP.Count == _patrolPoints.Length) CleanVisitedWp();
            
        return nearestPatrolpt;
    }
    private void CleanVisitedWp()
    {
        visitedWP.Clear();
    }
    private void ResetPatrolPoint()
    {
        _currpatrolPoint = NearestPatPoint();
        _obstacleAvoidance.SetNewTarget(_currpatrolPoint);
    }
}
