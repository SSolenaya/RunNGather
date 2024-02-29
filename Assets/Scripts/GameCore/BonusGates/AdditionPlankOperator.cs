using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AdditionPlankOperator : GateOperator
{
    public override void ChangePlankNumber(PlanksCounter planksCounter)
    {
        if (!isInteractable) return;
        base.ChangePlankNumber(planksCounter);
        planksCounter.PlankNumber += _modifier;
    }

    public override string GetActionSymbol() {
        return "+";
    }
}

