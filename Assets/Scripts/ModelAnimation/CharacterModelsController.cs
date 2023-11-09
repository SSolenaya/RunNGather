using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterModelsController
{
    [Inject] private PrefabHolder _prefabHolder;

    public CharacterAnimator GetModelByType (CharacterType charType)
    {
        CharacterAnimator model = GameObject.Instantiate(_prefabHolder.GetModelPrefabByType(charType));
        return model;
    }

}
