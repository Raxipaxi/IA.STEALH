using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Properties

    private FSM<PlayerStates> _fsm;
    private PlayerModel _player;
    private iInput _playerInput;
    #endregion

    public event Action<Vector3> OnMove; 
    public event Action OnIdle; 
    private void Awake()
    {
        _player = GetComponent<PlayerModel>();
        _playerInput = GetComponent<iInput>();
        
        FsmInit();
    }

    private void Start()
    {
        _player.Subscribe(this);
    }

    private void FsmInit()
    {
        
        //--------------- FSM Creation -------------------//                
        var idle = new PlayerIdleState<PlayerStates>(IdleCommmand, PlayerStates.Walk, _playerInput );
        var walk = new PlayerWalkState<PlayerStates>(WalkCommmand, PlayerStates.Idle, _playerInput);
        
        idle.AddTransition(PlayerStates.Walk, walk);
        walk.AddTransition(PlayerStates.Idle, idle);

        _fsm = new FSM<PlayerStates>();
        // Set init state
        _fsm.SetInit(idle);

    }

    public void WalkCommmand(Vector3 dir)
    {
        OnMove?.Invoke(dir);
    }
    public void IdleCommmand()
    {
        OnIdle?.Invoke();
    }
    
    void Update()
    {
     _fsm.OnUpdate();   
    }


}
