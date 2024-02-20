using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPlayerEntityState : BasePlayerEntityState
{
    public BuildPlayerEntityState(PlayerEntity playerEntity) : base(playerEntity)
    {
    }

    public override void OnEnterState()
    {
        _characterAnimator.ChangeAnimationState(AnimationState.Run);
        _playerEntity.Building();
    }
}
