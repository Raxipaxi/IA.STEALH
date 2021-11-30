using System;
using UnityEngine;

public class PlayerModel : Actor, IVel
{
    #region Properties

    private Rigidbody _rb;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private PlayerView _animation;

    private Transform _transform;
    // Damageable properties
    float CurrentLife => life;
    [Header("Current Life")]
    [SerializeField]private float life = 10;
    // public float MaxLife => maxLife;
    // [Header("Maximum Life")]
    [SerializeField] private float maxLife = 10;
    public event Action OnDie;
    private float _currSpeed;
    private Quaternion _currRot;
    #endregion
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animation = GetComponent<PlayerView>();
        _transform = transform;
    }

    public void Subscribe(PlayerController controller)
    {
        controller.OnIdle += Idle;
        controller.OnMove += Walk;
    }
    #region Mobile Methods
    public override void Walk(Vector3 dir)
    {
        Move(dir, walkSpeed);
        _currSpeed = walkSpeed;
        _animation.WalkAnimation(walkSpeed);
    }

    public override void LookDir(Vector3 dir)
    {
        if (dir!=Vector3.zero)
        {
            _transform.localRotation *= Quaternion.Euler(dir.x * Time.deltaTime * 360,0,0);
        }
       
       
         _transform.forward = dir;
    }

    public override void Idle() 
    {
        _rb.velocity = Vector3.zero;
        
        _animation.IdleAnimation();
    }

    public override void Move(Vector3 dir, float speed)
    {
        
      //  dir.y = _rb.velocity.y;
        var forw = Vector3.Lerp(transform.position,dir.normalized,1);
        _rb.velocity = new Vector3(dir.x*speed,_rb.velocity.y,dir.z*speed);
        
        LookDir(dir);
        
    }

    public override void Run(Vector3 dir)
    {
       Move(dir,runSpeed);
       _currSpeed = runSpeed;
    }

    #endregion

    #region Damageable Methods
    public override void Die()
    {
        OnDie?.Invoke();
        Destroy(gameObject);
    }

    public override void TakeDamage(float dmg)
    {
        life -= dmg;
        if (CurrentLife <=0)
        {
            Die();
        }
    }

    #endregion

    public float Vel => _currSpeed;
}
