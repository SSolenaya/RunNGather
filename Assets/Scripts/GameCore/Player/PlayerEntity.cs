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
    public ReactiveProperty<float> xLocalPos = new ReactiveProperty<float>();
    private int startingPlankNumber = 1000;            //  for testing (value for build version = 0)
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
    [SerializeField] private TMP_Text _planksNumText;
    [SerializeField] private SwipeMovingController _swipeController;
    [SerializeField] private PlayerCollisionController _collisionController;
    private Settings _settings;
    private PrefabHolder _prefabHolder;
    private int _speed;
    private PlayerState _playerGameState = PlayerState.none;
    private PlayerControlState _playerControlState = PlayerControlState.controlled;
    private Vector3 _currentDirectionV3;
    private RoadBlock _currentRoadBlock;
    private Vector3 _currentPlankPlace = Vector3.zero;
    private List<Plank> _planksList = new List<Plank>();
    private PoolManager _plankPoolManager;
    private Action _onFallingAct;
    

    public void Setup(Settings settings, PrefabHolder prefabHolder,PoolManager poolManager)
    {
        _settings = settings;
        _prefabHolder = prefabHolder;
        _plankPoolManager = poolManager;
        _speed = _settings.playerSpeed;
        _currentDirectionV3 = Vector3.left;
        //Observable.EveryUpdate().Subscribe(_ => SetNewLocalPos()).AddTo(this);
        _swipeController.Setup(this);
        _collisionController.Setup(this);
    }

    
    void Update()
    {
        switch (_playerGameState)
        {
            case PlayerState.run:
                SendRay();
                Move();
                break;
            case PlayerState.fall:
                Move();
                break;
        }
        SetNewLocalPos();
    }

    private void Move()
    {
       transform.Translate(_currentDirectionV3 * _speed * Time.deltaTime);
        if (transform.position.y < -5f)
        {
            SetPlayerState(PlayerState.idle);
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
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(ray.origin, ray.direction * 5, Color.green);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
        {
            SetPlayerState(PlayerState.build);
            SetControlState(PlayerControlState.uncontrolled);
            
        } else
        {
            Debug.Log(hit.collider.name);
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

    public void SetPlayerState(PlayerState newState)
    {
        if (_playerGameState == newState)
        {
            return;
        }
        _playerGameState = newState;
        switch (newState)
        {
            case PlayerState.idle:
                _currentRoadBlock = null;
                break;
            case PlayerState.run:
                break;
            case PlayerState.fall:
                fallingTween = DOVirtual.Float(0, _speed * 5, 1f, var => { _currentDirectionV3 = Vector3.down * var; }).SetEase(Ease.OutQuad);
                break;
            case PlayerState.build:
                
                if (PlankNumber > 0)
                {
                    if (_currentRoadBlock != null){
                        _currentPlankPlace = new Vector3 (_currentRoadBlock.GetBlockEndXPos() + 0.25f, 0.5f, transform.position.z);
                    }
                    BuildPlank();
                    _currentRoadBlock = null;
                    SetPlayerState(PlayerState.run);
                } else
                {
                    SetPlayerState(PlayerState.fall);
                }
                break;
            case PlayerState.none:
                Debug.LogError("Player state isn't defined");
                break;
        }
    }

    public void SetControlState(PlayerControlState newState)
    {
        if (_playerControlState == newState)
        {
            return;
        }
        Debug.Log("New control state: " + newState);
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
        SetPlayerState(PlayerState.idle);
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
    idle,
    run,
    build,
    fall,
    none
}




public enum PlayerControlState
{
    controlled,
    uncontrolled
}
