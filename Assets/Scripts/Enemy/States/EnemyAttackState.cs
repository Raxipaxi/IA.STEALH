using System;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAttackState<T> : State<T>
{
    #region Properties

    private EnemyModel _enemyModel;
    
    // Attack dmg
    private Dictionary<int, float> damageProb;
    private iNode _root;
    private float _attackCD;
    private float _counter;
    private Action<int> _onAttack;
    #endregion

    public EnemyAttackState(EnemyModel enemyModel,float attackCd,Action<int> onAttack, iNode root)
    {
        _enemyModel = enemyModel;
        _root = root;
        _attackCD = attackCd;
        _onAttack = onAttack;


    }

    public override void Awake()
    {
        // Set values in the Damage dictionary dmg/%
        damageProb = new Dictionary<int, float>();
        damageProb.Add(_enemyModel.normaldmg,90);
        damageProb.Add(_enemyModel.criticaldmg,15); 
        damageProb.Add(0,1); 

        //ResetCooldown();
    }

    public override void Execute()
    {
        Debug.Log("Attack");
        // Prob to attack a critical hit
        Roulette roulette = new Roulette();
        var damage = roulette.Run(damageProb);
        
        if (_counter < Time.time)
        {
            _onAttack?.Invoke(damage);
            ResetCooldown();
        }
            

        _root.Execute();
    }

    void ResetCooldown()
    {
        _counter = _attackCD + Time.time;
    }
}
