using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Animations;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Vector3 = UnityEngine.Vector3;

public class RoadBlock : MonoBehaviour, IPoolItem
{
    [SerializeField] private GameObject _roadBlockView;
    [SerializeField] private Transform _planksParentTransform;
    [Inject] private Settings _settings;
    [Inject] private PrefabHolder _prefabHolder;
    [Inject] private MainLogic _mainLogic;
    private RoadController _roadController;
    private float _thisBlockEndPosX;
    private BlockData _blockData;
    private List<Plank> _planksList = new List<Plank>();
    private PoolManager _plankPool;
    private BonusGate _bonusGate;
    private FinishLine _finishLine;
    public bool IsInPool { get; set; }

    public void Setup(RoadController roadController, BlockData blockData, Vector3 position, bool isFinalBlock)
    {
        _roadController = roadController;
        _blockData = blockData;
        _plankPool = _roadController.GetPlankPoolManager();
        transform.localPosition = position;
        SetupView(_blockData.length);
        _thisBlockEndPosX = transform.localPosition.x - _blockData.length;
        SetPlanks(blockData.mandatoryPlanksNumber);
        GateInstantiation(blockData);
        if (isFinalBlock)
        {
            FinishLineInstantiation();
        }
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
        
        for (int i = 0; i < _blockData.length; i++)
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
        if (planksNeeded > _blockData.length)
        {
            Debug.LogError(gameObject.name + ": Too many planks are demanded in template");
        }
        List<int> sourceIndexes = new List<int>(_blockData.length);

        for (int i = 0; i < _blockData.length; i++)
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
          if ((0.5f + plankPlacesArray[i]) == _blockData.length/2) continue;
          Plank plank = _plankPool.GetPoolItem<Plank>();
          plank.transform.SetParent(_planksParentTransform);
          plank.SetupPoolManager(_plankPool);
          float zCoord = UnityEngine.Random.Range(-1.35f, 1.35f);
          plank.transform.position = transform.TransformPoint(new Vector3(-(0.5f + plankPlacesArray[i]), 1f, zCoord));
          plank.gameObject.name = "Plank_" + i + "_on_" + gameObject.name;
          plank.gameObject.SetActive(true);
          _planksList.Add(plank);
        }
    }

    public void GateInstantiation(BlockData blockData)
    {
        if (blockData.gateArgs == null)
        {
            _bonusGate = _roadController.bonusGatesController.GetNextGate();
        }
        else
        {
            _bonusGate = _roadController.bonusGatesController.GetNextTemplatedGate(blockData.gateArgs);
        }
        _bonusGate.transform.SetParent(_planksParentTransform);
        _bonusGate.gameObject.transform.localPosition = Vector3.zero;
        _bonusGate.SetParentBlockName(gameObject.name);
        _bonusGate.SetupGates();
        _bonusGate.gameObject.SetActive(true);
    }

    public void FinishLineInstantiation()
    {
        _finishLine = Instantiate(_prefabHolder.finishLinePrefab);
        _finishLine.SubscribeForSuccessfulFinish(()=> _mainLogic.SetGameState(GameState.win));
        _finishLine.transform.SetParent(_planksParentTransform);
        _finishLine.gameObject.transform.localPosition = new Vector3(-(_blockData.length/2 - 1), 1,0);
        _finishLine.gameObject.SetActive(true);
    }

    public void SetupView(float length)
    {
        _roadBlockView.transform.localPosition = new Vector3(-length/2f, 0, 0);
        _roadBlockView.transform.localScale = new Vector3(length, _roadBlockView.transform.localScale.y, _roadBlockView.transform.localScale.z);
        _planksParentTransform.localPosition = new Vector3(-length / 2f, 0, 0);
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
        _roadController.bonusGatesController.GetBonusGatePoolManager().ReleaseItem(_bonusGate);
    }
}
