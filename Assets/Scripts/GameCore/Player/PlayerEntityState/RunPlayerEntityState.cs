using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPlayerEntityState : BasePlayerEntityState
{
    public RunPlayerEntityState(PlayerEntity playerEntity) : base(playerEntity)
    {
    }

    public override void OnEnterState()
    {
        _characterAnimator.ChangeAnimationState(AnimationState.Run);
    }

    public override void OnUpdateState()
    {
        _playerEntity.Running();
    }
}
