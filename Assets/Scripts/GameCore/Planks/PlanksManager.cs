using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlanksManager
{
    [Inject] private PrefabHolder _prefabHolder;
    [Inject] private Settings _settings;
    [Inject] private GameFieldHelper _gameFieldHelper;
    [Inject] private DiContainer _diContainer;
    private PoolManager _plankPoolManager;
    private List<Plank> _planksList = new List<Plank>();

    public void Restart()
    {
        if (_plankPoolManager == null)
        {
            _plankPoolManager = new PoolManager(_prefabHolder.plankPrefab, _settings.maxBlockLenght * _settings.startingBlockNumber, _gameFieldHelper, _diContainer);
        }
        ClearExistingPlanks();
    }

    public Plank GetPlank()
    {
        Plank plank = _plankPoolManager.GetPoolItem<Plank>();
        plank.SetBuilder(this);
        plank.gameObject.SetActive(true);
        _planksList.Add(plank);
        return plank;
    }

    public void HidePlank(Plank plank)
    {
        _plankPoolManager.ReleaseItem(plank);
    }

    public void ClearExistingPlanks()
    {
        if (_planksList.Count == 0) return;
        foreach (var plank in _planksList)
        {
            HidePlank(plank);
        }
        _planksList.Clear();
    }
}
