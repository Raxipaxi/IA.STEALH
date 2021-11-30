using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalEvent : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;


    private void OnTriggerEnter(Collider other)
    {
        var winner = other.GetComponent<PlayerModel>();
        if (winner!=null)
        {
            winScreen.SetActive(true);
        }
    }
}
