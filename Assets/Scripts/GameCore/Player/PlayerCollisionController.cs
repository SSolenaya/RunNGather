using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    private PlanksCounter _plankCounter;


    public void Setup(PlanksCounter plankCounter)
    {
        _plankCounter = plankCounter;
    }

    public void OnTriggerEnter(Collider col)
    {
        var t = col.gameObject.GetComponent<IPlankNumberChanger>();
        if (t != null)
        {
            t.ChangePlankNumber(_plankCounter);
        }
    }

    public void OnTriggerExit(Collider col)
    {
        var f = col.gameObject.GetComponentInParent<FinishLine>();
        if (f != null)
        {
            f.SetPlayerFinished();
        }
    }
}
