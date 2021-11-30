using UnityEngine;

public class EnemyView : MonoBehaviour
{
    private  Animator _enemyAnimator;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float chargeSpeed = 3.5f;
    [SerializeField] private float attackProximity = 0.3f;
    [SerializeField] private float nonAttackProximity = 10;
    
    void Awake()
    {
        _enemyAnimator = GetComponent<Animator>();
    }

    public void Subscribe(EnemyModel model)
    {
        model.OnIdle += IdleAnimation;
        model.OnWalk += WalkAnimation;
        model.OnChase += ChargeAnimation;
        model.OnAttack += AttackAnimation;
    }

    public void WalkAnimation() 
     {
        _enemyAnimator.SetFloat(EnemyAnimationParameters.Velocity,walkSpeed);
        _enemyAnimator.SetBool(EnemyAnimationParameters.InView, false);
     }

    public void IdleAnimation()
    {
        _enemyAnimator.SetFloat(EnemyAnimationParameters.Velocity,0f);
        _enemyAnimator.SetBool(EnemyAnimationParameters.InView, false);
    }

    public void ChargeAnimation() 
    {
        _enemyAnimator.SetFloat(EnemyAnimationParameters.Velocity, chargeSpeed);
        _enemyAnimator.SetBool(EnemyAnimationParameters.InView, true);
        _enemyAnimator.SetFloat(EnemyAnimationParameters.Proximity,nonAttackProximity);
    }

    public void AttackAnimation() 
    {
        Debug.Log("Te pegue");
        _enemyAnimator.Play("Attack");
    }
    
    
    
}
