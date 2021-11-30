using UnityEngine;
using System;
public class EnemyFollowState<T> : State<T>
{
    #region Properties

    private Transform _target;

    private iNode _root;
    private ObstacleAvoidance _behaviour;
   
    private float _chaseTimer;
    private float _counter;
    private Action<Vector3> _onMove;
    private Action <bool> _setIdleCommand;
    #endregion

    public EnemyFollowState(Transform target, iNode root, ObstacleAvoidance behaviour, float chaseTimer, Action<Vector3> onMove, Action<bool> setIdleCommand)
    {
        _target = target;
        _root = root;
        _behaviour = behaviour;
        _chaseTimer = chaseTimer;
        _onMove = onMove;
        _setIdleCommand = setIdleCommand;
    }

    public override void Awake()
    {
       //Debug.Log("Follow Awake");
        _behaviour.SetNewBehaviour(ObstacleAvoidance.DesiredBehaviour.Pursuit);
        _behaviour.SetNewTarget(_target);
        _setIdleCommand?.Invoke(false);
        ResetCounter();
    }

    public override void Execute()
    {
        //Debug.Log("Follow");
        var dir = _behaviour.GetDir();
        _onMove?.Invoke(dir);
        
        _counter -= Time.deltaTime;

        if (_counter > 0) return;
        
        _setIdleCommand?.Invoke(false);
        ResetCounter();
        _root.Execute();
    }
    private void ResetCounter()
    {
        _counter = _chaseTimer;
    }
}
