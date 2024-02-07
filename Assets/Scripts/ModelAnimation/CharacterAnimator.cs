using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private AnimationState _currentAnimationState = AnimationState.Idle;

    [ContextMenu("Blah-blah")]
    public void Test()
    {
        animator.SetTrigger(PlayerState.Run.ToString());
    }


    public void SetAnimationState(PlayerState newPalyerState)
    {
        switch (newPalyerState)
        {
            case PlayerState.Build:
            case PlayerState.Run:
                ChangeAnimationState(AnimationState.Run);
                break;
            case PlayerState.Fall:
                ChangeAnimationState(AnimationState.Fall);
                break;
            case PlayerState.Idle:
                ChangeAnimationState(AnimationState.Idle);
                break;
            case PlayerState.Win:
                ChangeAnimationState(AnimationState.Win);
                break;
        }
    }

    public void ChangeAnimationState(AnimationState newAnimationState)
    {
        if (_currentAnimationState == newAnimationState)
        {
            return;
        }
        animator.ResetTrigger(_currentAnimationState.ToString());
        _currentAnimationState = newAnimationState;
        animator.SetTrigger(_currentAnimationState.ToString());
    }
}

public enum AnimationState
{
    Idle,
    Run,
    Build,
    Win,
    Fall
}
