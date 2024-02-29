using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BonusGate : MonoBehaviour, IPoolItem
{

    [Inject] private GameFieldHelper _gameFieldHelper;
    [Inject] private PrefabHolder _prefabHolder;
    [Inject] private Settings _settings;
    [Inject] private AudioController _audioController;
    [SerializeField] private Transform _leftTextPos;
    [SerializeField] private Transform _rightTextPos;
    [SerializeField] private GameObject _leftGate;
    [SerializeField] private GameObject _rightGate;
    private GateOperator _leftBonus;
    private GateOperator _rightBonus;
    private BonusText _leftBonusText;
    private BonusText _rightBonusText;
    private string _parentBlockName;
    private BonusGateArgs _currentGatesSettings;

    bool IPoolItem.IsInPool { get; set; }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Release()
    {
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

    public void SetGateSettings(BonusGateArgs args)
    {
        _currentGatesSettings = args;
    }

    public void SetupGates()
    {
        SetupLeftGate(_currentGatesSettings.leftGateArgs);
        SetupRightGate(_currentGatesSettings.rightGateArgs);
    }


    private void SetupLeftGate(GateOperatorArgs args)
    {
        _leftBonus = SetOperationToGate(_leftGate, args.opType);
        _leftBonus.Setup(args.modifier, this);
        _leftBonusText = Instantiate(_prefabHolder.bonusTextPrefab, _gameFieldHelper.worldCanvas);
        _leftBonusText.transform.position = _leftTextPos.position;
        _leftBonusText.gameObject.name = "Left_text_"+ _parentBlockName;
        _leftBonusText.Setup(_leftBonus);
    }

    private void SetupRightGate(GateOperatorArgs args)
    {
        _rightBonus = SetOperationToGate(_rightGate, args.opType);
        _rightBonus.Setup(args.modifier, this);
        _rightBonusText = Instantiate(_prefabHolder.bonusTextPrefab, _gameFieldHelper.worldCanvas);
        _rightBonusText.transform.position = _rightTextPos.position;
        _rightBonusText.gameObject.name = "Right_text_" + _parentBlockName;
        _rightBonusText.Setup(_rightBonus);
    }

    public void SetParentBlockName(string name)
    {
        _parentBlockName = name;
    }

    public GateOperator SetRandomGate(GameObject go)
    {
        var random = new System.Random();
        var randomOpType = random.RandomEnum<BonusOperationTypes>();
        return SetOperationToGate(go, randomOpType);
    }

    public GateOperator SetOperationToGate(GameObject go, BonusOperationTypes operationType)
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

    public void SetInteracted()
    {
        _leftBonus.SetUninteractable();
        _rightBonus.SetUninteractable();
    }

    public void PlaySoundOnCrossingGate()
    {
        _audioController.PlayBonusPickingSound();
    }
}

[Serializable]
public class BonusGateArgs
{
    public GateOperatorArgs leftGateArgs;
    public GateOperatorArgs rightGateArgs;
}
