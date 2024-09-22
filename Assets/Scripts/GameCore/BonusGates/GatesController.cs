using MathRoom;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;
using System.Linq;

public class GatesController
{
    [Inject] private PrefabHolder _prefabHolder;
    [Inject] private Settings _settings;
    [Inject] private GameFieldHelper _gameFieldHelper;
    [Inject] private MathManager mathManager;
    [Inject] private DiContainer _diContainer;
    private PoolManager _bonusGatePoolManager;
    private PoolManager _mathGatePoolManager;
    private int _bonusGateCounter = 0;
    private List<AbstractGate> _gates = new List<AbstractGate>();
    private ReactiveProperty<AbstractGate> nextGate = new ReactiveProperty<AbstractGate>();


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

    public void Restart(GameMode gameMode)
    {
        _gates.Clear();
        if (_bonusGatePoolManager == null)
        {
            _bonusGatePoolManager = new PoolManager(_prefabHolder.bonusGatePrefab, _settings.startingBlockNumber * 2, _gameFieldHelper, _diContainer);
        }
        
        if (gameMode == GameMode.mathMode)
        {
            if (_mathGatePoolManager == null)
            {
                _mathGatePoolManager = new PoolManager(_prefabHolder.mathGatePrefab, _settings.startingBlockNumber * 2, _gameFieldHelper, _diContainer);
            }
            mathManager.CreateMathRoomModel(_settings.startingBlockNumber);
        }
    }

    public BonusGate CreateNextGate()
    {
        var gate = _bonusGatePoolManager.GetPoolItem<BonusGate>();
        BonusGateArgs args = _settings.bonusGatesList[BonusGateCounter++];
        gate.SetGatesController(this);
        gate.SetGateSettings(args);
        _gates.Add(gate);
        return gate;
    }

    public MathGate CreateNextMathGate()              //  temp test bear
    {
        var gate = _mathGatePoolManager.GetPoolItem<MathGate>();
        BaseExampleModel currentTask = mathManager.MathRoomModel.GetTasksList[MathGateCounter++];
        Debug.LogError(currentTask.GetTask() + " counter " + MathGateCounter);
        MathGateArgs args = new MathGateArgs(currentTask.GetTask(), currentTask.GetAnswer());
        gate.SetGateSettings(args);
        gate.SetGatesController(this);
        _gates.Add(gate);
        return gate;
    }

    public BonusGate CreateNextTemplatedGate(BonusGateArgs bonusArgs)
    {
        var bonusGate = _bonusGatePoolManager.GetPoolItem<BonusGate>();
        bonusGate.SetGateSettings(bonusArgs);
        bonusGate.SetGatesController(this);
        _gates.Add(bonusGate);
        return bonusGate;
    }

    public void ReleaseGate(AbstractGate gate)
    {
        _bonusGatePoolManager.ReleaseItem(gate);
    }

    public void SubscribeForCurrentGate(Action<AbstractGate> act)
    {
        nextGate.Subscribe(value => act?.Invoke(value));
    }

    public void SetNextGate()
    {
        nextGate.Value = _gates.Where(x => x.IsPassed == false).First();
    }
}
