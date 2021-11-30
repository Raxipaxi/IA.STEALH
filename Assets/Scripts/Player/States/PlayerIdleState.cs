using System;

public class PlayerIdleState<T> : State<T>
{
    T _input;
    private Action _onIdle;
    private iInput _playerInput;

    public PlayerIdleState(Action onIdle, T inputWalk, iInput playerInput)
    {
        _input = inputWalk;
        _onIdle = onIdle;
        _playerInput = playerInput;
    }


    public override void Execute()
    {
        _playerInput.UpdateInputs();
        if (_playerInput.IsMoving())
        {
            _fsm.Transition(_input);
        }
        _onIdle?.Invoke();
    }
}
