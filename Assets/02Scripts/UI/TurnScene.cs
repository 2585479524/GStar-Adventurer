using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

/// <summary>
/// 转场动画
/// </summary>
public class TurnScene : Singleton<TurnScene>
{
    public RectTransform Mask;
    private AsyncOperation async=null;
    string SceneName;
    bool IsLoad=false;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (IsLoad)
        {
            if (progressValue >= 0.9f)
            {
               
            }
        }
    }
    private int scene;
    private float progressValue;

    public void Turn(int scene)
    {
        Mask.gameObject.SetActive(true);
        IsLoad = true;
        Smaller();
        this.scene = scene;
        if (scene==2)
        {
            SceneName = "02Player";
        }
        else if (scene==1)
        {
            SceneName = "01Start";
        }
        //StartCoroutine(LoadScene());
    }
    private void Smaller()
    {
        Mask.sizeDelta = new Vector2(3000, 3000);
        Mask.DOSizeDelta(Vector2.zero, 1.2f).SetEase(Ease.OutQuad).onComplete += SmallerIsConplete;
    }

    private void Bigger()
    {
        Mask.sizeDelta = new Vector2(0, 0);
        Mask.DOSizeDelta(new Vector2(3000, 3000), 1.2f).onComplete += BiggerIsConplete;
    }

    private void SmallerIsConplete()
    {
        GameManager.Instance.LoadScene(scene);
        //async.allowSceneActivation = true;
        Bigger();
    }

    private void BiggerIsConplete()
    {
         Mask.gameObject.SetActive(false);
    }



    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(SceneName);
        async.allowSceneActivation = false;
        while (!async.isDone)
        { 
            yield return null;
        }
    }
}
