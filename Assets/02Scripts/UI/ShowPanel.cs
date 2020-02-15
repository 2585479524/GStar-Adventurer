using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShowPanel : MonoBehaviour
{
    public bool pause = false;

    private void OnEnable()
    {
        Time.timeScale = 1;
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.1f).onComplete += Pause;
    }

    private void Pause()
    {
        if (pause)
        {
            Time.timeScale = 0;
        }
    }
}
