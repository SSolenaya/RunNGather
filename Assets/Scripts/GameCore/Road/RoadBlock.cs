using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Vector3 = UnityEngine.Vector3;

public class RoadBlock : MonoBehaviour, IPoolItem
{
    [SerializeField] private RoadBlockViewBuilder _viewBuilder;
    [SerializeField] private GameObject _blockBody;
    [SerializeField] private Transform _onBlockObjectsParentTransform;
    [Inject] private Settings _settings;
    [Inject] private PrefabHolder _prefabHolder;
    [Inject] private MainLogic _mainLogic;
    [Inject] private GameFieldHelper _gameFieldHelper;
    private RoadController _roadController;
    private float _thisBlockEndPosX;
    private BlockData _blockData;
    private List<Plank> _planksList = new List<Plank>();
    private PoolManager _plankPool;
    private List<BonusGate> _bonusGateList = new List<BonusGate>();
    private FinishLine _finishLine;
    private bool _isFinalBlock;
    public bool IsInPool { get; set; }

    public void Setup(RoadController roadController, BlockData blockData, Vector3 position, bool isFinalBlock)
    {
        _roadController = roadController;
        _blockData = blockData;
        blockData.length = blockData.length <= 6? 6: blockData.length;
        _blockData.length = (blockData.length % 2) > 0 ? blockData.length + 1 : blockData.length;       //  due to minimal size of the scalable block's part model, that equals 2 unity units
        _plankPool = _roadController.GetPlankPoolManager();
        _isFinalBlock = isFinalBlock;
        transform.localPosition = position;
        int blockScalableArea = _blockData.length - 4;  // 2*2=4 - size of starting and ending block's parts together - it is prohibited to build gate there
        SetupView(_blockData.length, blockScalableArea);
        _thisBlockEndPosX = transform.localPosition.x - _blockData.length;
        SetPlanks(_blockData.mandatoryPlanksNumber);
        if (isFinalBlock)
        {
            FinishLineInstantiation();
        }
        GateInstantiation(_blockData, blockScalableArea);
        gameObject.SetActive(true);
    }

    public float GetBlockEndXPos()
    {
        return _thisBlockEndPosX;
    }

    public void SetPlanks(int planksNeeded)
    {
        if (planksNeeded > 0)
        {
            PlanksInstantiation(GetSpecificCells(planksNeeded));
        } else
        {
            PlanksInstantiation(GetRandomCells());
        }
    }

    public int[] GetRandomCells()
    {
        List<int> resultList = new List<int>();
        
        for (int i = 0; i < (_blockData.length-1); i++)
        {
            float r = UnityEngine.Random.Range(0, 100);
            if (r < _settings.plankChance)
            {
                resultList.Add(i);
            }
        }

        return _ = resultList.ToArray();
    }

    public int[] GetSpecificCells(int planksNeeded) 
    {
        if (planksNeeded >= _blockData.length)
        {
            planksNeeded = _blockData.length - 1;
            Debug.LogError(gameObject.name + ": Too many planks are demanded in template");
        }
        List<int> sourceIndexes = new List<int>(_blockData.length);

        for (int i = 0; i < (_blockData.length - 1); i++)
        {
            sourceIndexes.Add(i);
        }

        int[] resultArray = new int[planksNeeded];
        for (int i = 0; i < planksNeeded; i++)
        {
            System.Random random = new System.Random();
            var index = random.Next(sourceIndexes.Count);
            var randomItem = sourceIndexes[index];
            resultArray[i] = randomItem;
            sourceIndexes.RemoveAt(index);
        }
        return resultArray;
    }

    public void PlanksInstantiation(int[] plankPlacesArray)
    {
        for (int i = 0; i < plankPlacesArray.Length; i++)
        {
          Plank plank = _plankPool.GetPoolItem<Plank>();
          plank.transform.SetParent(_onBlockObjectsParentTransform);
          plank.SetupPoolManager(_plankPool);
          float zCoord = UnityEngine.Random.Range(-1.35f, 1.35f);
          plank.transform.localPosition = new Vector3(-(0.5f + plankPlacesArray[i]), 0.5f, zCoord);
          plank.gameObject.name = "Plank_" + i + "_on_" + gameObject.name;
          plank.gameObject.SetActive(true);
          _planksList.Add(plank);
        }
    }

    public void GateInstantiation(BlockData blockData, int workingArea)
    {
        if (_isFinalBlock)         // final block has the finish line, so it is necessary to keep its end free from any gates 
        {
            workingArea =-_settings.minDistanceBetweenGates;
        }
        int maxGatesQuantity = (workingArea / _settings.minDistanceBetweenGates) + 1;
        if (blockData.gateArgs != null)        // when Settings has gate args in the current template of a block
        {
            if (blockData.gateArgs.Count == 0) return;        // when this list is deliberately empty
            int templatedNumber = blockData.gateArgs.Count;
            int gatesNumber;
            if (templatedNumber > maxGatesQuantity)
            {
                gatesNumber = maxGatesQuantity;
                Debug.LogError("The number of gates in the template is exceeding the maximum: " + gameObject.name);
            }
            else
            {
                gatesNumber = templatedNumber;
            }

            for (int j = 0; j < gatesNumber; j++)
            {
                _bonusGateList.Add(_roadController.bonusGatesController.GetNextTemplatedGate(blockData.gateArgs[j]));
            }

        }
        else        // when Settings doesn't have gate args in the current template of a block, so we have to create it randomly (at least 1 gate for a block)
        {
            int r = UnityEngine.Random.Range(1, maxGatesQuantity + 1);
            for (int j = 0; j < r; j++)
            {
                _bonusGateList.Add(_roadController.bonusGatesController.GetNextGate());
            }
        }
        // created gates placing on the current block
        float freeSpace = (maxGatesQuantity - _bonusGateList.Count)* _settings.minDistanceBetweenGates;
        float allowedXPos = 2f;
        for (int i = 0; i < _bonusGateList.Count; i++)
        {
            float delta = UnityEngine.Random.Range(1, 11) * freeSpace/10;
            float localXCoord = allowedXPos + delta;
            freeSpace -= delta;
            SingleGateInstantiation(_bonusGateList[i], localXCoord);
            allowedXPos = localXCoord + _settings.minDistanceBetweenGates;
            int workingAreaBoarder = 2 + workingArea;
            if (i < (_bonusGateList.Count-1) && allowedXPos > workingAreaBoarder) {
                Debug.LogError("Next gate will cross the block boarder. " + "Gate xPos: " + allowedXPos  + "block's working area boarder: " + workingAreaBoarder + "   " + gameObject.name);
                break; 
            }
        }
        
    }

    private void SingleGateInstantiation(BonusGate bonusGate, float gatesLocalXCoord)
    {
        bonusGate.transform.SetParent(_onBlockObjectsParentTransform);
        bonusGate.gameObject.transform.localPosition = Vector3.left * gatesLocalXCoord;
        bonusGate.SetParentBlockName(gameObject.name);
        bonusGate.SetupGates();
        bonusGate.gameObject.SetActive(true);
    }

    public void FinishLineInstantiation()
    {
        _finishLine = Instantiate(_prefabHolder.finishLinePrefab);
        _finishLine.SubscribeForSuccessfulFinish(()=> _mainLogic.SetGameState(GameState.win));
        _finishLine.transform.SetParent(_onBlockObjectsParentTransform);
        _finishLine.gameObject.transform.localPosition = new Vector3(-(_blockData.length - 2), 0.7f,0);
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
        foreach (var plank in _planksList)
        {
            _plankPool.ReleaseItem(plank);
        }
        _planksList.Clear();
        foreach (var gate in _bonusGateList)
        {
            _roadController.bonusGatesController.GetBonusGatePoolManager().ReleaseItem(gate);
        }
        _bonusGateList.Clear();
    }
}
