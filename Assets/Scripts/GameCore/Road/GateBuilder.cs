using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBuilder
{
    private RoadBlock _currentBlock;
    private Transform _currentBlockPlankParent;
    private BlockData _blockData;
    private Settings _settings; 
    private BonusGatesController _bonusGatesController;
    private int _workingBlockArea;
    private List<BonusGate> _bonusGateList;

    public GateBuilder(RoadBlock roadBlock, BlockData blockData, int blockScalableArea, Settings settings, BonusGatesController bonusGatesController) {
        _currentBlock = roadBlock;
        _currentBlockPlankParent = _currentBlock.GetObjectsOnBlockParent();
        _blockData = blockData;
        _settings = settings;
        _bonusGatesController = bonusGatesController;
        _workingBlockArea = roadBlock.IsFinalBlock ? (blockScalableArea - _settings.minDistanceBetweenGates) : blockScalableArea;         // final block has the finish line, so it is necessary to keep its end free from any gates 
        _bonusGateList = new List<BonusGate>();
    }

    public void GateInstantiation()
    {
        
        int maxGatesQuantity = (_workingBlockArea / _settings.minDistanceBetweenGates) + 1;
        if (_blockData.gateArgs != null)        // when Settings has gate args in the current template of a block
        {
            if (_blockData.gateArgs.Count == 0) return;        // when this list is deliberately empty
            int templatedNumber = _blockData.gateArgs.Count;
            int gatesNumber;
            if (templatedNumber > maxGatesQuantity)
            {
                gatesNumber = maxGatesQuantity;
                Debug.LogError("The number of gates in the template is exceeding the maximum: " + _currentBlock.gameObject.name);
            }
            else
            {
                gatesNumber = templatedNumber;
            }

            for (int j = 0; j < gatesNumber; j++)
            {
                _bonusGateList.Add(_bonusGatesController.GetNextTemplatedGate(_blockData.gateArgs[j]));
            }

        }
        else        // when Settings doesn't have gate args in the current template of a block, so we have to create it randomly (at least 1 gate for a block)
        {
            int r = UnityEngine.Random.Range(1, maxGatesQuantity + 1);
            for (int j = 0; j < r; j++)
            {
                _bonusGateList.Add(_bonusGatesController.GetNextGate());
            }
        }
        // created gates placing on the current block
        float freeSpace = (maxGatesQuantity - _bonusGateList.Count) * _settings.minDistanceBetweenGates;
        float allowedXPos = 2f;
        for (int i = 0; i < _bonusGateList.Count; i++)
        {
            float delta = UnityEngine.Random.Range(1, 11) * freeSpace / 10;
            float localXCoord = allowedXPos + delta;
            freeSpace -= delta;
            SingleGateInstantiation(_bonusGateList[i], localXCoord);
            allowedXPos = localXCoord + _settings.minDistanceBetweenGates;
            int workingAreaBoarder = 2 + _workingBlockArea;
            if (i < (_bonusGateList.Count - 1) && allowedXPos > workingAreaBoarder)
            {
                Debug.LogError("Next gate will cross the block boarder. " + "Gate xPos: " + allowedXPos + "block's working area boarder: " + workingAreaBoarder + "   " + _currentBlock.gameObject.name);
                break;
            }
        }
    }

    private void SingleGateInstantiation(BonusGate bonusGate, float gatesLocalXCoord)
    {
        bonusGate.transform.SetParent(_currentBlockPlankParent);
        bonusGate.gameObject.transform.localPosition = Vector3.left * gatesLocalXCoord;
        bonusGate.SetParentBlockName(_currentBlock.gameObject.name);
        bonusGate.SetupGates();
        bonusGate.gameObject.SetActive(true);
    }

    public void ReleaseGates()
    {
        foreach (var gate in _bonusGateList)
        {
            _bonusGatesController.ReleaseGate(gate);
        }
        _bonusGateList.Clear();
    }
}
