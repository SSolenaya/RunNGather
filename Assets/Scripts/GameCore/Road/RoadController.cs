using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using Zenject;

public class RoadController : MonoBehaviour
{
    [Inject] public BonusGatesController bonusGatesController;
    [Inject] private PrefabHolder _prefabHolder;
    [Inject] private Settings _settings;
    [Inject] private GameFieldHelper _gameFieldHelper;
    [Inject] private PlayerController _playerController;
    [Inject] private MainLogic _mainLogic;
    [Inject] private DiContainer _diContainer;
    private List<RoadBlock> _roadBlocksList = new List<RoadBlock>();
    private Vector3 _positionForNextBlock = Vector3.zero;
    private PoolManager _roadBlockPoolManager;
    private PoolManager _plankPoolManager;
    private float _roadEndX;

    public void Restart()
    {
        if (_roadBlockPoolManager == null)
        {
            _roadBlockPoolManager = new PoolManager(_prefabHolder.roadBlockPrefab, _settings.startingBlockNumber * 2, _gameFieldHelper, _diContainer);
        }
        if (_plankPoolManager == null)
        {
            _plankPoolManager = new PoolManager(_prefabHolder.plankPrefab, _settings.maxBlockLenght * _settings.startingBlockNumber, _gameFieldHelper, _diContainer);
        }
        ClearExistingRoad();
        switch (_mainLogic.GameMode)
        {
            case GameMode.eternalRunning:
                GenerateRoad(_settings.startingBlockNumber);
                break;
            case GameMode.templatedLevels:
                BuildLevel();
                break;
            default: break;
        }
    }

    private void GenerateRoad(int blockNumber)
    {
        for (int i = 0; i <= blockNumber; i++)
        {
            GenerateOneBlock();
        }
        _playerController.SubscribeForPlayerPosition(CheckForRoadEnding);
    }

    private void GenerateOneBlock()
    {
        BlockData blockData = new BlockData();
        blockData.length = Random.Range(_settings.minBlockLenght, _settings.maxBlockLenght);
        RoadBlock block = _roadBlockPoolManager.GetPoolItem<RoadBlock>();
        _roadEndX = _positionForNextBlock.x - blockData.length;
        blockData.nextAbyssLength = Random.Range(_settings.minBlockLenght /4, (blockData.length + 1));
        block.gameObject.name = "Block_" + _roadBlocksList.Count.ToString();
        block.Setup(this, blockData, _positionForNextBlock, isFinalBlock: false);
        _positionForNextBlock += new Vector3(-(blockData.nextAbyssLength + blockData.length), 0, 0);
        _playerController.SubscribeForPlayerPosition(block.CheckForHiding);
        _roadBlocksList.Add(block);
    }

    private void BuildLevel()
    {
        
      LevelData levelData = _settings.levelTemplatesList[_mainLogic.LevelNumber];
      BuildRoadFromTemplate(levelData);
    }

    private void BuildRoadFromTemplate(LevelData levelData)
    {
        for (int i = 0; i < levelData.blocksList.Count; i++)
        {
            RoadBlock block = _roadBlockPoolManager.GetPoolItem<RoadBlock>();
            block.gameObject.name = "Block_" + i.ToString();
            block.Setup(this, levelData.blocksList[i], _positionForNextBlock, isFinalBlock: i == (levelData.blocksList.Count - 1));
            _positionForNextBlock += new Vector3(-(levelData.blocksList[i].nextAbyssLength + levelData.blocksList[i].length), 0, 0);
            _playerController.SubscribeForPlayerPosition(block.CheckForHiding);
            _roadBlocksList.Add(block);
        }
    }

    private void ClearExistingRoad()
    {
        if (_roadBlocksList.Count == 0) return;
        foreach (var block in _roadBlocksList)
        {
            _roadBlockPoolManager.ReleaseItem(block);
        }
        _roadBlocksList.Clear();
        _positionForNextBlock = Vector3.zero;
    }

    private void CheckForRoadEnding(float newPlayerPosX)
    {
        if ((newPlayerPosX - _roadEndX) < _settings.roadEndDistance)
        {
            GenerateRoad(_settings.startingBlockNumber/2);
        }
    }

    public void HideBlock(RoadBlock roadBlock)
    {
        _roadBlockPoolManager.ReleaseItem(roadBlock);
        _roadBlocksList.Remove(roadBlock);
    }

    public PoolManager GetPlankPoolManager()
    {
        return _plankPoolManager;
    }

}
