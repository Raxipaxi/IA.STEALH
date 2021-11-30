using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : Actor
{
    #region Properties

    //Animator
    private Rigidbody _rb;
    
    public PlayerModel _target;
    private EnemyView _enemyView;
    // Actions

    public event Action OnAttack;
    public event Action OnIdle;
    public event Action OnWalk;
    public event Action OnChase;
    

    
    [SerializeField]private float walkSpeed;
    [SerializeField]private float runSpeed;
    [SerializeField] public int normaldmg;
    [SerializeField] public int criticaldmg;
    [SerializeField] private LineOfSightAI _lineOfSightAI;
    [SerializeField] private Transform hitZone;
    [SerializeField] private LayerMask _mask;

    private int damage;
    public LineOfSightAI LineOfSight => _lineOfSightAI;

    #endregion
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _enemyView = GetComponent<EnemyView>();
    }

    private void Start()
    {
        _enemyView.Subscribe(this);
    }

    public void Subscribe(EnemyController controller)
    {
        controller.OnFollow += Run;
        controller.OnPatrol += Walk;
        controller.OnAttack += Attack;
        controller.OnIdle += Idle;
    }
    #region Mobile Methods
    public override void Run(Vector3 dir)
    {
       Move(dir, runSpeed);
       LookDir(dir.normalized);
       OnChase?.Invoke();

    }

    public override void Idle()
    {
        _rb.velocity = Vector3.zero;
        OnIdle?.Invoke();
    }

    public override void Attack(int dmg)
    {
        damage = dmg;
        _rb.velocity = Vector3.zero;
        OnAttack?.Invoke();
      
    }

    public override void Walk(Vector3 dir)
    {
        Move(dir,walkSpeed);
        LookDir(dir.normalized);
        OnWalk?.Invoke();
    }

    public override void LookDir(Vector3 dir)
    {
        transform.forward = dir;
    }

    public override void Move(Vector3 dir, float speed)
    {
        dir *= speed;
        dir.y = _rb.velocity.y;
        _rb.velocity = dir;
    }

    #endregion

    public void AttackMode()
    {
        var hit = Physics.OverlapSphere(hitZone.position, 1f, _mask);
        if (hit.Length>0)
        {
            var player = hit[0].GetComponent<iDamageable>(); 
            player?.TakeDamage(damage); 
        }
        
    }

    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(hitZone.position, 1f);
    // }
}
