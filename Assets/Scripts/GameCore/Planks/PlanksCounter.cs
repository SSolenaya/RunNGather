using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanksCounter
{
    private int startingPlankNumber = 0;            //  for testing (value for build version = 0)
    private int _plankNum;
    private Action<int> _onPlanksNumberChange;
    public int PlankNumber
    {
        get => _plankNum;
        set
        {
            value = Mathf.Clamp(value, 0, value);
            _plankNum = value;
            _onPlanksNumberChange?.Invoke(value);
            
        }
    }

    public PlanksCounter(Action<int> act)
    {
        _onPlanksNumberChange = act;
    }

    public void Restart()
    {
        PlankNumber = startingPlankNumber;
    }

    public void IncreasePlanksNumber()
    {
        //_audioController.PlayGatheringSound();
        PlankNumber++;
    }

    public void DecreasePlanksNumber()
    {
        PlankNumber--;
    }

    public void ChangePlanksNumber(int delta)
    {
        PlankNumber += delta;
    }
}
