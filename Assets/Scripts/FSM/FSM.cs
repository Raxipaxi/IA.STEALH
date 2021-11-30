using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T>
{
    IState<T> _current;
    public FSM()
    {
    }
    public void SetInit(IState<T> initState)
    {
        _current = initState;
        _current.FinitStateMachine = this;
        _current.Awake();
    }
    public void OnUpdate()
    {
        if (_current != null)
            _current.Execute();
    }
    public void Transition(T input)
    {
        IState<T> newState = _current.GetTransition(input);
        if (newState != null)
        {
            newState.FinitStateMachine = this;
            _current.Sleep();
            _current = newState;
            _current.Awake();
        }
    }
}
