using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState<T> : State<T>
{
    private Action _onIdle;
    private Func<bool> _isInSight;
    private iNode _root;
    private float _counter;
    private float _cooldown;
    private Action<bool> _setIdleCommand;

    public EnemyIdleState(float idlecounter, Action onIdle, Func<bool> isInSight,Action<bool> setIdleCommand ,iNode root)
    {
        _onIdle = onIdle;
        _root = root;
        _isInSight = isInSight;
        _counter = idlecounter;
        _setIdleCommand = setIdleCommand;

    }

    public override void Awake()
    {
        ResetCooldown();
    }

    public override void Execute()
    {
        //Debug.Log("Its a me Idle");
        _onIdle?.Invoke();
        var isSeen = _isInSight();

        if (isSeen||_cooldown<Time.time)
        {
           // Debug.Log("End of Idle");
            _root.Execute(); 
            _setIdleCommand?.Invoke(false);
            ResetCooldown();
        }
        
        
        
    }
    

    void ResetCooldown()
    {
        _cooldown = _counter + Time.time;
    }

}
