using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlayerEntityState : BasePlayerEntityState
{
    public FallPlayerEntityState(PlayerEntity playerEntity) : base(playerEntity)
    {
    }

    public override void OnEnterState()
    {
        _playerEntity.ChangeDirectionOnFalling();
        _characterAnimator.ChangeAnimationState(AnimationState.Fall);
    }

    public override void OnUpdateState()
    {
        _playerEntity.Fall();
    }
}
