using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanksOnBlockBuilder
{
    private RoadBlock _currentBlock;
    private BlockData _blockData;
    private Transform _currentBlockPlankParrent;
    private PlanksManager _planksManager;
    private Settings _settings;

    public PlanksOnBlockBuilder(RoadBlock roadBlock, BlockData blockData, PlanksManager planksManager, Settings settings)
    {
        _currentBlock = roadBlock;
        _blockData = blockData;
        _currentBlockPlankParrent = _currentBlock.GetObjectsOnBlockParent();
        _planksManager = planksManager;
        _settings = settings;
    }

    public void SetPlanks( int planksNeeded)
    {
        
        if (planksNeeded > 0)
        {
            PlanksInstantiation(GetSpecificCells(planksNeeded));
        }
        else
        {
            PlanksInstantiation(GetRandomCells());
        }
    }

    public int[] GetRandomCells()
    {
        List<int> resultList = new List<int>();

        for (int i = 0; i < (_blockData.length - 1); i++)
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
            Debug.LogError(_currentBlockPlankParrent.gameObject.name + ": Too many planks are demanded in template");
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
            Plank plank = _planksManager.GetPlank();
            plank.transform.SetParent(_currentBlockPlankParrent);
            float zCoord = UnityEngine.Random.Range(-1.35f, 1.35f);
            plank.transform.localPosition = new Vector3(-(0.5f + plankPlacesArray[i]), 0.5f, zCoord);
            plank.gameObject.name = "Plank_" + i + "_on_" + _currentBlockPlankParrent.gameObject.name;
            plank.gameObject.SetActive(true);
            _currentBlock.SubscribeForHidingBlock(plank);
        }
    }
}
