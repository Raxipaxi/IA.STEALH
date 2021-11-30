using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    

    #region Properties

    [SerializeField] private float attackRange;
    [SerializeField] private PlayerModel target;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float _idleLenght;
    [SerializeField] private float _chaseTimer;
    [SerializeField] private float _attackCd;
    
    private EnemyModel _enemyModel;

    [SerializeField] private ObstacleAvoidanceScriptableObject obstacleAvoidance;
    public ObstacleAvoidance Behaviour { get; private set; }
    private bool _waitForIdleState;
    public Transform _transform => transform;
    [SerializeField] private float minPatrolDist;

    private bool _isIdle;
    // Decision Tree and FSM Variables 
    private FSM<EnemyStates> _fsm;
  
    iNode _root;
    //private bool _isInSight;

    
    #endregion

    #region Actions

    public event Action OnIdle;
    public event Action<Vector3> OnPatrol;
    public event Action<Vector3> OnFollow;
    public event Action<int> OnAttack;
    
    
    #endregion
    void Awake()
    {
        _enemyModel = GetComponent<EnemyModel>();
        Behaviour = new ObstacleAvoidance(transform, null, obstacleAvoidance.radius,
            obstacleAvoidance.maxObjs, obstacleAvoidance.obstaclesMask,
            obstacleAvoidance.multiplier, target, obstacleAvoidance.timePrediction,
            ObstacleAvoidance.DesiredBehaviour.Seek);
    }
    private void Start()
    {
        _enemyModel.Subscribe(this);
        Subscribe();
        InitTree();
        FsmInit();
    }

    void Subscribe()
    {
        target.OnDie += PlayerDead;
    }

    #region FSM
    private void FsmInit()
    {
        //--------------- FSM Creation -------------------//     
        //------States creation
        var idle = new EnemyIdleState<EnemyStates>(_idleLenght, IdleCommand,IsInSight,IdleTime,_root);
        var patrol = new EnemyPatrolState<EnemyStates>(_transform,IdleTime,IsInSight,waypoints,Behaviour,PatrolCommand,minPatrolDist, _root);
        var follow = new EnemyFollowState<EnemyStates>(target.transform,_root,Behaviour,_chaseTimer,FollowCommand, IdleTime);
        var attack = new EnemyAttackState<EnemyStates>(_enemyModel,_attackCd,AttackCommand,_root);
        
        //------ Idle
        idle.AddTransition(EnemyStates.Follow, follow);
        idle.AddTransition(EnemyStates.Patrol, patrol);
     
        //------ Follow -- 
        follow.AddTransition(EnemyStates.Patrol, patrol);
        follow.AddTransition(EnemyStates.Idle,idle);
        follow.AddTransition(EnemyStates.Attack, attack);
       
        // ----- Patrol ---
        patrol.AddTransition(EnemyStates.Idle, idle);
        patrol.AddTransition(EnemyStates.Follow,follow);
        
        // ----- Attack ---
        attack.AddTransition(EnemyStates.Follow, follow);
        attack.AddTransition(EnemyStates.Patrol, patrol);
        follow.AddTransition(EnemyStates.Idle,idle);
        
        
        _fsm = new FSM<EnemyStates>();
        // Set init state
        _fsm.SetInit(idle);

    }
    #endregion

   
    #region DecitionTree
    void InitTree(){
    // Actions

          iNode follow = new ActionNode(()=> _fsm.Transition(EnemyStates.Follow));
          iNode patrol = new ActionNode(()=> _fsm.Transition(EnemyStates.Patrol));
          iNode attack = new ActionNode(()=> _fsm.Transition(EnemyStates.Attack));
          iNode idle = new ActionNode(() => _fsm.Transition(EnemyStates.Idle));
        
    //Questions
          var isInIdle = new QuestionNode(IsInIdle,idle ,patrol ); 
          var isInSight = new QuestionNode(IsInSight,follow,isInIdle);       
          var isInRange = new QuestionNode(IsInRange,attack,isInSight); 
          var isPlayerAlive = new QuestionNode(IsPlayerAlive,isInRange , isInIdle);
          _root = isPlayerAlive;
    }
    #region Commands

    public void FollowCommand(Vector3 dir)
    {
        OnFollow?.Invoke(dir);
    }

    public void PatrolCommand(Vector3 dir)
    {
        OnPatrol?.Invoke(dir);
    }

    public void AttackCommand(int dmg)
    {
        OnAttack?.Invoke(dmg);
    }

    public void IdleCommand()
    {
        OnIdle?.Invoke();
    }

    #endregion

   
    #region Questions
    bool IsInSight()
    {
//        var teveo = _enemyModel.LineOfSight.SingleTargetInSight(target.transform);
        // if (teveo)
        // {
        //     Debug.Log("IsInSight " + teveo);
        // }
        if (!IsPlayerAlive()) return false;
        return _enemyModel.LineOfSight.SingleTargetInSight(target.transform);
    }

    public void IdleTime(bool idleChange)
    {
        _isIdle = idleChange;
    }
    public bool IsPlayerAlive()
    {
        //Debug.Log("TargetCheck " +target.gameObject.activeSelf );
        return target != null;
    }

    public void PlayerDead()
    {
        target = null;
    }
    public bool IsInIdle()
    {
        return _isIdle;
    }

    bool IsInRange()
    {
         var estaenrango = Vector3.Distance(transform.position, target.transform.position) ;
         // Debug.Log("Esta en rango " + estaenrango);
         // Debug.Log(" rango " + attackRange);
        return (Vector3.Distance(transform.position, target.transform.position) < attackRange);
    }

    #endregion
    
    #endregion

    void Update()
    {
        _fsm.OnUpdate();
    }
    
}
