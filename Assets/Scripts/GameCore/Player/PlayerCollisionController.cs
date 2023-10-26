using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    private PlankChangerActor _plankChangerActor;


    public void Setup(PlankChangerActor plankChangerActor)
    {
       _plankChangerActor = plankChangerActor;
    }

    public void OnTriggerEnter(Collider col)
    {
        var t = col.gameObject.GetComponent<IPlankNumberChanger>();
        if (t != null)
        {
            t.ChangePlankNumber(_plankChangerActor);
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
