using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class AbstractGate : MonoBehaviour, IPoolItem
{
    private bool _isPassed;
    public bool IsPassed => _isPassed;
    [Inject] protected GameFieldHelper _gameFieldHelper;
    [Inject] protected PrefabHolder _prefabHolder;
    [Inject] protected Settings _settings;
    [Inject] protected AudioController _audioController;
    [Inject] protected UICanvasRoot _rootCanvas;
    [SerializeField] protected Transform _leftTextPos;
    [SerializeField] protected Transform _rightTextPos;
    [SerializeField] protected GameObject _leftGate;
    [SerializeField] protected GameObject _rightGate;
    protected GateOperator _leftBonus;
    protected GateOperator _rightBonus;
    protected BonusText _leftBonusText;
    protected BonusText _rightBonusText;
    protected string _parentBlockName;
    protected GatesController _gatesController;
    bool IPoolItem.IsInPool { get; set; }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void SetParentBlockName(string name)
    {
        _parentBlockName = name;
    }

    public void SetGatesController(GatesController gatesController)
    {
        _gatesController = gatesController;
    }

    private void SetInteracted()
    {
        _leftBonus.SetUninteractable();
        _rightBonus.SetUninteractable();
    }

    public void PlaySoundOnCrossingGate()
    {
        _audioController.PlayBonusPickingSound();
    }

    protected GateOperator SetOperationToGate(GameObject go, BonusOperationTypes operationType)
    {
        switch (operationType)
        {
            case BonusOperationTypes.addition:
                return go.AddComponent<AdditionPlankOperator>();
            case BonusOperationTypes.subtraction:
                return go.AddComponent<SubtractionPlankOperator>();
            case BonusOperationTypes.multiplication:
                return go.AddComponent<MultiplicationPlankOperator>();
            case BonusOperationTypes.division:
                return go.AddComponent<DivisionPlankOperator>();
            default:
                return go.AddComponent<AdditionPlankOperator>();
        }
    }

    public virtual void SetupGates()
    {
        SetupLeftSemiGate();
        SetupRightSemiGate();
    }

    public void OnGateCrossing()
    {
        SetInteracted();
        PlaySoundOnCrossingGate();
        _isPassed = true;
        _gatesController.SetNextGate();
    }

    public virtual void Release()
    {
        _isPassed = false;
        if (_leftBonusText != null)
        {
            Destroy(_leftBonusText.gameObject);
        }
        if (_rightBonusText != null)
        {
            Destroy(_rightBonusText.gameObject);
        }
        Destroy(_leftBonus);
        Destroy(_rightBonus);
    }

    public abstract void SetGateSettings(AbstractGateArgs args);
    protected abstract void SetupLeftSemiGate();
    protected abstract void SetupRightSemiGate();
}
