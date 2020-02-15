using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CheckmarkFX : MonoBehaviour
{
    private Tween tween;
    public bool playFirst = false;

    void Start()
    {
        tween = transform.DOScale(1.05f, 0.8f).SetLoops(-1, LoopType.Yoyo);
        if (!playFirst)
        {
            tween.Pause();
        }
    }

    public void SetTween(bool value)
    {
        if (value)
        {
            tween.PlayForward();
        }
        else
        {
            tween.Pause();
        }
    }
}
