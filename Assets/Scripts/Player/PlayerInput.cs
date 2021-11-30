using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour, iInput
{
    #region Properties
    private float _h;
    private float _v;
    public float GetH => _h;
    public float GetV => _v;
    #endregion

    #region Methods
    public bool IsMoving()
    {
        UpdateInputs();
        // Verifies Movement
        return (_h != 0 || _v != 0);
    }


    public void UpdateInputs()
    {
        _h = Input.GetAxis("Horizontal");
        _v = Input.GetAxis("Vertical");
    }

    #endregion
 


}
