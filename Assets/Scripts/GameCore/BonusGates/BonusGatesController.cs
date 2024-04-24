using MathRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BonusGatesController
{
    [Inject] private PrefabHolder _prefabHolder;
    [Inject] private Settings _settings;
    [Inject] private GameFieldHelper _gameFieldHelper;
    [Inject] private MathManager mathManager;
    [Inject] private DiContainer _diContainer;
    private PoolManager _bonusGatePoolManager;
    private PoolManager _mathGatePoolManager;
    private int _bonusGateCounter = 0;

    public int BonusGateCounter
    {
        get => _bonusGateCounter;
        set
        {
            _bonusGateCounter = value;
            if (_bonusGateCounter > _settings.bonusGatesList.Count - 1)
            {
                _bonusGateCounter = 0;
            }
        }
    }

    private int _mathGateCounter = 0;          //  temp test beaar

    public int MathGateCounter
    {
        get => _mathGateCounter;
        set
        {
            _mathGateCounter = value;
            if (_mathGateCounter >= _settings.startingBlockNumber - 1)
            {
                _mathGateCounter = 0;
                mathManager.CreateMathRoomModel(_settings.startingBlockNumber);
            }
        }
    }

    [Inject]
    public void Setup()
    {
        _bonusGatePoolManager = new PoolManager(_prefabHolder.bonusGatePrefab, _settings.startingBlockNumber * 2, _gameFieldHelper, _diContainer);
        _mathGatePoolManager = new PoolManager(_prefabHolder.mathGatePrefab, _settings.startingBlockNumber * 2, _gameFieldHelper, _diContainer);
        bool math = _settings.gameMode == GameMode.mathMode;
        if (math)
        {
            mathManager.CreateMathRoomModel(_settings.startingBlockNumber);
        }
    }

    public BonusGate GetNextGate()
    {
        var gate = _bonusGatePoolManager.GetPoolItem<BonusGate>();
        BonusGateArgs args = _settings.bonusGatesList[BonusGateCounter++];
        gate.SetGateSettings(args);
        return gate;
    }

    public MathGate GetNextMathGate()              //  temp test bear
    {
        var gate = _mathGatePoolManager.GetPoolItem<MathGate>();
        BaseExampleModel currentTask = mathManager.MathRoomModel.GetTasksList[MathGateCounter++];
        MathGateArgs args = new MathGateArgs(currentTask.GetTask(), currentTask.GetAnswer());
        gate.SetGateSettings(args);
        return gate;
    }

    public BonusGate GetNextTemplatedGate(BonusGateArgs bonusArgs)
    {
        var bonusGate = _bonusGatePoolManager.GetPoolItem<BonusGate>();
        bonusGate.SetGateSettings(bonusArgs);
        return bonusGate;
    }

    public void ReleaseGate(AbstractGate gate)
    {
        _bonusGatePoolManager.ReleaseItem(gate);
    }
}
