using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PoolManager
{
    private Transform _gameObjParent;
    private DiContainer _diContainer;
    private IPoolItem _poolItem;
    private List<IPoolItem> _poolItemList = new List<IPoolItem>();
    private Transform _poolParentRectTransform;


    public PoolManager(IPoolItem poolItem, int poolSize, GameFieldHelper gameFieldHelper, DiContainer diContainer)
    {
        _gameObjParent = gameFieldHelper.gameObjParent;
        _diContainer = diContainer;
        _poolItem = poolItem;
        _poolParentRectTransform = gameFieldHelper.pooledObjParent;
        CreatePool(poolSize);
    }

    private void CreatePool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreatePoolItem<IPoolItem>();
        }
    }

    public T GetPoolItem<T>() where T : IPoolItem
    {
        var poolItem = CreatePoolItem<T>();
        poolItem.IsInPool = false;
        return (T)poolItem;
    }

    private T CreatePoolItem<T>() where T : IPoolItem
    {
         foreach (IPoolItem poolItem in _poolItemList)
         {
             if (poolItem.IsInPool)
             {
                poolItem.GetGameObject().transform.SetParent(_gameObjParent);
                return (T)poolItem;
             }
         }

        GameObject gameObject = GameObject.Instantiate(_poolItem.GetGameObject(), _gameObjParent);
        T item = gameObject.GetComponent<T>();
        _diContainer.Inject(item);
        _poolItemList.Add(item);
        item.IsInPool = true;
        gameObject.gameObject.SetActive(false);
        return item;
    }

    public void ReleaseItem(IPoolItem poolItem)
    {
        if (poolItem == null) return;
        poolItem.IsInPool = true;
        poolItem.Release();
        poolItem.GetGameObject().transform.SetParent(_poolParentRectTransform);
        poolItem.GetGameObject().SetActive(false);
    }

    public void ReleaseAll()
    {
        foreach (IPoolItem poolItem in _poolItemList)
        {
            poolItem.Release();
        }
    }

}

