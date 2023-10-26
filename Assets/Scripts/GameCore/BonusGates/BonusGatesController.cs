using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BonusGatesController
{
    [Inject] private PrefabHolder _prefabHolder;
    [Inject] private Settings _settings;
    [Inject] private GameFieldHelper _gameFieldHelper;
    [Inject] private DiContainer _diContainer;
    private PoolManager _bonusGatePoolManager;
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

    [Inject]
    public void Setup()
    {
        _bonusGatePoolManager = new PoolManager(_prefabHolder.bonusGatePrefab, _settings.startingBlockNumber * 2, _gameFieldHelper, _diContainer);
    }

    public BonusGate GetNextGate()
    {
        var bonusGate = _bonusGatePoolManager.GetPoolItem<BonusGate>();
        BonusGateArgs bonusArgs = _settings.bonusGatesList[BonusGateCounter++];
        bonusGate.SetGateSettings(bonusArgs);
        return bonusGate;
    }

    public BonusGate GetNextTemplatedGate(BonusGateArgs bonusArgs)
    {
        var bonusGate = _bonusGatePoolManager.GetPoolItem<BonusGate>();
        bonusGate.SetGateSettings(bonusArgs);
        return bonusGate;
    }

    public PoolManager GetBonusGatePoolManager()
    {
        return _bonusGatePoolManager;
    }


}
