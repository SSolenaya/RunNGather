using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;


public class PlayerEntity : MonoBehaviour
{
    [Inject] private PrefabHolder _prefabHolder;//TODOSALT
    [Inject] private Settings _settings;
    [Inject] private CharacterModelsController _characterModelsController;
    [Inject] private AudioController _audioController;
    [Inject] private PlanksManager _planksManager;
    public CharacterAnimator GetCharacterAnimator => _characterAnimator;
    public LayerMask layerMask;
    public ReactiveProperty<float> xLocalPos => _movingEntity.xLocalPos;
    private RoadBlock _currentRoadBlock;
    public RoadBlock CurrentRoadBlock { get => _currentRoadBlock; set => _currentRoadBlock = value; }
    public bool IsControlled => _currentRoadBlock != null && _currentState is RunPlayerEntityState;

    [SerializeField] private Transform _parentForView;
    [SerializeField] private TMP_Text _planksNumText;
    [SerializeField] private SwipeMovingController _swipeController;
    [SerializeField] private PlayerCollisionController _collisionController;
    private MovingEntity _movingEntity;
    private CharacterType _currentType = CharacterType.none;
    private PlayerState _playerGameState = PlayerState.Idle;
    private CharacterAnimator _characterAnimator;
    private PlanksCounter _planksCounter;
    private BasePlayerEntityState _currentState;
    private Dictionary<Type, BasePlayerEntityState> _entityStates = new Dictionary<Type, BasePlayerEntityState>();

    public void Setup()
    {
        SetupMovingEntity();
        SetupPlanksCounter();
        //Observable.EveryUpdate().Subscribe(_ => SetNewLocalPos()).AddTo(this);//TODOSALT
        SetView();
        SetEntityStates();
        _swipeController.Setup(this);
        _collisionController.Setup(_planksCounter);
    }

    public void Restart()
    {
        _movingEntity.Restart();
        _planksCounter.Restart();
        ResetView();
    }

    void Update()
    {
        if (_currentState != null)
        {
            _currentState.OnUpdateState();
        }
    }

    private void SetupMovingEntity()
    {
       _movingEntity = new MovingEntity(this.transform, _settings);
    }

    private void SetEntityStates()
    {
        InitialPlayerEntityStates();
        SetPlayerState<IdlePlayerEntityState>();
    }

    private void SetupPlanksCounter()
    {
        _planksCounter = new PlanksCounter(ChangePlanksCounterText);
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

    private void SetView()
    {
        _currentType = _settings.currentCharType;
        _characterAnimator = _characterModelsController.GetModelByType(_currentType);
        _characterAnimator.transform.SetParent(_parentForView);
        _characterAnimator.transform.localScale = Vector3.one;
        _characterAnimator.transform.localPosition = Vector3.zero;
        _characterAnimator.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
    }

    private void ResetView()
    {
        if (_currentType == _settings.currentCharType) return;
        Destroy(_characterAnimator.gameObject);
        SetView();
        _entityStates.Clear();
        SetEntityStates();
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

    public PlayerState GetGameState()
    {
        return _playerGameState;
    }

    //TODOSALT
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

