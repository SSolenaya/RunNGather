using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MultiplicationPlankOperator : GateOperator
{
    public override void ChangePlankNumber(PlankChangerActor plankChangerActor)
    {
        Debug.Log("Multiplication op class");
        if (!isInteractable) return;
        base.ChangePlankNumber(plankChangerActor);
        plankChangerActor.PlankNumber *= _modifier;
    }

    public override string GetActionSymbol()
    {
        return "x";
    }
}

//public class MultiplicationOperatorArgs : GateOperatorArgs
//{
//
//}


