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
    private List<AbstractGate> _gateList;

    public GateBuilder(RoadBlock roadBlock, BlockData blockData, int blockScalableArea, Settings settings, BonusGatesController bonusGatesController) {
        _currentBlock = roadBlock;
        _currentBlockPlankParent = _currentBlock.GetObjectsOnBlockParent();
        _blockData = blockData;
        _settings = settings;
        _bonusGatesController = bonusGatesController;
        int distance = _settings.gameMode == GameMode.mathMode ? _settings.mathDistanceBetweenGates : _settings.minDistanceBetweenGates;
        _workingBlockArea = roadBlock.IsFinalBlock ? (blockScalableArea - distance) : blockScalableArea;         // final block has the finish line, so it is necessary to keep its end free from any gates 
        _gateList = new List<AbstractGate>();
    }

    public void GateInstantiation()
    {
        bool math = _settings.gameMode == GameMode.mathMode;
        int distance = math ? _settings.mathDistanceBetweenGates : _settings.minDistanceBetweenGates;
        int maxGatesQuantity = (_workingBlockArea / distance) + 1;
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
                _gateList.Add(_bonusGatesController.GetNextTemplatedGate(_blockData.gateArgs[j]));
            }

        }
        else        // when Settings doesn't have gate args in the current template of a block, so we have to create it randomly (at least 1 gate for a block)
        {
            int r = UnityEngine.Random.Range(1, maxGatesQuantity + 1);
            for (int j = 0; j < r; j++)
            {
                if (math)           //  temp test bear
                {
                    _gateList.Add(_bonusGatesController.GetNextMathGate());
                }
                else
                {
                    _gateList.Add(_bonusGatesController.GetNextGate());
                }
                
            }
        }
        // placing created gates  on the current block
        float freeSpace = (maxGatesQuantity - _gateList.Count) * distance;
        float allowedXPos = 2f;
        for (int i = 0; i < _gateList.Count; i++)
        {
            float delta = UnityEngine.Random.Range(1, 11) * freeSpace / 10;
            float localXCoord = allowedXPos + delta;
            freeSpace -= delta;
            SingleGateInstantiation(_gateList[i], localXCoord);
            allowedXPos = localXCoord + distance;
            int workingAreaBoarder = 2 + _workingBlockArea;
            if (i < (_gateList.Count - 1) && allowedXPos > workingAreaBoarder)
            {
                Debug.LogError("Next gate will cross the block boarder. " + "Gate xPos: " + allowedXPos + "block's working area boarder: " + workingAreaBoarder + "   " + _currentBlock.gameObject.name);
                break;
            }
        }
    }

    private void SingleGateInstantiation(AbstractGate gate, float gatesLocalXCoord)
    {
        gate.transform.SetParent(_currentBlockPlankParent);
        gate.gameObject.transform.localPosition = Vector3.left * gatesLocalXCoord;
        gate.SetParentBlockName(_currentBlock.gameObject.name);
        gate.SetupGates();
        gate.gameObject.SetActive(true);
    }

    public void ReleaseGates()
    {
        foreach (var gate in _gateList)
        {
            _bonusGatesController.ReleaseGate(gate);
        }
        _gateList.Clear();
    }
}
