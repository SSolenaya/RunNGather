using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private Action _onSuccessfulFinish;

    public void SubscribeForSuccessfulFinish(Action act)
    {
        _onSuccessfulFinish += act;
    }

    public void Release()
    {
        _onSuccessfulFinish = null;
    }

    public void SetPlayerFinished() 
    {
        _onSuccessfulFinish?.Invoke();
    }
}
