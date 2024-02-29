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
