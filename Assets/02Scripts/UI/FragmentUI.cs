using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FragmentUI : MonoBehaviour
{
    public Transform[] fragmentUI;
    private int hasFragmentAmount=0;

    private void Start()
    {
        hasFragmentAmount = 0;
        for (int i = 0; i < fragmentUI.Length; i++)
        {
            fragmentUI[i].localScale = Vector3.zero;
        }
    }
    public void AddFragment()
    {
        Debug.Log(hasFragmentAmount);
        fragmentUI[hasFragmentAmount++].DOScale(1, 0.3f);
    }

    public int GetFragmentAmount()
    {
        return hasFragmentAmount;
    }

}
