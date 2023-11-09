using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AdditionPlankOperator : GateOperator
{
    public override void ChangePlankNumber(PlankChangerActor plankChangerActor)
    {
        if (!isInteractable) return;
        base.ChangePlankNumber(plankChangerActor);
        plankChangerActor.PlankNumber += _modifier;
    }

    public override string GetActionSymbol() {
        return "+";
    }
}

