using System;
using UnityEngine;


public class Actor : MonoBehaviour, iDamageable, iMobile
{
    public float CurrentLife { get; }
    public float MaxLife { get; }

    #region iDamageable

    public virtual void TakeDamage(float x)
    {
        throw new NotImplementedException();
    }

    public virtual void Die()
    {
        throw new NotImplementedException();
    }
    public virtual void Idle()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region iMobile

    public virtual void Walk(Vector3 dir)
    {
        throw new NotImplementedException();
    }

    public virtual void Attack(int dmg)
    {
        throw new NotImplementedException();
    }

    public virtual void LookDir(Vector3 dir)
    {
        throw new NotImplementedException();
    }

    public virtual void Run(Vector3 dir)
    {
        throw new NotImplementedException();
    }

    public virtual void Move(Vector3 dir, float speed)
    {
        throw new NotImplementedException();
    }

    public virtual void Chase()
    {
        throw new NotImplementedException();
    }
    public virtual bool Patrol()
    {
        return true;
    }
    #endregion
    
}
