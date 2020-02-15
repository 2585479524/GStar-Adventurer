using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    public ChooseLevel Level;
    
    // Start is called before the first frame update
    public void Init(bool IsFrist)
    {
        Level.Init(IsFrist);
    }

    public void LoadFree()
    {
        TurnScene.Instance.Turn(3);
    }
}
