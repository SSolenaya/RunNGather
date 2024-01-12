using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public interface IPlankNumberChanger
{
    void ChangePlankNumber(PlankChangerActor plankChangerActor);
}

public abstract class GateOperator : MonoBehaviour, IPlankNumberChanger
{
    protected BonusGate _parentGate;
    protected int _modifier;
    protected bool isInteractable = true;
    private Action _onBonusGateCross;

    public void Setup(int modifierValue, BonusGate parentGate)
    {
        _modifier = modifierValue;
        _parentGate = parentGate;
        _onBonusGateCross = parentGate.PlaySoundOnCroosingGate;
    }

    public int GetModifier()                                    //  TODO: property
    {
        return _modifier;
    }

    public virtual void ChangePlankNumber(PlankChangerActor plankChangerActor) 
    {
        _onBonusGateCross?.Invoke();
        _parentGate.SetInteracted();
    }

    public void SetUninteractable()
    {
        isInteractable = false;
    }

    public abstract string GetActionSymbol();
}
[Serializable]
public class GateOperatorArgs
{
    public BonusOperationTypes opType;
    public int modifier;
}

public enum BonusOperationTypes
{
    addition,
    subtraction,
    multiplication,
    division
}

public static class RandomExtensions
{
    public static T RandomEnum<T>(this Random random)
    {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(random.Next(values.Length));
    }
}

