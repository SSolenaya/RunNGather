using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class EnvironmentObjectsController : MonoBehaviour
{
    [Inject] private PrefabHolder _prefabHolder;
    [Inject] private Settings _settings;
    [Inject] private GameFieldHelper _gameFieldHelper;
    [Inject] private PlayerController _playerController;
    private List<LandscapeObject> _landscapeObjectsList = new List<LandscapeObject>();
    private List<int> _landscapeObjectsIndexes = new List<int> ();
    private Vector3 _endOfLandscapePos;
    private float distance = 900f;        //  check for next landscape piece necesserity
    private float deltaX = 1100f;        //  check for next landscape piece necesserity


    public void Restart()
    {
        _endOfLandscapePos = Vector3.zero;
        ClearAllBlocks();
        SetupBlocks();
        GenerateLandscape();
        _playerController.SubscribeForPlayerPosition(CheckForNExtLandscapeObject);
    }

    private void SetupBlocks()
    {
        foreach (var obj in _prefabHolder.landscapeObjectsList)
        {
            var block = Instantiate(obj, _gameFieldHelper.gameObjParent);
            block.gameObject.SetActive(false);
            _landscapeObjectsList.Add(block);
        }

        FillInIndexList();
    }

    private void GenerateLandscape()
    {
        var lo = GetRandomLandscapeObject();
        ShowLandscapeBlock(lo);
    }

    private void CheckForNExtLandscapeObject(float playersXPos)
    {
        if (Math.Abs(_endOfLandscapePos.x - playersXPos) <= distance)
        {
            GenerateLandscape();
        }
    }

    private LandscapeObject GetRandomLandscapeObject()
    {
        return _landscapeObjectsList[GetRandomIndex()];
    } 

    private int GetRandomIndex()
    {
        if (_landscapeObjectsIndexes.Count == 1)
        {
            int remainedIndex = _landscapeObjectsIndexes[0];
            FillInIndexList();
            _landscapeObjectsIndexes.Remove(remainedIndex);
            return remainedIndex;
        }
        else
        {
            if (_landscapeObjectsIndexes.Count == 0)
            {
                Debug.LogError("The indexes list is empty");
                return -1;
            }
            int r = UnityEngine.Random.Range(0, _landscapeObjectsIndexes.Count);
            int choosenIndex = _landscapeObjectsIndexes[r];
            _landscapeObjectsIndexes.Remove(choosenIndex);
            return choosenIndex;
        }
    }

    private void FillInIndexList()
    {
        for (int i = 0; i < _landscapeObjectsList.Count; i++)
        {
            _landscapeObjectsIndexes.Add(i);
        }
    }

    private void ShowLandscapeBlock(LandscapeObject obj)
    {
        obj.transform.localPosition = _endOfLandscapePos;
        obj.gameObject.SetActive(true);
        _endOfLandscapePos -= Vector3.right * deltaX;
    }

    private void ClearAllBlocks()
    {
        foreach (var item in _landscapeObjectsList)
        {
            if (item.gameObject != null){ 
            Destroy(item.gameObject); }
        }
        _landscapeObjectsList.Clear();
        _landscapeObjectsIndexes.Clear();
    }

}
