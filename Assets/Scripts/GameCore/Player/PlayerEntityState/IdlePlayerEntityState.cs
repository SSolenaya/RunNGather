using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePlayerEntityState : BasePlayerEntityState
{
    public IdlePlayerEntityState(PlayerEntity playerEntity) : base(playerEntity)
    {
    }

    public override void OnEnterState()
    {
        _playerEntity.CurrentRoadBlock = null;
        _characterAnimator.ChangeAnimationState(AnimationState.Idle);
    }
}
