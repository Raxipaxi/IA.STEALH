
using System;
using UnityEngine;

public class PlayerWalkState<T> : State<T>
{
    private T _input;
    private Action<Vector3> _onWalk;
    private iInput _playerInput;

    public PlayerWalkState(Action<Vector3> onWalk, T inputIdle, iInput playerInput)
    {
        _onWalk = onWalk;
        _input = inputIdle;
        _playerInput = playerInput; 
    }

    public override void Execute()
    {
        _playerInput.UpdateInputs();
        var h = _playerInput.GetH;
        var v = _playerInput.GetV;
       
        if (!_playerInput.IsMoving())
        {
            _fsm.Transition(_input); // Trans to Idle
        }
        else
        {
            Vector3 dir = new Vector3(h, 0, v);
            _onWalk?.Invoke(dir);
        }
        
    }
}
