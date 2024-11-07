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
    [Inject] private UICanvasRoot _rootCanvas;
    [Inject] private GameFieldHelper _gameFieldHelper;
    [Inject] private MathManager _mathManager;//TODOSALT наименование полей
    [Inject] private DiContainer _diContainer;
    private GameMode _gameMode;
    private PoolManager _currentPoolManager;
    private PoolManager _bonusGatePoolManager;
    private PoolManager _mathGatePoolManager;
    private GameUIController _gameUIController;
    private int _bonusGateCounter = 0;
    private List<AbstractGate> _gates = new List<AbstractGate>();
    private AbstractGate _nextGate;


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

    private int _mathGateCounter = 0;          //  temp test beaar//TODOSALT remove ??

    public int MathGateCounter
    {
        get => _mathGateCounter;
        set
        {
            _mathGateCounter = value;
            if (_mathGateCounter == (_settings.startingBlockNumber - 1))
            {
                _mathGateCounter = 0;
                _mathManager.CreateMathRoomModel(_settings.startingBlockNumber);
            }
        }
    }

    public void Restart(GameMode gameMode)
    {
        _gates.Clear();
        _gameMode = gameMode;
        if (_gameUIController == null)
        {
            _gameUIController = _rootCanvas.gameUIController;
        }
        //TODOSALT вынести в Init/Setup. от маинлогик могут пройти иниты контроллеров

        RestartPoolManager();
        InitMath();
    }

    private void RestartPoolManager()
    {
        if (_gameMode == GameMode.mathMode)
        { 
            if (_mathGatePoolManager == null)
            {
                _mathGatePoolManager = new PoolManager(_prefabHolder.mathGatePrefab, _settings.startingBlockNumber * 2, _gameFieldHelper, _diContainer);
            }
            _currentPoolManager = _mathGatePoolManager;
        }
        else
        {
            if (_bonusGatePoolManager == null)
            {
                _bonusGatePoolManager = new PoolManager(_prefabHolder.bonusGatePrefab, _settings.startingBlockNumber * 2, _gameFieldHelper, _diContainer);
            }
            _currentPoolManager = _bonusGatePoolManager;
        }
    }

    private void InitMath()
    {
        if (_gameMode == GameMode.mathMode)
        { 
            _mathManager.CreateMathRoomModel(_settings.startingBlockNumber);
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

    public MathGate CreateNextMathGate()             //TODOSALT  //  temp test bear
    {
        var gate = _mathGatePoolManager.GetPoolItem<MathGate>();
        BaseExampleModel currentTask = _mathManager.MathRoomModel.GetTasksList[MathGateCounter++];//TODOSALT изменить на "дай пример" позвать Солёного. Смысл - убрать массив. 
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
        _gates.Remove(gate);
        _currentPoolManager.ReleaseItem(gate);
    }

    public void SetGateCrossed (AbstractGate gate)//TODOSALT AbstractGate gate не использует? убрать
    {
        SetNextUnsolvedGate();
    }

    public void SetNextUnsolvedGate()
    {
        _nextGate = _gates.Where(x => x.IsPassed == false).First();
        SetUnsolvedMathGate(_settings.gameMode == GameMode.mathMode, _nextGate);
    }

    private void SetUnsolvedMathGate(bool isMathMode, AbstractGate abstractGate)
    {
        if (!isMathMode) return;
        MathGate nextMathGate = (MathGate)abstractGate;
        if (nextMathGate == null)
        {
            Debug.LogError ("Can't convert gate to math gate");
            return;
        }
        _gameUIController.OnTaskChanging(nextMathGate);
    }
}
