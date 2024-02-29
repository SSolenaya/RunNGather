using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerEntityState
{
    protected PlayerEntity _playerEntity;
    protected CharacterAnimator _characterAnimator;

    public BasePlayerEntityState(PlayerEntity playerEntity)
    {
        _playerEntity = playerEntity;
        _characterAnimator = playerEntity.GetCharacterAnimator;
    }

    public virtual void OnEnterState() { }
    public virtual void OnUpdateState() { }
    //public virtual void OnExitState() { }
}
