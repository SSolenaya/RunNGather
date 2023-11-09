using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private AnimationState _currentAnimationState = AnimationState.none;

    [ContextMenu("Blah-blah")]
    public void Test()
    {
        animator.SetTrigger(PlayerState.run.ToString());
    }


    public void SetAnimationState(PlayerState newPalyerState)
    {
        switch (newPalyerState)
        {
            case PlayerState.build:
            case PlayerState.run:
                ChangeAnimationState(AnimationState.running);
                break;
            case PlayerState.fall:
            case PlayerState.none:
                ChangeAnimationState(AnimationState.falling);
                break;
            case PlayerState.idle:
                ChangeAnimationState(AnimationState.idle);
                break;
        }
    }

    public void ChangeAnimationState(AnimationState newAnimationState)
    {
        if (_currentAnimationState == newAnimationState)
        {
            return;
        }
        _currentAnimationState = newAnimationState;
        switch (_currentAnimationState)
        {
            case AnimationState.running:
                animator.ResetTrigger("idle");
                animator.SetTrigger("run");
                break;
            case AnimationState.falling:
                animator.SetTrigger("fall");
                break;
            case AnimationState.idle:
                animator.SetTrigger("idle");
                break;
        }
    }
}

public enum AnimationState
{
    running,
    falling,
    idle,
    none
}
