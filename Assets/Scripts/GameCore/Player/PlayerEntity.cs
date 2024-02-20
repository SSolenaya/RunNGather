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

public interface PlankChangerActor
{
    int PlankNumber { get; set; }
}


public class PlayerEntity : MonoBehaviour, PlankChangerActor
{
    [Inject] private PrefabHolder _prefabHolder;
    [Inject] private Settings _settings;
    [Inject] private CharacterModelsController _characterModelsController;
    [Inject] private AudioController _audioController;
    [SerializeField] private Transform _parentForView;
    public ReactiveProperty<float> xLocalPos = new ReactiveProperty<float>();
    private int startingPlankNumber = 0;            //  for testing (value for build version = 0)
    private int _plankNum;
    public int PlankNumber
    {
        get => _plankNum;
        set
        {
            value = value < 0 ? 0 : value;
            _plankNum = value;
            _planksNumText.text = _plankNum.ToString();
        }
    }

    public RoadBlock CurrentRoadBlock { get => _currentRoadBlock; set => _currentRoadBlock = value; }

    [SerializeField] private TMP_Text _planksNumText;
    [SerializeField] private SwipeMovingController _swipeController;
    [SerializeField] private PlayerCollisionController _collisionController;
    private int _speed;
    private PlayerState _playerGameState = PlayerState.Idle;
    private PlayerControlState _playerControlState = PlayerControlState.controlled;
    private CharacterAnimator _characterAnimator;
    private Vector3 _currentDirectionV3;
    private RoadBlock _currentRoadBlock;
    private Vector3 _currentPlankPlace = Vector3.zero;
    private List<Plank> _planksList = new List<Plank>();
    private PoolManager _plankPoolManager;
    private Action _onFallingAct;
    [SerializeField] private LayerMask _layerMask;
    private BasePlayerEntityState _currentState;
    private Dictionary<Type, BasePlayerEntityState> _entityStates = new Dictionary<Type, BasePlayerEntityState>();

    public void Setup(PoolManager poolManager)
    {
        _plankPoolManager = poolManager;
        _currentDirectionV3 = Vector3.left;
        //Observable.EveryUpdate().Subscribe(_ => SetNewLocalPos()).AddTo(this);
        _speed = _settings.playerSpeed;
        SetView();
        InitialPlayerEntityStates();
        _swipeController.Setup(this);
        _collisionController.Setup(this);
        SetPlayerState<IdlePlayerEntityState>();
    }

    private void InitialPlayerEntityStates()
    {
        _entityStates.Add(typeof(IdlePlayerEntityState), new IdlePlayerEntityState(this)); 
        _entityStates.Add(typeof(RunPlayerEntityState), new RunPlayerEntityState(this));
        _entityStates.Add(typeof(BuildPlayerEntityState), new BuildPlayerEntityState(this));
        _entityStates.Add(typeof(FallPlayerEntityState), new FallPlayerEntityState(this));
        _entityStates.Add(typeof(WinPlayerEntityState), new WinPlayerEntityState(this));
    }

    public CharacterAnimator GetCharacterAnimator() { return _characterAnimator; }

    
    void Update()
    {
        if (_currentState != null)
        {
            _currentState.OnUpdateState();
        }
    }

    public void Running()
    {
        SendRay();
        Move();
        SetNewLocalPos();
    }

    public void Fall()
    {
        Move();
    }

    private void SetView()
    {
        _characterAnimator = _characterModelsController.GetModelByType(_settings.currentCharType);           //  TODO: Get model by logic
        _characterAnimator.transform.SetParent(_parentForView);
        _characterAnimator.transform.localScale = Vector3.one;
        _characterAnimator.transform.localPosition = Vector3.zero;
        _characterAnimator.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
    }

    private void Move()
    {
       transform.Translate(_currentDirectionV3 * _speed * Time.deltaTime);
        if (transform.position.y < -15f)
        {
            _onFallingAct?.Invoke();
        }
    }

    private void SetNewLocalPos()
    {
        if (transform.localPosition.x == xLocalPos.Value) return;
        xLocalPos.SetValueAndForceNotify(transform.localPosition.x);
    }

    public void SubscribeForFalling(Action act)
    {
        _onFallingAct += act;
        
    }

    private void SendRay()
    {
        Ray ray = new Ray(transform.position + Vector3.up*1f, Vector3.down);
        Debug.DrawRay(ray.origin, ray.direction * 5, Color.green);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, 20, _layerMask))
        {
           SetPlayerState<BuildPlayerEntityState>();
           SetControlState(PlayerControlState.uncontrolled);
            
        } else
        {
            if (_currentRoadBlock != null)
            {
                return;
            }
            RoadBlock hitBlock = hit.collider.gameObject.GetComponentInParent<RoadBlock>();
            if (hitBlock != null)
            {
                _currentRoadBlock = hitBlock;
                SetControlState(PlayerControlState.controlled);
            }
            
        }
    }

    private Tween fallingTween;

    public void SetPlayerState<T>()
    {
        if (_currentState is T) return;
        _currentState = _entityStates[typeof(T)];
        _currentState.OnEnterState();
    }


    public void Building()
    {
        if (PlankNumber > 0)
        {
            if (_currentRoadBlock != null)
            {
                _currentPlankPlace = new Vector3(_currentRoadBlock.GetBlockEndXPos() + 0.25f, 0.95f, transform.position.z);
            }
            BuildPlank();
            _currentRoadBlock = null;
            SetPlayerState<RunPlayerEntityState>();
        }
        else
        {
            SetPlayerState<FallPlayerEntityState>();
        }
    }

    public void ChangeDirectionOnFalling()
    {
        fallingTween = DOVirtual.Float(0, _speed * 5, 1f, var => { _currentDirectionV3 = Vector3.down * var; }).SetEase(Ease.OutQuad);
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

    private void BuildPlank()
    {
        Plank plank = _plankPoolManager.GetPoolItem<Plank>();
        plank.SetupPoolManager(_plankPoolManager);
        _currentPlankPlace += Vector3.left * 0.45f;
        plank.transform.position = _currentPlankPlace;
        plank.gameObject.SetActive(true);
        _audioController.PlayBuildingSound();
        plank.gameObject.name = "Plank_" + _planksList.Count;
        xLocalPos.Subscribe(x => plank.CheckForHiding(x, _settings.roadStartDistance, () => _planksList.Remove(plank)));
        _planksList.Add(plank);
        PlankNumber--;
    }

    public void Restart()
    {
        fallingTween?.Kill();
        PlankNumber = startingPlankNumber;
        ClearExistingPlanks();
        SetPlayerState<IdlePlayerEntityState>();
        transform.localPosition = new Vector3(-0.25f, 1f, 0);
        xLocalPos.SetValueAndForceNotify(transform.localPosition.x);
        _currentDirectionV3 = Vector3.left;
    }

    public void ClearExistingPlanks()
    {
        if (_planksList.Count == 0) return;
        foreach (var plank in _planksList)
        {
            plank.SendItemToPool();
        }
        _planksList.Clear();
    }

    public PlayerState GetGameState()
    {
        return _playerGameState;
    }

    public PlayerControlState GetControlState()
    {
        return _playerControlState;
    }

    public void OnPlankNumberChange(int newNumber)
    {
        newNumber = newNumber < 0 ? 0 : newNumber;
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
