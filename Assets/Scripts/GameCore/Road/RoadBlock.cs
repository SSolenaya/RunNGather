using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using Vector3 = UnityEngine.Vector3;

public class RoadBlock : MonoBehaviour, IPoolItem
{
    public bool IsInPool { get; set; }
    [SerializeField] private RoadBlockViewBuilder _viewBuilder;
    [SerializeField] private GameObject _blockBody;
    [SerializeField] private Transform _onBlockObjectsParentTransform;
    [Inject] private Settings _settings;
    [Inject] private PrefabHolder _prefabHolder;
    [Inject] private MainLogic _mainLogic;
    [Inject] private GameFieldHelper _gameFieldHelper;
    [Inject] private PlanksManager _planksManager;
    [Inject] private BonusGatesController _bonusGatesController;
    private RoadController _roadController;
    private PlanksOnBlockBuilder _planksOnBlockBuilder;
    private GateBuilder _gateBuilder;
    private float _thisBlockEndPosX;
    private BlockData _blockData;
    private List<Plank> _planksList = new List<Plank>();
    private FinishLine _finishLine;
    public bool IsFinalBlock { get; private set;}
    private ReactiveCommand _onHidingCommand;
    

    public void Setup(RoadController roadController, BlockData blockData, Vector3 position, bool isFinalBlock)
    {
        _onHidingCommand = new ReactiveCommand();
        _roadController = roadController;
        _blockData = blockData;
        blockData.length = blockData.length <= 6? 6: blockData.length;
        _blockData.length = (blockData.length % 2) > 0 ? blockData.length + 1 : blockData.length;       //  due to minimal size of the scalable block's part model, that equals 2 unity units
        IsFinalBlock = isFinalBlock;
        transform.localPosition = position;
        int blockScalableArea = _blockData.length - 4;  // 2*2=4 - size of starting and ending block's parts together
        SetupView(_blockData.length, blockScalableArea);
        _thisBlockEndPosX = transform.localPosition.x - _blockData.length;
        _planksOnBlockBuilder = new PlanksOnBlockBuilder(this, _blockData, _planksManager, _settings);
        _planksOnBlockBuilder.SetPlanks(_blockData.mandatoryPlanksNumber);
        _gateBuilder = new GateBuilder(this, _blockData, blockScalableArea, _settings, _bonusGatesController);
        _gateBuilder.GateInstantiation();
        _onHidingCommand.Subscribe(_ => _gateBuilder.ReleaseGates());
        if (IsFinalBlock)
        {
            FinishLineInstantiation();
        }
        gameObject.SetActive(true);
    }

    public float GetBlockEndXPos()
    {
        return _thisBlockEndPosX;
    }

    public Transform GetObjectsOnBlockParent()
    {
        return _onBlockObjectsParentTransform;
    }

    public void SubscribeForHidingBlock(Plank plank)
    {
        IDisposable dis = _onHidingCommand.Subscribe(_ => plank.HidePlank());
        plank.SetSubscription(dis);
    }

    public void FinishLineInstantiation()
    {
        _finishLine = Instantiate(_prefabHolder.finishLinePrefab);
        _finishLine.SubscribeForSuccessfulFinish(()=> _mainLogic.SetGameState(GameState.win));
        _finishLine.transform.SetParent(_onBlockObjectsParentTransform);
        _finishLine.gameObject.transform.localPosition = new Vector3(-(_blockData.length - 2), 0f,0);
        _finishLine.SetFinishText(_prefabHolder, _gameFieldHelper);
    }

    public void SetupView(int fullLength, int viewLength)
    {
        _viewBuilder.BuildView(viewLength);
        _blockBody.transform.localPosition = new Vector3(-fullLength / 2f, 0, 0);
        _blockBody.transform.localScale = new Vector3(fullLength + 0.8f, 2, 6.5f);
    }

    public  void CheckForHiding(float currentPlayerPosX)
    {
        if(IsInPool) return;
        if ((_thisBlockEndPosX - currentPlayerPosX) > _settings.roadStartDistance)
        {
            _roadController.HideBlock(this);
        }
    }

    GameObject IPoolItem.GetGameObject()
    {
        return gameObject;
    }


    void IPoolItem.Release()
    {
        if (_finishLine != null)
        {
            _finishLine.Release();
            Destroy(_finishLine.gameObject); 
        }

        _onHidingCommand.Execute();
        _onHidingCommand.Dispose();
    }
}
