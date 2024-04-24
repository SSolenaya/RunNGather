using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MathGateArgs : AbstractGateArgs
{
    public string gateTask;
    public MathGateOperatorArgs leftGateArgs;
    public MathGateOperatorArgs rightGateArgs;

    public MathGateArgs(string task, int correctAnswer) {
        gateTask = task;
        MathGateOperatorArgs correctArgs = new MathGateOperatorArgs();
        MathGateOperatorArgs wrongArgs = new MathGateOperatorArgs();
        correctArgs.opType = BonusOperationTypes.addition;
        wrongArgs.opType = BonusOperationTypes.subtraction;
        correctArgs.modifier = Settings.planksMathModifier;
        wrongArgs.modifier = Settings.planksMathModifier;
        correctArgs.visibleValue = correctAnswer;
        do
        {
            wrongArgs.visibleValue = UnityEngine.Random.Range(0, 100);
        } while (wrongArgs.visibleValue == correctAnswer);

        float r = UnityEngine.Random.Range(0, 1f);
        leftGateArgs = r >= 0.5? correctArgs : wrongArgs;
        rightGateArgs = r >= 0.5 ? wrongArgs : correctArgs;
    }
}
