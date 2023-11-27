using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public bool IsSoundOn { get; private set; }

    [SerializeField] private AudioClip _clickClip;
    [SerializeField] private AudioClip _gatheringClip;
    [SerializeField] private AudioClip _bonusPickClip;
    [SerializeField] private AudioClip _fallClip;
    [SerializeField] private AudioClip _winningClip;
    [SerializeField] private AudioClip _buildingClip;
    [SerializeField] private AudioSource _effectsAudioSource;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        //IsSoundOn = SaveManager.GetSoundState();      TODO:
        IsSoundOn = true;                           //  temp bear
        _effectsAudioSource.mute = !IsSoundOn;
    }

    public void PlayGatheringSound()
    {
        _effectsAudioSource.PlayOneShot(_gatheringClip);
    }

    public void PlayClickSound()
    {
        _effectsAudioSource.PlayOneShot(_clickClip);
    }

    public void PlayBonusPickingSound()
    {
        _effectsAudioSource.PlayOneShot(_bonusPickClip);
    }

    public void PlayFallingSound()
    {
        _effectsAudioSource.PlayOneShot(_fallClip);
    }

    public void PlayWinningSound()
    {
        _effectsAudioSource.PlayOneShot(_winningClip);
    }

    public void PlayBuildingSound()
    {
        _effectsAudioSource.PlayOneShot(_buildingClip);
    }

    public void SwitchSound()
    {
        IsSoundOn = !IsSoundOn;
        _effectsAudioSource.mute = !IsSoundOn;
    }

    public void SwitchSound(bool isSoundOn)
    {
        IsSoundOn = isSoundOn;
        _effectsAudioSource.mute = !isSoundOn;
    }
}
