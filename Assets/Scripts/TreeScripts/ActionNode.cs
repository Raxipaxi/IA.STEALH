using System;
using UnityEngine;

public class ActionNode : iNode
{
    private Action _action;

    public ActionNode(Action action)
    {
        _action = action;
    }
    
    public void Execute()
    {
        if (_action != null)
            _action();
    }
}
