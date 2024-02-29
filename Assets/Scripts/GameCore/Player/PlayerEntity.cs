using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using UniRx.Extensions;
using Zenject;


public class PlayerEntity : MonoBehaviour
{
    [Inject] private PrefabHolder _prefabHolder;
    [Inject] private Settings _settings;
    [Inject] private CharacterModelsController _characterModelsController;
    [Inject] private AudioController _audioController;
    [Inject] private PlanksManager _planksManager;
    public CharacterAnimator GetCharacterAnimator => _characterAnimator;
    public LayerMask layerMask;
    public ReactiveProperty<float> xLocalPos => _movingEntity.xLocalPos;
    private RoadBlock _currentRoadBlock;
    public RoadBlock CurrentRoadBlock { get => _currentRoadBlock; set => _currentRoadBlock = value; }
    [SerializeField] private Transform _parentForView;
    [SerializeField] private TMP_Text _planksNumText;
    [SerializeField] private SwipeMovingController _swipeController;
    [SerializeField] private PlayerCollisionController _collisionController;
    private MovingEntity _movingEntity;
    private PlayerState _playerGameState = PlayerState.Idle;
    private PlayerControlState _playerControlState = PlayerControlState.controlled;
    private CharacterAnimator _characterAnimator;
    private PlanksCounter _planksCounter;
    private BasePlayerEntityState _currentState;
    private Dictionary<Type, BasePlayerEntityState> _entityStates = new Dictionary<Type, BasePlayerEntityState>();

    public void Setup()
    {
        SetupMovingEntity();
        SetupPlanksCounter();
        //Observable.EveryUpdate().Subscribe(_ => SetNewLocalPos()).AddTo(this);
        SetView();
        InitialPlayerEntityStates();
        _swipeController.Setup(this);
        _collisionController.Setup(_planksCounter);
        SetPlayerState<IdlePlayerEntityState>();
    }

    private void SetupMovingEntity()
    {
       _movingEntity = new MovingEntity(this.transform, _settings);
       _movingEntity.Restart();
    }

    private void SetupPlanksCounter()
    {
        _planksCounter = new PlanksCounter(ChangePlanksCounterText);
        _planksCounter.Restart();
    }

    private void ChangePlanksCounterText(int newPlanksNumber)
    {
       _planksNumText.text = newPlanksNumber.ToString();
    }

    private void InitialPlayerEntityStates()
    {
        _entityStates.Add(typeof(IdlePlayerEntityState), new IdlePlayerEntityState(this)); 
        _entityStates.Add(typeof(RunPlayerEntityState), new RunPlayerEntityState(this, _movingEntity));
        _entityStates.Add(typeof(BuildPlayerEntityState), new BuildPlayerEntityState(this, _planksCounter, _planksManager, _audioController, _settings));
        _entityStates.Add(typeof(FallPlayerEntityState), new FallPlayerEntityState(this, _movingEntity));
        _entityStates.Add(typeof(WinPlayerEntityState), new WinPlayerEntityState(this));
    }
    
    void Update()
    {
        if (_currentState != null)
        {
            _currentState.OnUpdateState();
        }
    }

    private void SetView()
    {
        _characterAnimator = _characterModelsController.GetModelByType(_settings.currentCharType);           //  TODO: Get model by logic     bear
        _characterAnimator.transform.SetParent(_parentForView);
        _characterAnimator.transform.localScale = Vector3.one;
        _characterAnimator.transform.localPosition = Vector3.zero;
        _characterAnimator.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
    }

    public void SubscribeForFalling(Action act)
    {
        _movingEntity.SubscribeForFalling(act);
        
    }

    public void SetPlayerState<T>()
    {
        if (_currentState is T) return;
        _currentState = _entityStates[typeof(T)];
        _currentState.OnEnterState();
    }

    public void SetControlState(PlayerControlState newState)
    {
        if (_playerControlState == newState)
        {
            return;
        }
        _playerControlState = newState;
        switch (newState)
        {
            case PlayerControlState.controlled:
                break;
            case PlayerControlState.uncontrolled:
                break;
        }
    }
    public void Restart()
    {
        _movingEntity.Restart();
        _planksCounter.Restart();
        SetPlayerState<IdlePlayerEntityState>();
    }

    public PlayerState GetGameState()
    {
        return _playerGameState;
    }

    public PlayerControlState GetControlState()
    {
        return _playerControlState;
    }

    //public void IncreasePlanksNumber(int count)
    //{
    //    plankDataReactProperty.Value.plankNumber += count*_settings.planksPoints;
    //}

}

public enum PlayerState
{
    Idle,
    Run,
    Build,
    Fall,
    Win
}




public enum PlayerControlState
{
    controlled,
    uncontrolled
}
